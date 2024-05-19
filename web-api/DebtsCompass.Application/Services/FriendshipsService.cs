using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
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

            var userDtos = friendsFromDb.Select(u =>
            {
                return Mapper.UserToUserDto(u, Status.Accepted); // assuming it's indeed a friend that has mandatory the request accepted
            }).ToList();

            var userDtosPagedList =
                new PagedList<UserDto>(userDtos, friendsFromDb.TotalCount, friendsFromDb.CurrentPage, friendsFromDb.PageSize);

            return userDtosPagedList;
        }

        public async Task<List<UserDto>> GetAllUserFriends(string email)
        {
            User userFromDb = await userRepository.GetUserByEmail(email) ?? throw new UserNotFoundException(email);
            List<User> friendsFromDb = await friendshipRepository.GetAllUserFriendsById(userFromDb.Id);

            var userDtos = friendsFromDb.Select(u =>
            {
                return Mapper.UserToUserDto(u, Status.Accepted); // assuming it's indeed a friend that has mandatory the request accepted
            }).ToList();

            return userDtos;
        }

        public async Task<PagedList<UserDto>> GetUserFriendRequestsById(string email, PagedParameters pagedParameters)
        {
            User userFromDb = await userRepository.GetUserByEmail(email) ?? throw new UserNotFoundException(email);
            PagedList<User> friendsFromDb = await friendshipRepository.GetUserFriendRequestsById(userFromDb.Id, pagedParameters);

            var userDtos = friendsFromDb.Select(u =>
            {
                return Mapper.UserToUserDto(u, Status.Pending, true); // assuming it's indeed a friend request that has mandatory the request pending
            }).ToList();

            var userDtosPagedList =
                new PagedList<UserDto>(userDtos, friendsFromDb.TotalCount, friendsFromDb.CurrentPage, friendsFromDb.PageSize);

            return userDtosPagedList;
        }

        public async Task AddFriend(FriendRequest friendRequest)
        {
            User requesterUser = await userRepository.GetUserByEmail(friendRequest.RequesterUserEmail) ?? throw new UserNotFoundException(friendRequest.RequesterUserEmail);
            User receiverUser = await userRepository.GetUserByEmail(friendRequest.ReceiverUserEmail) ?? throw new UserNotFoundException(friendRequest.ReceiverUserEmail);

            Friendship friendship = Mapper.FriendRequestToFriendship(friendRequest, requesterUser, receiverUser);

            await friendshipRepository.Add(friendship);
        }

        public async Task DeleteFriendRequest(FriendRequestDto deleteFriendRequest)
        {
            User requesterUser = await userRepository.GetUserByEmail(deleteFriendRequest.RequesterUserEmail) ?? throw new UserNotFoundException(deleteFriendRequest.RequesterUserEmail);
            User receiverUser = await userRepository.GetUserByEmail(deleteFriendRequest.SelectedUserEmail) ?? throw new UserNotFoundException(deleteFriendRequest.SelectedUserEmail);

            Friendship friendshipFromDb = await friendshipRepository.GetUsersFriendship(requesterUser.Id, receiverUser.Id) ?? throw new EntityNotFoundException();

            await friendshipRepository.Delete(friendshipFromDb);
        }

        public async Task AcceptFriendRequest(FriendRequestDto friendRequestDto)
        {
            User requesterUser = await userRepository.GetUserByEmail(friendRequestDto.RequesterUserEmail) ?? throw new UserNotFoundException(friendRequestDto.RequesterUserEmail);
            User receiverUser = await userRepository.GetUserByEmail(friendRequestDto.SelectedUserEmail) ?? throw new UserNotFoundException(friendRequestDto.SelectedUserEmail);

            Friendship friendshipFromDb = await friendshipRepository.GetUsersFriendship(receiverUser.Id, requesterUser.Id) ?? throw new EntityNotFoundException();

            await friendshipRepository.AcceptFriendRequest(friendshipFromDb);
        }

        public async Task RejectFriendRequest(FriendRequestDto friendRequestDto)
        {
            User requesterUser = await userRepository.GetUserByEmail(friendRequestDto.RequesterUserEmail) ?? throw new UserNotFoundException(friendRequestDto.RequesterUserEmail);
            User receiverUser = await userRepository.GetUserByEmail(friendRequestDto.SelectedUserEmail) ?? throw new UserNotFoundException(friendRequestDto.SelectedUserEmail);

            Friendship friendshipFromDb = await friendshipRepository.GetUsersFriendship(receiverUser.Id, requesterUser.Id) ?? throw new EntityNotFoundException();

            await friendshipRepository.RejectFriendRequest(friendshipFromDb);
        }
    }
}
