﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.Data;

    using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

    public partial class Organize
    {
        /// <summary>
        /// 获取部门及子部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SortLevelItem> GetChildrenOrganizes(Guid id)
        {
            var item = Get(id);
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                const string query = "Select Id,Name,[Level] From Sys_Organizes Where Level Like @Level+'%' AND Level!=@Level AND Status!=-1";
                return conn.Query<Organize>(query, new { Level = item == null ? "00" : item.Level })
                    .Select(d => new SortLevelItem { Id = d.Id,Level = d.Level, Sort = d.Level.Length/2-1, Name = d.Name })
                    .OrderBy(d => d.Level).ToList();
            }
        }

        /// <summary>
        /// 获取目录，非自身及子目录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SortLevelItem> GetOtherOrganizes(Guid id)
        {
            var item = Get(id);
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                const string query = "Select Id,Name,[Level] From Sys_Organizes Where Level Not Like @Level+'%'  AND Status!=-1";
                return conn.Query<Organize>(query, new { Level = item == null ? "00" : item.Level })
                    .Select(d => new SortLevelItem { Id = d.Id, Level = d.Level, Sort = d.Level.Length / 2 - 1, Name = d.Name })
                    .OrderBy(d => d.Level).ToList();
            }
        }

        /// <summary>
        /// 层级编号支持每层2位，共100项
        /// 如：01,0203,010311
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode(IDbConnection conn, IDbTransaction transaction)
        {
            //获取当前层级最大编号
            const string maxCodeQuery = "Select Max(Level) From Sys_Organizes Where ParentId=@ParentId AND Status<>-1";
            var maxCode = conn.Query<string>(maxCodeQuery, new { ParentId = this.ParentId }, transaction).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(maxCode))//当前层级没有项目
            {
                //获取父对象编号
                const string parentCodeQuery = "Select Level From Sys_Organizes Where Id=@ParentId AND Status<>-1";
                var parentCode = conn.Query<string>(parentCodeQuery, new { ParentId = this.ParentId }, transaction).SingleOrDefault();
                if (string.IsNullOrWhiteSpace(parentCode))//父层级不存在,即为第一条数据
                {
                    return "01";
                }
                else
                {
                    return parentCode + "01";//父级存在，即为父级下第一条数据
                }
            }
            else
            {
                if (maxCode.TrimStart('0').Length % 2 == 0 || maxCode.ToInt() == 9)//当前层级最大编号>=9,层级编号直接+1
                {
                    return (maxCode.ToInt() + 1).ToString();
                }
                else
                {
                    return (maxCode.ToInt() + 1).ToString().PadLeft(maxCode.Length, '0');
                }
            }
        }

        protected override void PreInsert(IDbConnection conn, IDbTransaction transaction)
        {
            this.Level = this.GetMaxCode(conn, transaction);
        }

        protected override void PreUpdate(IDbConnection conn, IDbTransaction transaction)
        {
            if (!this._updateParameters.Contains("ParentId"))
            {
                return;
            }
            var organizes = Organize.Query(o=>o.Where(l=>l.Level.StartsWith(this.Level) && l.Level!=this.Level)
                ,conn, transaction);
                
            var level = this.GetMaxCode(conn, transaction);
            organizes.ForEach(o =>
                {
                    o.Level = level + o.Level.Substring(this.Level.Length, o.Level.Length - this.Level.Length);
                    o.Update(conn, transaction);
                });
            this.Level = level;
        }
    }
}
