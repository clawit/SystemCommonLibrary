using System;
using System.ComponentModel.DataAnnotations;

namespace SystemCommonLibrary.Data.DataEntity
{
    public abstract class Entity
    {
        [Key]
        [Column("序号")]
        [Editor(EditorType.Number, Editable = false)]
        public int Id { get; set; }

        [Column("创建时间", Hidden = true)]
        [Editor(EditorType.DateTime, Editable = false)]
        public DateTime CreateAt { get; set; }

        [Column("创建者", Hidden = true)]
        [Editor(EditorType.Text, Editable = false)]
        public string Creator { get; set; }
    }
}
