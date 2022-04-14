using System;
using System.ComponentModel.DataAnnotations;

namespace TestEng.Data.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
    }
}
