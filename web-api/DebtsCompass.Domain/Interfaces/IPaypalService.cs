﻿using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IPaypalService
    {
        string BaseUrl { get; }

        Task<string> GetAccessToken();
        Task<string> CreateOrder(CreatePaypalOrderRequest createOrderRequest, string userEmail);
        Task<string> CompleteOrder(CompletePaypalOrderRequest completeOrderRequest);
    }
}