using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository userRepository;
        private readonly IFriendshipRepository friendshipRepository;
        public UsersService(IUserRepository userRepository, IFriendshipRepository friendshipRepository)
        {
            this.userRepository = userRepository;
            this.friendshipRepository = friendshipRepository;
        }

        public async Task<PagedList<UserDto>> SearchUsers(string query, string email, PagedParameters pagedParameters)
        {
            User currentUser = await userRepository.GetUserByEmail(email);

            var usersFromDb = await userRepository.GetUsersBySearchQuery(query, currentUser, pagedParameters);


            var userDtos = new List<UserDto>();
            foreach (var user in usersFromDb)
            {
                Friendship friendshipFromDb = await friendshipRepository.GetUsersFriendStatus(currentUser, user);

                Status friendStatus;
                if (friendshipFromDb is null)
                {
                    friendStatus = Status.None;
                }
                else
                {
                    friendStatus = friendshipFromDb.Status;
                }

                UserDto userDto = Mapper.UserToUserDto(user, friendStatus);
                userDtos.Add(userDto);
            }

            return new PagedList<UserDto>(userDtos, usersFromDb.TotalCount, usersFromDb.CurrentPage, usersFromDb.PageSize);
        }
    }
}
