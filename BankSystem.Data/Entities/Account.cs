using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("User")]
        [Required]
        public long UserId { get; set; }

        [Required]
        [StringLength(16)]
        public string? Number { get; set; }

        public double? CurrentBalance { get; set; }

        public virtual User? User { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}


