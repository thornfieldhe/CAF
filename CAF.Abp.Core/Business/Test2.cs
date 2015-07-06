using System;

namespace CAF.Abp.Core.Business
{
    using global::Abp.Domain.Entities;

    public class Test2 : Entity<Guid>
    {
        public string Name { get; set; }

        public double AMount { get; set; }


    }
}
