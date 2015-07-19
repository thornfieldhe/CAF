using System;

namespace CAF.Models
{
    public partial class Message
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

    }

    public partial class Message1 : Message { }
    public partial class Message2 : Message { }
}
