using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HttpStatusCodeAttribute : Attribute
    {
        private readonly int _value;

        public HttpStatusCodeAttribute(int value)        {
            _value = value;
        }

        public HttpStatusCodeAttribute(System.Net.HttpStatusCode httpStatusCode)
        {
            _value = (int)httpStatusCode;
        }

        public int GetValue() => _value;
        public System.Net.HttpStatusCode GetHttpStatusCode() => (System.Net.HttpStatusCode)_value;  

    }
}
