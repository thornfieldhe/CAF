﻿using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;

    using FineUI;

    public partial class Directory_Query : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("5CCA7546-79EB-4AAE-A7F9-90F9E660A3A3");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected override void Bind()
        {
            base.Bind();
            this.grid_OnQuery();
            this.btnNew.OnClientClick = this.winEdit.GetShowReference("Directory_Edit.aspx", "新增");
        }

        protected override void Delete() { this.grid.Delete<Directory>(); }

        protected void btnDeleteRows_Click(object sender, EventArgs e)
        {
            this.grid.Delete<Directory>();
        }

        protected void gridRowCommand(object sender, GridCommandEventArgs e)
        {
            this.grid.Excute<Directory>(e);
        }

        private void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var criteria = new ReadOnlyDirectory();
            this.grid.BindDataSource(criteria);
        }
    }
}