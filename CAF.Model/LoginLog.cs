
namespace CAF.Model
{
    public partial class LoginLog
    {
        public string StatusName
        {
            get
            {
                return RichEnumContent.GetDescription<LoginStatusEnum>(this.Status);
            }
        }

    }
}
