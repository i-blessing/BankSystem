using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business.Exceptions
{
    public class BankSystemException : Exception
    {
        private readonly BankSystemExceptionStatusCodes _statusCode;
        private readonly string _message;
        private readonly BankSystemExceptionProperties _props;

        public BankSystemException(BankSystemExceptionStatusCodes statusCode, string message, BankSystemExceptionProperties props)
            : base(message)
        {
            _statusCode = statusCode;
            _message = message;
            _props = props;
        }

        public BankSystemExceptionStatusCodes StatusCode => _statusCode;
        public BankSystemExceptionProperties Properties => _props;
    }
}
