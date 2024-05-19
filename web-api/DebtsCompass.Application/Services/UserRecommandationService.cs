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

            int guidSum = targetUser.Id.Where(char.IsDigit).Sum(c => c - '0');
            var random = new Random(DateTime.Now.Day + guidSum);
            allUsers = allUsers.OrderBy(u => random.Next()).ToList();

            foreach (var user in allUsers)
            {
                var userVector = similarityService.GetUserVector(user, allCategories);
                recommendedUsers.Add(Mapper.UserToRecommendedUserDto(user, userVector));

                if (recommendedUsers.Count >= numRecommendations)
                {
                    break;
                }
            }

            recommendedUsers.Sort((u1, u2) =>
            {
                var similarity1 = -similarityService.GetCosineSimilarity(targetUserVector, u1.UserVector);
                var similarity2 = -similarityService.GetCosineSimilarity(targetUserVector, u2.UserVector);

                return similarity1.CompareTo(similarity2);
            });

            var userDtos = new List<UserDto>();
            foreach (var recommendedUser in recommendedUsers)
            {
                UserDto userDto = Mapper.RecommendedUserDtoToUserDto(recommendedUser, Status.None, false);
                userDtos.Add(userDto);
            }

            return userDtos;
        }
    }
}
