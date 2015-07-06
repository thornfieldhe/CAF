using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace CAF.Abp.Application.Users.Dto
{
    using CAF.Abp.Core.Users;

    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        public string UserName { get; set; }

        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string EmailAddress { get; set; }
    }
}