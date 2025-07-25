﻿using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IIncomeRepository
    {
        Task CreateIncome(Income income);
        Task DeleteIncome(Income income);
        Task<Income> GetIncomeById(string id);
        Task UpdateIncome(Income incomeFromDb, Income updatedIncome);
    }
}