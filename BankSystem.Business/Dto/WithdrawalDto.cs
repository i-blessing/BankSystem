using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business.Dto
{
    public class WithdrawalDto
    {
        [Required]
        public long AccountId { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}
