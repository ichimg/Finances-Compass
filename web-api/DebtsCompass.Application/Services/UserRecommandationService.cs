using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.Application.Services
{
    public class UserRecommandationService : IUserRecommandationService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserSimilarityService similarityService;
        private readonly IExpenseCategoryRepository expenseCategoryRepository;
        public UserRecommandationService(IUserRepository userRepository, IUserSimilarityService similarityService, IExpenseCategoryRepository expenseCategoryRepository)
        {
            this.userRepository = userRepository;
            this.similarityService = similarityService;
            this.expenseCategoryRepository = expenseCategoryRepository;
        }

        public async Task<List<UserDto>> RecommendSimilarUsers(string targetUserEmail, int numRecommendations)
        {
            var targetUser = await userRepository.GetUserByEmail(targetUserEmail);

            if(!targetUser.IsDataConsent)
            {
                throw new BadRequestException();
            }

            var allUsers = await userRepository.GetAllAllowedDataConsent(targetUserEmail);
            var allCategories = await expenseCategoryRepository.GetAllByEmail(targetUserEmail);

            var targetUserVector = similarityService.GetUserVector(targetUser, allCategories);
            var recommendedUsers = new List<RecommendedUserDto>();


            foreach (var user in allUsers)
            {
                var userVector = similarityService.GetUserVector(user, allCategories);
                double cosineSimilarity = similarityService.GetCosineSimilarity(targetUserVector, userVector);
                recommendedUsers.Add(Mapper.UserToRecommendedUserDto(user, userVector, cosineSimilarity));
            }

            recommendedUsers = recommendedUsers.OrderByDescending(u => u.CosineSimilarity).ToList();
            var userDtos = recommendedUsers.Take(numRecommendations).Select(ru => Mapper.RecommendedUserDtoToUserDto(ru, Status.None, false)).ToList();

            return userDtos;
        }
    }
}
