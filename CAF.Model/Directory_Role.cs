
namespace CAF.Model
{
    using CAF.Utility;

    public partial class ReadOnlyDirectoryRole
    {
        public string StatusName
        {
            get
            {
                return Enum.GetDescription<RightStatusEnum>(this.Status);
            }
        }


    }
}
