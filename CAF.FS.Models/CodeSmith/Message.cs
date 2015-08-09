
namespace CAF.Models
{
    /// <summary>
    /// The message.
    /// </summary>
    public partial class Message : EFEntity<InfoLog>
    {
        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }

    /// <summary>
    /// The message 1.
    /// </summary>
    public partial class Message1 : Message { }

    /// <summary>
    /// The message 2.
    /// </summary>
    public partial class Message2 : Message { }
}
