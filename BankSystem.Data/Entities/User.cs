using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(32)]
        public string? UserName { get; set; }

        [Required]
        [StringLength(128)]
        public string? Password { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
