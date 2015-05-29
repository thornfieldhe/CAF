
namespace CAF.Model
{
    public partial class LoginLog
    {
        public string StatusName
        {
            get
            {
                return EnumContent.GetDescription<LoginStatusEnum>(this.Status);
            }
        }

    }
}
