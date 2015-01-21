using System;

namespace SweetDessert.Model.Common
{
    public interface IBussinessEntity
    {
        Guid Id { get; set; }

        string Creator { get; set; }

        string Updator { get; set; }

        DateTime Updated { get; set; }

        DateTime Created { get; set; }

        int Category { get; set; }

        Guid DepId { get; set; }

        int Status { get; set; }
    }
}