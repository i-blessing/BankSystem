using BankSystem.Business.Exceptions;
using BankSystem.Data;
using BankSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Tests.UnitTests
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task Withdrawal_Success()
        {
            // arrange
            var contextOptions = new DbContextOptionsBuilder<BankSystemDbContext>()
                                        .UseInMemoryDatabase("withdrawl_success")
                                        .Options;

            // Create the schema and seed some data
            var context = new BankSystemDbContext(contextOptions);

            context.Accounts.Add(new Data.Entities.Account { Id = 2, CurrentBalance = 200, Number = "1234", UserId = 1 });
            context.SaveChanges();

            var service = new AccountService(context, new Business.AccountSettings());
            var dto = new Business.Dto.AccountTransactionDto { AccountId = 2, Amount = 10, Type = Business.Enums.TransactionType.WithDrawal };


            // act
            var r = await service.CreateTransaction(dto);
            var acc = await context.Accounts.FirstOrDefaultAsync(x => x.Id == 2);

            // assert
            Assert.True(r);
            Assert.Equal(190, acc.CurrentBalance);
        }

        [Fact]
        public async Task Withdrawal_Fail_Over_Limit()
        {

            // arrange
            var contextOptions = new DbContextOptionsBuilder<BankSystemDbContext>()
                                        .UseInMemoryDatabase("withdrawl_fail")
                                        .Options;

            // Create the schema and seed some data
            var context = new BankSystemDbContext(contextOptions);
            var service = new AccountService(context, new Business.AccountSettings());
            var dto = new Business.Dto.AccountTransactionDto { AccountId = 1, Amount = 91, Type = Business.Enums.TransactionType.WithDrawal };


            // act
            var r = await Assert.ThrowsAsync<BankSystemException>(() => service.CreateTransaction(dto));

            // assert
            Assert.Equal(BankSystemExceptionStatusCodes.WithDrawalExceedsMinimumBalance, r.StatusCode);
        }

        [Fact]
        public async Task Deposit_Success()
        {

            // arrange
            var contextOptions = new DbContextOptionsBuilder<BankSystemDbContext>()
                                        .UseInMemoryDatabase("deposit_success")
                                        .Options;

            // Create the schema and seed some data
            var context = new BankSystemDbContext(contextOptions);
            var service = new AccountService(context, new Business.AccountSettings());
            var dto = new Business.Dto.AccountTransactionDto { AccountId = 1, Amount = 10, Type = Business.Enums.TransactionType.Deposit };


            // act
            var r = await service.CreateTransaction(dto);
            var acc = await context.Accounts.FirstOrDefaultAsync(x => x.Id == 1);

            // assert
            Assert.True(r);
            Assert.Equal(110, acc.CurrentBalance);
        }

        [Fact]
        public async Task Deposit_Fail_Over_Limit()
        {

            // arrange
            var contextOptions = new DbContextOptionsBuilder<BankSystemDbContext>()
                                        .UseInMemoryDatabase("deposit_fail")
                                        .Options;

            // Create the schema and seed some data
            var context = new BankSystemDbContext(contextOptions);
            var service = new AccountService(context, new Business.AccountSettings());
            var dto = new Business.Dto.AccountTransactionDto { AccountId = 1, Amount = 10001, Type = Business.Enums.TransactionType.Deposit };


            // act
            var r = await Assert.ThrowsAsync<BankSystemException>(() => service.CreateTransaction(dto));

            // assert
            Assert.Equal(BankSystemExceptionStatusCodes.DepositExceedsMaxValue, r.StatusCode);
        }
    }
}