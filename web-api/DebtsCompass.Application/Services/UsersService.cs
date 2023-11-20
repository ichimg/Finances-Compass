using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository userRepository;
        public UsersService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<PagedList<UserDto>> SearchUsers(string query, string email, PagedParameters pagedParameters)
        {
            User currentUser = await userRepository.GetUserByEmail(email);

            var usersFromDb = await userRepository.GetUsersBySearchQuery(query, currentUser, pagedParameters);

            var userDtos = usersFromDb.Select(Mapper.UserToUserDto).ToList(); 

           return new PagedList<UserDto>(userDtos, usersFromDb.TotalCount, usersFromDb.CurrentPage, usersFromDb.PageSize);
        }
    }
}
