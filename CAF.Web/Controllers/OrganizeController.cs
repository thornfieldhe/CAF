﻿
namespace CAF.Web.Controllers
{
    using CAF.Model;
    using System;
    using System.Web.Http;

    public class OrganizeController : ApiController
    {
        public OrganizeList Get()
        {
            return Organize.GetAll();
        }

        public Organize Get(Guid id)
        {
            return Organize.Get(id);
        }

        public void Post(Organize item)
        {
            if (item.IsValid)
            {
                item.Create();
            }
        }

        public void Put(Organize item)
        {
            if (item.IsValid)
            {
                item.Save();
            }
        }

    }
}
