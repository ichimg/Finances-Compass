using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Application.Services
{
    public class FriendshipsService : IFriendshipsService
    {
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUserRepository userRepository;

        public FriendshipsService(IFriendshipRepository friendshipRepository, IUserRepository userRepository)
        {
            this.friendshipRepository = friendshipRepository;
            this.userRepository = userRepository;
        }

        public async Task<PagedList<UserDto>> GetUserFriendsByEmail(string email, PagedParameters pagedParameters)
        {
            User userFromDb = await userRepository.GetUserByEmail(email) ?? throw new UserNotFoundException(email);
            PagedList<User> friendsFromDb = await friendshipRepository.GetUserFriendsById(userFromDb.Id, pagedParameters);

            var userDtos =  friendsFromDb.Select(Mapper.UserToUserDto).ToList();

            var userDtosPagedList = 
                new PagedList<UserDto>(userDtos, friendsFromDb.TotalCount, friendsFromDb.CurrentPage, friendsFromDb.PageSize);

            return userDtosPagedList;
        }
    }
}
