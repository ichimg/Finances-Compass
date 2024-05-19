using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.Application.Services
{
    public class UserSimilarityService : IUserSimilarityService
    {
        public UserSimilarityService() { }

        public double[] GetUserVector(User user, List<ExpenseCategory> allCategories)
        {
            var userVector = new double[allCategories.Count];
            var userExpenses = user.Expenses.GroupBy(e => e.Category.Name).ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            for (int i = 0; i < allCategories.Count; i++)
            {
                var category = allCategories[i];
                userVector[i] = userExpenses.ContainsKey(category.Name) ? Convert.ToDouble(userExpenses[category.Name]) : 0;
            }

            return userVector;
        }

        public double GetCosineSimilarity(double[] vectorA, double[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
            {
                throw new ArgumentException("Vectors must have the same length");
            }

            var dotProduct = 0.0;
            var magnitudeA = 0.0;
            var magnitudeB = 0.0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }

            double denominator = Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB);
            if (denominator == 0)
            {
                return 0;
            }

            return dotProduct / denominator;
        }
    }
}
