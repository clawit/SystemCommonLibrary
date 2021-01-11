using System;
using System.ComponentModel.DataAnnotations;

namespace SystemCommonLibrary.Data.DataEntity
{
    public abstract class Entity
    {
        [Key]
        [Display(Name = "序号")]
        public int Id { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "创建者")]
        public string Creator { get; set; }
    }
}
