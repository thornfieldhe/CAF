using Abp.Domain.Entities.Auditing;
using CAF.Abp.Core.Users;
using System;

namespace CAF.Abp.Core.Business
{
    public class Test1 : CreationAuditedEntity<Guid, User>
    {
        public bool IsSuccess { get; set; }

        public int Index { get; set; }


    }
}
