using System;

namespace CAF.FSModels
{
    public partial class Message: EFEntity<InfoLog>
    {
        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }

    public partial class Message1 : Message { }
    public partial class Message2 : Message { }
}
