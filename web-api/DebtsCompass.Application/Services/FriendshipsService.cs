using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;

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

        public async Task<List<UserDto>> GetUserFriendsByEmail(string email)
        {
            User userFromDb = await userRepository.GetUserByEmail(email);

            if (userFromDb is null)
            {
                throw new UserNotFoundException(email);
            }

            IEnumerable<User> friendsFromDb = await friendshipRepository.GetUserFriendsById(userFromDb.Id);

            var userDtos =  friendsFromDb.Select(u => Mapper.UserToUserDto(u)).ToList();

            return userDtos;
        }
    }
}
