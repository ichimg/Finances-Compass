using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Interfaces;
using System.Security.Claims;
using DebtsCompass.Domain.Services;
using DebtsCompass.Domain.Entities.DtoResponses;

namespace DebtsCompass.Application.Services
{
    public class DebtsService : IDebtsService
    {
        private readonly IDebtAssignmentRepository debtAssignmentRepository;

        public DebtsService(IDebtAssignmentRepository debtAssignmentRepository)
        {
            this.debtAssignmentRepository = debtAssignmentRepository;
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
    }
}
