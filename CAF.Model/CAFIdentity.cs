using System;
using System.Security.Principal;

namespace CAF.Model
{
    [Serializable()]
    public class CAFIdentity : IIdentity
    {
        internal User User;

        public string AuthenticationType
        {
            get { return "Form"; }
        }

        private bool _isAuthenticated;

        public bool IsAuthenticated
        {
            get { return this._isAuthenticated; }
        }

        private string _name;

        public string Name
        {
            get { return this._name; }
        }

        public static IIdentity GetIdentity(string userName, string userPass)
        {
            var identity = new CAFIdentity();
            var u = User.Get(userName, userPass);
            if (u == null)
            {
                return identity;
            }
            identity.User = u;
            identity._isAuthenticated = true;
            identity._name = u.LoginName;
            return identity;
        }

        internal static CAFIdentity UnauthenticatedIdentity()
        {
            return new CAFIdentity();
        }
    }
}