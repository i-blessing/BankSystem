using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business.Dto
{
    public class DepositDto
    {
        [Required]
        public long AccountId { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}
