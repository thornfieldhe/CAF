using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Model
{
    public partial class ReadOnlyDirectoryRole
    {
        public string StatusName
        {
            get
            {
                return RichEnumContent.GetDescription<RightStatusEnum>(this.Status);
            }
        }

        
    }
}
