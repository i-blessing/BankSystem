using BankSystem.Business.Dto;

namespace BankSystem.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CreateAccountAsync(CreateAccountDto dto);
        Task<bool> CreateTransaction(AccountTransactionDto dto);
        Task<bool> DeleteAccountAsync(DeleteAccountDto dto);
        Task<IEnumerable<AccountDto>> GetAccountsForUserAsync(long userId);
    }
}