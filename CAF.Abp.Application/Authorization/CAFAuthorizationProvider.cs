using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Abp.Application
{
    using global::Abp.Authorization;
    using global::Abp.Localization;

    public class CAFAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //TODO: Localize (Change FixedLocalizableString to LocalizableString)

            context.CreatePermission("CanCreateQuestions", new FixedLocalizableString("Can create questions"));
            context.CreatePermission("CanAnswerToQuestions", new FixedLocalizableString("Can answer to questions"), isGrantedByDefault: true);
        }
    }
}
