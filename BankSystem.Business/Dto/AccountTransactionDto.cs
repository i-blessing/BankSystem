using BankSystem.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business.Dto
{
    public class AccountTransactionDto
    {
        public long AccountId { get; set; }
        public double Amount { get; set; }
        public TransactionType Type { get; set; }
    }
}
