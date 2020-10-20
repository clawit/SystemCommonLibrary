using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreateAt { get; set; }

        public string Creator { get; set; }
    }
}
