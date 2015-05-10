using System;
using System.Linq;
using System.Security.Principal;

namespace CAF.Model
{

    [Serializable]
    public class CAFPrincipal : IPrincipal
    {
        public CAFPrincipal() { }

        private CAFPrincipal(IIdentity identity) { this._identity = identity; }

        public bool IsInRole(string role)
        {
            var identity = (CAFIdentity)this.Identity;
            return identity.User.Roles.Count(r => r.Id == new Guid(role)) > 0;
        }

        public bool Login(string username, string password)
        {
            this.Identity = CAFIdentity.GetIdentity(username, password);
            return this.Identity.IsAuthenticated;
        }

        public static void Logout()
        {
            //使用未经验证的标记方法，声明一个标准方法
            var identity = CAFIdentity.UnauthenticatedIdentity();
            var principal = new CAFPrincipal(identity);
        }

        private IIdentity _identity;

        public IIdentity Identity
        {
            get { return this._identity; }
            private set { this._identity = value; }
        }
    }
}