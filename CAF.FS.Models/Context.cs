
namespace CAF.FSModels
{
    using FS.Core.Data;
    using FS.Mapping.Context.Attribute;

    public class Context : DbContext<Context>
    {
        [Set(Name = "Sys_Directories")]
        public TableSet<Directory> Directories { get; set; }
    }
}
