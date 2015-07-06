using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Abp.Application.Users
{
    using CAF.Abp.Application.Users.Dto;
    using CAF.Abp.Core.Users;

    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly UserManager _userManager;

        public UserAppService(UserManager userManager)
        {
            _userManager = userManager;
        }

        public ListResultOutput<UserDto> GetUsers()
        {
            return new ListResultOutput<UserDto>
                   {
                       Items = _userManager.Users.ToList().MapTo<List<UserDto>>()
                   };
        }

        public int Test() { return 1; }


    }
}