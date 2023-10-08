using DebtsCompass.Domain;
using DebtsCompass.Domain.DtoResponses;
using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Interfaces;
using System.Security.Claims;
using DebtsCompass.Domain.Services;

namespace DebtsCompass.Application.Services
{
    public class DebtsService : IDebtsService
    {
        private readonly IDebtAssignmentRepository debtAssignmentRepository;

        public DebtsService(IDebtAssignmentRepository debtAssignmentRepository)
        {
            this.debtAssignmentRepository = debtAssignmentRepository;
        }

        public async Task<List<DebtDto>> GetAll(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllByEmailForExistingUsers(email);

            List<DebtDto> debts = debtsFromDb.Select(d => Mapper.DebtAssignmentDbModelToDebtDto(d)).ToList();

            return debts;
        }
    }
}
