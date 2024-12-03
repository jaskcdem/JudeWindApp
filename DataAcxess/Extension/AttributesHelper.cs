using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcxess.Extension
{
    internal class AttributesHelper<T>(string propertyName) where T : class
    {
        string PropName { get; set; } = propertyName;

        #region << Get Vertify Attributes >>
        internal string GetDisplayName()
        {
            var attr = GetCustomAttributes<DisplayAttribute>();
            return attr?.Name ?? string.Empty;
        }
        internal bool IsRequired
        {
            get
            {
                var attr = GetCustomAttributes<RequiredAttribute>();
                return attr != null;
            }
        }
        internal string GetRequirdFailMessage()
        {
            var attr = GetCustomAttributes<RequiredAttribute>();
            string msg = attr?.ErrorMessage?.Replace("[DisplayName]", GetDisplayName()) ?? string.Empty;
            return msg;
        }
        internal bool IsMaxLength
        {
            get
            {
                var attr = GetCustomAttributes<MaxLengthAttribute>();
                return attr != null;
            }
        }
        internal int MaxLength
        {
            get
            {
                var attr = GetCustomAttributes<MaxLengthAttribute>();
                return attr?.Length ?? 5000;
            }
        }
        internal string MaxLengthFailMessage()
        {
            var attr = GetCustomAttributes<MaxLengthAttribute>();
            string msg = attr?.ErrorMessage?.Replace("[DisplayName]", GetDisplayName()).Replace("[Length]", MaxLength.ToString()) ?? string.Empty;
            return msg;
        }
        internal bool IsMinxLength
        {
            get
            {
                var attr = GetCustomAttributes<MinLengthAttribute>();
                return attr != null;
            }
        }
        internal int MinxLength
        {
            get
            {
                var attr = GetCustomAttributes<MinLengthAttribute>();
                return attr?.Length ?? 0;
            }
        }
        internal string MinLengthFailMessage()
        {
            var attr = GetCustomAttributes<MinLengthAttribute>();
            string msg = attr?.ErrorMessage?.Replace("[DisplayName]", GetDisplayName()).Replace("[Length]", MinxLength.ToString()) ?? string.Empty;
            return msg;
        }
        internal bool IsRange
        {
            get
            {
                var attr = GetCustomAttributes<RangeAttribute>();
                return attr != null;
            }
        }
        internal int[] Range
        {
            get
            {
                var attr = GetCustomAttributes<RangeAttribute>();
                return [(int)(attr?.Minimum ?? 0), (int)(attr?.Maximum ?? 5000)];
            }
        }
        internal string RangeFailMessage()
        {
            var attr = GetCustomAttributes<RangeAttribute>();
            int[] _ran = Range;
            string msg = attr?.ErrorMessage?.Replace("[DisplayName]", GetDisplayName()).
                Replace("[Min]", _ran[0].ToString()).Replace("[Max]", _ran[1].ToString()) ?? string.Empty;
            return msg;
        }
        internal bool IsEmail
        {
            get
            {
                var attr = GetCustomAttributes<EmailAddressAttribute>();
                return attr != null;
            }
        }
        internal bool EmailIsValid(object email)
        {
            var attr = GetCustomAttributes<EmailAddressAttribute>();
            return attr?.IsValid(email) ?? true;
        }
        internal string EmailFailMessage()
        {
            var attr = GetCustomAttributes<EmailAddressAttribute>();
            string msg = attr?.ErrorMessage?.Replace("[DisplayName]", GetDisplayName()) ?? string.Empty;
            return msg;
        }
        internal bool IsPhone
        {
            get
            {
                var attr = GetCustomAttributes<PhoneAttribute>();
                return attr != null;
            }
        }
        internal bool PhonesValid(object phone)
        {
            var attr = GetCustomAttributes<PhoneAttribute>();
            return attr?.IsValid(phone) ?? true;
        }
        internal string PhoneFailMessage()
        {
            var attr = GetCustomAttributes<PhoneAttribute>();
            string msg = attr?.ErrorMessage?.Replace("[DisplayName]", GetDisplayName()) ?? string.Empty;
            return msg;
        }
        internal bool IsRegular
        {
            get
            {
                var attr = GetCustomAttributes<RegularExpressionAttribute>();
                return attr != null;
            }
        }
        internal string RegularPattern
        {
            get
            {
                var attr = GetCustomAttributes<RegularExpressionAttribute>();
                return attr?.Pattern ?? string.Empty;
            }
        }
        internal bool RegularIsValid(object val)
        {
            var attr = GetCustomAttributes<RegularExpressionAttribute>();
            return attr?.IsValid(val) ?? true;
        }
        internal string RegularFailMessage()
        {
            var attr = GetCustomAttributes<RegularExpressionAttribute>();
            string msg = attr?.ErrorMessage?.Replace("[DisplayName]", GetDisplayName()) ?? string.Empty;
            return msg;
        }
        #endregion

        #region << Get DB Attribute >>
        internal static string TableName
        {
            get
            {
                var attr = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
                return attr?.Name ?? string.Empty;
            }
        }
        internal string ColumnName
        {
            get
            {
                var attr = GetCustomAttributes<ColumnAttribute>();
                return attr?.Name ?? string.Empty;
            }
        }
        #endregion

        TAttr? GetCustomAttributes<TAttr>() where TAttr : Attribute
        {
            var attrs = typeof(T).GetProperty(PropName)?.GetCustomAttributes(typeof(TAttr), false) as TAttr?[];
            return attrs?[0];
        }
    }
}
