using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Business.Exceptions
{
    public class BankSystemExceptionProperties
    {
        private Dictionary<string, string> _props;
        private BankSystemExceptionProperties() => _props = new Dictionary<string, string>();

        public static BankSystemExceptionProperties Create()
        {
            return new BankSystemExceptionProperties();
        }

        public static BankSystemExceptionProperties Create(params (string name, string value)[] props)
        {
            var newProps = new BankSystemExceptionProperties();

            foreach(var (name, value) in props)
            {
                newProps.Add(name, value);
            }

            return newProps;
        }

        public void Add(string name, string value) => _props.Add(name, value);
        public Dictionary<string,string> Get() => _props;
    }
}
