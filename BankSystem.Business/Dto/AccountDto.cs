using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business.Dto
{
    public class AccountDto
    {
        public long AccountId { get; set; }
        public string? AccountNumber { get; set; }
        public double Balance { get; set; }
    }
}
