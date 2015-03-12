using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;

    using FineUI;

    public abstract class AuditableBase 
    {
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                PerApprove();
                Approve();
                PostApprove();
                Alert.ShowInTop(Resource.System_Message_Approve);
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                PerReject();
                Reject();
                PostReject();
                Alert.ShowInTop(Resource.System_Message_Reject);
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        protected abstract void Approve();

        protected virtual void PerApprove() { }

        protected virtual void PostApprove() { }

        protected abstract void Reject();

        protected virtual void PerReject() { }

        protected virtual void PostReject() { }
    }
}