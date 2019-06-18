using System;
using System.ComponentModel.DataAnnotations;

namespace Kkd.ShortUrl.Modals {
    public class Entity {
        [Key] public Guid Id { get; set; }

        public int Status { get; set; }
        public DateTime LastChange { get; set; }

        [Timestamp] public byte[] RowFlag { get; set; }
    }

    public enum EntityStatus {
        Init = 0,
        Deleted = 1000
    }
}