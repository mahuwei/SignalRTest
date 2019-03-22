using System;
using System.ComponentModel;

namespace MySqlTest.Models {
    /// <summary>
    ///     所有数据实体的基类
    /// </summary>
    public abstract class Entity : ICloneable {
        /// <summary>
        ///     实体主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        ///     行标识，每次修改都会变更
        /// </summary>
        public byte[] RowFlag { get; set; }

        /// <summary>
        ///     最后变更时间
        /// </summary>
        public DateTime LastChange { get; set; }

        /// <summary>
        ///     不在数据库中存储，仅用于判断，记录是否修改过，需要保存
        /// </summary>
        public bool IsChanged { get; set; }

        public virtual string Memo { get; set; }

        public object Clone() {
            return MemberwiseClone();
        }

        /// <summary>
        ///     是否是新实体；如果是，同时生成Id。
        /// </summary>
        /// <returns></returns>
        public bool IsNew() {
            if (Id != Guid.Empty) return false;
            Id = Guid.NewGuid();
            return true;
        }
    }

    /// <summary>
    ///     实体状态
    /// </summary>
    public enum EntityStatus {
        [Description("正常")] Init = 0,

        [Description("已删除")] Deleted = 1000
    }
}