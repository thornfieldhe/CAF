
namespace CAF.Model
{
    using CAF.Utility;

    public partial class LoginLog
    {
        public string StatusName
        {
            get
            {
                return Enum.GetDescription<LoginStatusEnum>(this.Status);
            }
        }

    }
}
