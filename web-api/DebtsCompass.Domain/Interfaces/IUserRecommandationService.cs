using DebtsCompass.Domain.Entities.DtoResponses;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUserRecommandationService
    {
        Task<List<UserDto>> RecommendSimilarUsers(string targetUserEmail, int numRecommendations);
    }
}