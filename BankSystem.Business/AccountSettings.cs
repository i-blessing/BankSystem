using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business
{
    public class AccountSettings
    {
        public double MinimumAccountBalance = 100;
        public double MaximumDepositValue = 10000;
        public double MaximumWithdrawalPercentage = 90;
    }
}
