
namespace CAF.FSModels
{
    using FS.Core.Data.Table;
    using FS.Mapping.Context.Attribute;


    public class Table : BaseTableContext<Table>
    {
        [Set(Name = "Sys_Posts")]
        public TableSet<Post> Posts { get; set; }
    }
}
