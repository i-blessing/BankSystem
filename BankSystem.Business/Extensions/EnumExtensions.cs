
using BankSystem.Business.Attributes;

namespace BankSystem.Business.Extensions
{
    public static class EnumExtensions
    {
        public static int GetHttpStatusCode(this Enum e)
        {
            var defaultStatusCode = 400;
            var attr = e.GetAttribute<HttpStatusCodeAttribute>();
            return attr == null ? defaultStatusCode : attr.GetValue();
        }

        private static T? GetAttribute<T>(this Enum e) where T : Attribute
        {
            var enumType = e.GetType();
            var memberInfo = enumType.GetMember(e.ToString());
            var attrs = memberInfo[0].GetCustomAttributes(typeof(T), false);

            return attrs != null && attrs.Length > 0 ? (T)attrs[0] : null;
        }
    }
}
