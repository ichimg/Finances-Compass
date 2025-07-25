﻿using DebtsCompass.Application.Configurations;
using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Net.Http.Headers;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Application.Services
{
    public class PaypalService : IPaypalService
    {
        private readonly PaypalConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IHttpClientFactory httpClientFactory;
        public string BaseUrl => configuration.Mode == "Live" ? configuration.LiveURL
            : configuration.SandboxURL;

        public PaypalService(PaypalConfiguration configuration, IHttpClientFactory httpClientFactory, IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
            this.userRepository = userRepository;
        }

        public async Task<string> GetAccessToken()
        {
            string authentication = $"{configuration.ClientId}:{configuration.ClientSecret}";
            byte[] authenticationBytes = Encoding.UTF8.GetBytes(authentication);
            string base64Authentication = Convert.ToBase64String(authenticationBytes);

            string data = "grant_type=client_credentials";

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/v1/oauth2/token")
            {
                Headers =
                {
                    {HeaderNames.Authorization,  $"Basic {base64Authentication}"},
                },
                Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            using HttpClient httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequestException();
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            var jsonResponse = JObject.Parse(responseBody);
            string accessToken = jsonResponse["access_token"]?.ToString();

            return accessToken;
        }

        public async Task<string> CreateOrder(CreatePaypalOrderRequest createOrderRequest, string userEmail)
        {
            string accessToken = await GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new BadRequestException();
            }

            User payerUser = await userRepository.GetUserByEmail(userEmail);

            if(payerUser.CurrencyPreference is CurrencyPreference.RON)
            {
                createOrderRequest.Value = Math.Round(Decimal.Parse(createOrderRequest.Value) * Decimal.Parse(createOrderRequest.EurExchangeRate), 2).ToString();
            }

            var requestBody = new
            {
                intent = createOrderRequest.Intent.ToUpper(),
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = payerUser.CurrencyPreference == CurrencyPreference.RON ? 
                            CurrencyPreference.EUR.ToString() : payerUser.CurrencyPreference.ToString(),
                            value = createOrderRequest.Value
                        },
                        payee = new
                        {
                            email_address = createOrderRequest.PayeeEmail
                        }
                    }
                }
            };

            string requestBodyJson = JsonConvert.SerializeObject(requestBody);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/v2/checkout/orders")
            {
                Headers =
                {
                    {HeaderNames.Authorization, $"Bearer {accessToken}" }
                },
                Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json")
            };

            using HttpClient httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);

            string orderId = jsonResponse["id"]?.ToString();

            return orderId;
        }

        public async Task<string> CompleteOrder(CompletePaypalOrderRequest completeOrderRequest)
        {
            string accessToken = await GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ForbiddenRequestException();
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/v2/checkout/orders/{completeOrderRequest.OrderId}/{completeOrderRequest.Intent}")
            {
                Headers =
                {
                    {HeaderNames.Authorization, $"Bearer {accessToken}" }
                },
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };


            using HttpClient httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequestException();
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);

            return jsonResponse.ToString();
        }
    }
}