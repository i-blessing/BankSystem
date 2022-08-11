using BankSystem.Business.Attributes;

namespace BankSystem.Business.Exceptions
{
    public enum BankSystemExceptionStatusCodes
    {
        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        GenericFailure = 0,

        // Account
        [HttpStatusCode(System.Net.HttpStatusCode.Unauthorized)]
        InvalidCredentials = 2000,

        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        DuplicateEmailAddress = 2001,

        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        DuplicateUserName = 2002,

        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        InvalidEmailAddress = 2003,

        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        InvalidPassword = 2004,

        // Transactions
        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        DepositExceedsMaxValue = 3000,

        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        InvalidAccountId = 3001,

        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        WithDrawalExceedsMaxValue = 3001,

        [HttpStatusCode(System.Net.HttpStatusCode.BadRequest)]
        WithDrawalExceedsMinimumBalance = 3002,

        // Token
        [HttpStatusCode(System.Net.HttpStatusCode.Unauthorized)]
        InvalidUserToken = 4000,

        [HttpStatusCode(System.Net.HttpStatusCode.Unauthorized)]
        InvalidUserNameInToken = 4001
    }
}
