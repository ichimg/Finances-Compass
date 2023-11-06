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

        public DebtsService(IDebtAssignmentRepository debtAssignmentRepository,
            IUserRepository userRepository,
            INonUserRepository nonUserRepository,
            IEmailService emailService)
        {
            this.debtAssignmentRepository = debtAssignmentRepository;
            this.userRepository = userRepository;
            this.nonUserRepository = nonUserRepository;
            this.emailService = emailService;
        }

        public async Task<List<DebtDto>> GetAllReceivingDebts(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllReceivingDebtsByEmail(email);

            List<DebtDto> debts = debtsFromDb
                    .Select(d => Mapper.ReceivingDebtAssignmentDbModelToDebtDto(d))
                    .ToList();

            return debts;
        }

        public async Task<List<DebtDto>> GetAllUserDebts(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllUserDebtsByEmail(email);

            List<DebtDto> debts = debtsFromDb.Select(d => Mapper.UserDebtAssignmentDbModelToDebtDto(d)).ToList();

            return debts;
        }

        public async Task CreateDebt(CreateDebtRequest createDebtRequest, string creatorEmail)
        {
            User creatorUser = await userRepository.GetUserByEmail(creatorEmail);
            
            if(creatorUser is null)
            {
                throw new UserNotFoundException(creatorEmail);
            }

            bool isUserAccount = await userRepository.GetUserByEmail(createDebtRequest.Email) != null;

            if (isUserAccount)
            {
                User selectedUser = await userRepository.GetUserByEmail(createDebtRequest.Email);

                if (selectedUser is null)
                {
                    throw new UserNotFoundException(createDebtRequest.Email);
                }

                DebtAssignment debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, selectedUser);
                await debtAssignmentRepository.CreateDebt(debtAssignment);
                return;
            }

            if(!isUserAccount)
            {
                DebtAssignment debtAssignment;
                bool isFirstDebtAdded = false;

                NonUser selectedNonUser = await nonUserRepository.GetNonUserByEmail(createDebtRequest.Email);
                if (selectedNonUser is not null) 
                {
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, selectedNonUser);
                }
                else
                {
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser);
                    isFirstDebtAdded = true;
                }

                await debtAssignmentRepository.CreateDebt(debtAssignment);

                if(isFirstDebtAdded)
                {
                    ReceiverInfoDto receiverInfoDto = Mapper.NonUserToReceiverInfoDto(debtAssignment.NonUser);

                    CreatedDebtEmailInfoDto createdDebtEmailInfoDto = Mapper.UserToCreatedDebtEmailInfoDto(creatorUser);

                    await emailService.SendNoAccountDebtCreatedNotification(receiverInfoDto, createdDebtEmailInfoDto);
                }
            }
        }
    }
}
