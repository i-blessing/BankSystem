using BankSystem.Business;
using BankSystem.Business.Dto;
using BankSystem.Business.Exceptions;
using BankSystem.Data;
using BankSystem.Data.Entities;
using BankSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankSystemDbContext _dbContext;
        private readonly AccountSettings _accountSettings;

        public AccountService(BankSystemDbContext dbContext, AccountSettings accountSettings)
        {
            this._dbContext = dbContext;
            this._accountSettings = accountSettings;
            GenerateTestData();
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsForUserAsync(long userId)
        {
            return _dbContext.Accounts.Where(x => x.UserId == userId).Select(a => new AccountDto
            {
               AccountId = a.Id,
               AccountNumber = a.Number,
               Balance = a.CurrentBalance ?? 0
            });
        }

        public async Task<bool> CreateAccountAsync(CreateAccountDto dto)
        {
            await _dbContext.Accounts.AddAsync(new Account
            {
                CurrentBalance = 0,
                Number = GenerateAccountNumber(),
                UserId = dto.UserId
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAccountAsync(DeleteAccountDto dto)
        {
            var acc = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == dto.AccountId);
            var props = BankSystemExceptionProperties.Create();
            props.Add("AccountId", dto.AccountId.ToString());

            if (acc == null)
            {
                throw new BankSystemException(BankSystemExceptionStatusCodes.InvalidAccountId, "Invalid account id", props);
            }

            _dbContext.Accounts.Remove(acc);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateTransaction(AccountTransactionDto dto)
        {
            var props = BankSystemExceptionProperties.Create();

            var account = _dbContext.Accounts.FirstOrDefault(x => x.Id == dto.AccountId);
            props.Add("AccountId", dto.AccountId.ToString());

            if (account == null)
            {
                throw new BankSystemException(BankSystemExceptionStatusCodes.InvalidAccountId, "Invalid account id", props);
            }

            switch (dto.Type)
            {
                case Business.Enums.TransactionType.WithDrawal:
                    ProcessWithDrawal(dto, props, account);
                    break;
                case Business.Enums.TransactionType.Deposit:
                    ProcessDeposit(dto, props, account);
                    break;
                default:
                    throw new Exception("Invalid Transaction Type");
            }

            var transaction = await _dbContext.Transactions.AddAsync(new Transaction
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                TransactionDate = DateTime.UtcNow,
                Type = dto.Type,
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }

        private string GenerateAccountNumber()
        {
            Random rnd = new Random();

            var r = rnd.NextInt64(1000000000, 9999999999);

            return r.ToString();
        }

        private void ProcessDeposit(AccountTransactionDto dto, BankSystemExceptionProperties props, Account account)
        {
            ValidateDeposit(dto, props);

            account.CurrentBalance += dto.Amount;
        }

        private void ProcessWithDrawal(AccountTransactionDto dto, BankSystemExceptionProperties props, Account account)
        {
            ValidateWithDrawal(dto, props, account);

            account.CurrentBalance -= dto.Amount;
        }

        private void ValidateWithDrawal(AccountTransactionDto dto, BankSystemExceptionProperties props, Account account)
        {
            if (dto.Type == Business.Enums.TransactionType.WithDrawal)
            {
                var currentBalance = account?.CurrentBalance.GetValueOrDefault(0) ?? 0;
                var balanceAfter = currentBalance - dto.Amount;
                var ninetyPercent = currentBalance * 0.9;

                props.Add("CurrentBalance", currentBalance.ToString());
                props.Add("WithDrawlAmount", dto.Amount.ToString());

                if (balanceAfter < _accountSettings.MinimumAccountBalance)
                {
                    throw new BankSystemException(BankSystemExceptionStatusCodes.WithDrawalExceedsMinimumBalance, "WithDrawal will cause balance to go under minimum balance", props);
                }

                if (ninetyPercent < dto.Amount)
                {
                    throw new BankSystemException(BankSystemExceptionStatusCodes.WithDrawalExceedsMaxValue, "WithDrawal exceeds max value", props);
                }
            }
        }

        private void ValidateDeposit(AccountTransactionDto dto, BankSystemExceptionProperties props)
        {
            if (dto.Type == Business.Enums.TransactionType.Deposit)
            {
                if (dto.Amount > _accountSettings.MaximumDepositValue)
                {
                    props.Add("DepositValue", dto.Amount.ToString());
                    throw new BankSystemException(BankSystemExceptionStatusCodes.DepositExceedsMaxValue, $"Deposit value max {_accountSettings.MaximumDepositValue}", props);
                }
            }
        }

        private void GenerateTestData()
        {
            if (_dbContext.Accounts.Any()) return;

            _dbContext.Users.Add(new Data.Entities.User
            {
                 Id = 1,
                 Password = "encrypt_this",
                 UserName = "test"
            });

            _dbContext.Accounts.Add(new Data.Entities.Account
            {
                CurrentBalance = 100,
                Id = 1,
                Number = "1234567890",
                UserId = 1
            });

            _dbContext.SaveChanges();
        }
    }
}