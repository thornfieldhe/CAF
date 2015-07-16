using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.FSModels
{
    public partial class Test : EFEntity<Role>
    {

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }

    public partial class Test1 : Test
    {
        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Age:" + this.Age);
        }
    }
    public partial class Test2 : Test
    {
        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Class:" + this.Class);
        } 
    }
}
