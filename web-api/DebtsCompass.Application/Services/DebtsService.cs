using DebtsCompass.Domain;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;
using EmailSender;

namespace DebtsCompass.Application.Services
{
    public class DebtsService : IDebtsService
    {
        private readonly IDebtAssignmentRepository debtAssignmentRepository;
        private readonly IUserRepository userRepository;
        private readonly INonUserRepository nonUserRepository;
        private readonly IEmailService emailService;
        private readonly ICurrencyRatesJob currencyRatesJob;

        public DebtsService(IDebtAssignmentRepository debtAssignmentRepository,
            IUserRepository userRepository,
            INonUserRepository nonUserRepository,
            IEmailService emailService,
            ICurrencyRatesJob currencyRatesJob)
        {
            this.debtAssignmentRepository = debtAssignmentRepository;
            this.userRepository = userRepository;
            this.nonUserRepository = nonUserRepository;
            this.emailService = emailService;
            this.currencyRatesJob = currencyRatesJob;
        }

        public async Task<List<DebtDto>> GetAllReceivingDebts(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllReceivingDebtsByEmail(email);

            List<DebtDto> debts = debtsFromDb
                    .Select(Mapper.ReceivingDebtAssignmentDbModelToDebtDto)
                    .ToList();

            return debts;
        }

        public async Task<List<DebtDto>> GetAllUserDebts(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllUserDebtsByEmail(email);

            List<DebtDto> debts = debtsFromDb.Select(Mapper.UserDebtAssignmentDbModelToDebtDto).ToList();

            return debts;
        }

        public async Task<Guid> CreateDebt(CreateDebtRequest createDebtRequest, string creatorEmail)
        {
            User creatorUser = await userRepository.GetUserByEmail(creatorEmail) ?? throw new UserNotFoundException(creatorEmail);
            bool isUserAccount = createDebtRequest.IsUserAccount;

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            DebtAssignment debtAssignment;
            if (isUserAccount)
            {
                User selectedUser = await userRepository.GetUserByEmail(createDebtRequest.Email)
                                    ?? throw new UserNotFoundException(createDebtRequest.Email);

                debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, selectedUser, currentCurrencies);
                await debtAssignmentRepository.CreateDebt(debtAssignment);

                ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(debtAssignment.SelectedUser);
                DebtEmailInfoDto createdDebtEmailInfoDto = Mapper.DebtAssignmentToCreatedDebtEmailInfoDto(debtAssignment);
                await emailService.SendDebtCreatedNotification(receiverInfoDto, createdDebtEmailInfoDto);
            }
            else
            {
                NonUser existingNonUser = await nonUserRepository.GetNonUserByEmail(createDebtRequest.Email);

                if (existingNonUser is not null) 
                {
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, existingNonUser, currentCurrencies);
                }
                else
                { 
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, currentCurrencies);
                }
                await debtAssignmentRepository.CreateDebt(debtAssignment);

                ReceiverInfoDto receiverInfoDto = Mapper.NonUserToReceiverInfoDto(debtAssignment.NonUser);
                DebtEmailInfoDto createdDebtEmailInfoDto = Mapper.UserToCreatedDebtEmailInfoDto(creatorUser);
                await emailService.SendNoAccountDebtCreatedNotification(receiverInfoDto, createdDebtEmailInfoDto);
            }

            return debtAssignment.Id;
        }

        public async Task DeleteDebt(string id, string email)
        {
            var debtFromDb = await debtAssignmentRepository.GetDebtById(id);

            if (debtFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            if (!debtFromDb.CreatorUser.Email.Equals(email))
            {
                throw new ForbiddenRequestException();
            }


            if (debtFromDb.SelectedUser is not null)
            {
                ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(debtFromDb.SelectedUser);
                DebtEmailInfoDto deletedDebtEmailInfoDto = Mapper.DebtAssignmentToCreatedDebtEmailInfoDto(debtFromDb);

                await debtAssignmentRepository.DeleteDebt(debtFromDb);
                await emailService.SendDebtDeletedNotification(receiverInfoDto, deletedDebtEmailInfoDto);
            }
            else if (debtFromDb.NonUser is not null)
            {
                ReceiverInfoDto receiverInfoDto = Mapper.NonUserToReceiverInfoDto(debtFromDb.NonUser);
                DebtEmailInfoDto deletedDebtEmailInfoDto = Mapper.DebtAssignmentToCreatedDebtEmailInfoDto(debtFromDb);

                await debtAssignmentRepository.DeleteDebt(debtFromDb);
                await emailService.SendDebtDeletedNotification(receiverInfoDto, deletedDebtEmailInfoDto);
            }
        }

        public async Task EditDebt(EditDebtRequest editDebtRequest, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(editDebtRequest.Guid) ?? throw new EntityNotFoundException();

            bool isUserAccount = editDebtRequest.IsUserAccount;
            DebtAssignment updatedDebt;
            if (isUserAccount)
            {
                User selectedUser = await userRepository.GetUserByEmail(editDebtRequest.Email);
                updatedDebt = Mapper.EditDebtRequestToDebtAssignment(editDebtRequest, selectedUser);
            }
            else
            {
                NonUser nonUser = await nonUserRepository.GetNonUserByEmail(editDebtRequest.Email);
                updatedDebt = Mapper.EditDebtRequestToDebtAssignment(editDebtRequest, nonUser);
            }

            await debtAssignmentRepository.UpdateDebt(debtAssignmentFromDb, updatedDebt);
        }

        public async Task ApproveDebt(string debtId, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.ApproveDebt(debtAssignmentFromDb);
            // TODO: send e-mail notification
        }

        public async Task RejectDebt(string debtId, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.RejectDebt(debtAssignmentFromDb);
            // TODO: send e-mail notification
        }

        public async Task PayDebt(string debtId)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.PayDebt(debtAssignmentFromDb);
        }
    }
}
