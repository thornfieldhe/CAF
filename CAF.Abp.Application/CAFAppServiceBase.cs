using Abp.Application.Services;

namespace CAF.Abp.Application
{
    using CAF.Abp.Core;

    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class CAFAppServiceBase : ApplicationService
    {
        protected CAFAppServiceBase()
        {
            this.LocalizationSourceName = CAFConsts.LocalizationSourceName;
        }
    }
}