using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUserSimilarityService
    {
        double GetCosineSimilarity(double[] vectorA, double[] vectorB);
        double[] GetUserVector(User user, List<ExpenseCategory> allCategories);
    }
}