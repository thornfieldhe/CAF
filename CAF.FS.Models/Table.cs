
namespace CAF.FS.Models
{
    using CAF.FS.Core.Data.Table;
    using CAF.FS.Mapping.Context.Attribute;

    public class Table : TableContext<Table>
    {
        [Set(Name = "Sys_Posts")]
        public TableSet<Post> Posts { get; set; }
    }
}
