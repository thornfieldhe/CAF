using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Abp.Application.Configuration
{
    using global::Abp.Configuration;

    public class MySettingProvider: SettingProvider
    {
        public const string DefaultPageSize = "DefaultPageSize";

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                   {
                       new SettingDefinition(DefaultPageSize, "10")
                   };
        }
    }
}
