using System;

namespace CAF.Mapper
{
    public interface IMapper
    {
        /// <summary>
        /// Mapper Source Type 
        /// </summary>
        Type SourceType { get; }
        /// <summary>
        /// Mapper Destination Type
        /// </summary>
        Type DestinationType { get; }
    }
}
