﻿using System;

namespace CAF.FS.Mapping.Context.Attribute
{
    /// <summary>
    /// 设置字段在数据库中的映射关系
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FieldAttribute : System.Attribute
    {
        /// <summary>
        /// 设置字段在数据库中的映射关系
        /// </summary>
        public FieldAttribute()
        {
            this.IsMap = true;
            this.IsPrimaryKey = false;
        }

        /// <summary>
        /// 数据库字段名称（映射）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否为数据库主键（自动标识）
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 插入时字段状态（默认允许修改）
        /// </summary>
        public StatusType InsertStatusType { get; set; }

        /// <summary>
        /// 修改时字段状态（默认允许修改）
        /// </summary>
        public StatusType UpdateStatusType { get; set; }

        /// <summary>
        /// 是否映射到数据库字段中(默认为true)
        /// </summary>
        public bool IsMap { get; set; }

        /// <summary>
        /// 指示字段是否为存储过程中输出的参数
        /// （默认为false)
        /// </summary>
        public bool IsOutParam { get; set; }

        /// <summary>
        /// 指示字段是否为存储过程中输入的参数
        /// （默认为false)
        /// </summary>
        public bool IsInParam { get; set; }
    }

    /// <summary>
    /// 字段状态
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// 允许修改
        /// </summary>
        CanWrite,
        /// <summary>
        /// 只读状态，不作任何设置
        /// </summary>
        ReadOnly,
        /// <summary>
        /// 只读状态，但如果存在值时，将转换成==条件
        /// </summary>
        ReadCondition,
    }
}
