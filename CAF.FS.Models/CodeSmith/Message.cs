using System;

namespace CAF.FSModels
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

    }

    public class Message1 : Message { }
    public class Message2 : Message { }
}
