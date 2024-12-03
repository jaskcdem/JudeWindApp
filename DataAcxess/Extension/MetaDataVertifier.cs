using GreenUtility;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DataAcxess.Extension
{
    /// <summary> 資料檢核員 </summary>
    public class MetaDataVertifier<T>(T instance, ref List<string> errMsg) where T : class
    {
        T Instance { get; set; } = instance;
        readonly List<string> ErrMsg = errMsg;

        #region <-- PopularVertify -->
        /// <summary>  </summary>
        public void StringVertify((string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsRequired && string.IsNullOrWhiteSpace(vertify.data))
                ErrMsg.Add(helper.GetRequirdFailMessage());
            else
            {
                if (helper.IsMaxLength && vertify.data.Length > helper.MaxLength)
                    ErrMsg.Add(helper.MaxLengthFailMessage());
                if (helper.IsMinxLength && vertify.data.Length < helper.MinxLength)
                    ErrMsg.Add(helper.MinLengthFailMessage());
            }
        }
        /// <summary>  </summary>
        public void StringVertify(params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) StringVertify(vertify);
        }
        /// <summary>  </summary>
        public void RangeVertify((int data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsRange)
            {
                int[] _ran = helper.Range;
                if (!((decimal)vertify.data).Between(_ran[0], _ran[1]))
                    ErrMsg.Add(helper.RangeFailMessage());
            }
        }
        /// <summary>  </summary>
        public void RangeVertify(params (int data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) RangeVertify(vertify);
        }
        #endregion
        #region <-- RegularVertify -->
        /// <summary>  </summary>
        public void RegularVertify((string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsRegular && !helper.RegularIsValid(vertify.data))
                ErrMsg.Add(helper.RegularFailMessage());
        }
        /// <summary>  </summary>
        public void RegularVertify(params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) RegularVertify(vertify);
        }
        /// <summary>  </summary>
        public void RegularVertify(string patten, (string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsRegular && !new Regex(patten).IsMatch(vertify.data))
                ErrMsg.Add(helper.RegularFailMessage());
        }
        /// <summary>  </summary>
        public void RegularVertify(string patten, params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) RegularVertify(patten, vertify);
        }
        /// <summary>  </summary>
        public void RegularVertify(Func<string, bool> vertifior, (string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsRegular && !vertifior.Invoke(vertify.data))
                ErrMsg.Add(helper.RegularFailMessage());
        }
        /// <summary>  </summary>
        public void RegularVertify(Func<string, bool> vertifior, params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) RegularVertify(vertifior, vertify);
        }
        #endregion
        #region <-- EmailVertify -->
        /// <summary>  </summary>
        public void EmailVertify((string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsEmail && !helper.EmailIsValid(vertify.data))
                ErrMsg.Add(helper.EmailFailMessage());
        }
        /// <summary>  </summary>
        public void EmailVertify(params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) EmailVertify(vertify);
        }
        /// <summary>  </summary>
        public void EmailVertify(string patten, (string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsEmail && !new Regex(patten).IsMatch(vertify.data))
                ErrMsg.Add(helper.EmailFailMessage());
        }
        /// <summary>  </summary>
        public void EmailVertify(string patten, params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) EmailVertify(patten, vertify);
        }
        /// <summary>  </summary>
        public void EmailVertify(Func<string, bool> vertifior, (string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsEmail && !vertifior.Invoke(vertify.data))
                ErrMsg.Add(helper.EmailFailMessage());
        }
        /// <summary>  </summary>
        public void EmailVertify(Func<string, bool> vertifior, params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) EmailVertify(vertifior, vertify);
        }
        #endregion
        #region <-- PhoneVertify -->
        /// <summary>  </summary>
        public void PhoneVertify((string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsPhone && !helper.PhonesValid(vertify.data))
                ErrMsg.Add(helper.PhoneFailMessage());
        }
        /// <summary>  </summary>
        public void PhoneVertify(params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) PhoneVertify(vertify);
        }
        /// <summary>  </summary>
        public void PhoneVertify(string patten, (string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsPhone && !new Regex(patten).IsMatch(vertify.data))
                ErrMsg.Add(helper.PhoneFailMessage());
        }
        /// <summary>  </summary>
        public void PhoneVertify(string patten, params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) PhoneVertify(patten, vertify);
        }
        /// <summary>  </summary>
        public void PhoneVertify(Func<string, bool> vertifior, (string data, Expression<Func<T, object>> expression) vertify)
        {
            AttributesHelper<T> helper = new(Instance.GetMemberName(vertify.expression));
            if (helper.IsPhone && !vertifior.Invoke(vertify.data))
                ErrMsg.Add(helper.PhoneFailMessage());
        }
        /// <summary>  </summary>
        public void PhoneVertify(Func<string, bool> vertifior, params (string data, Expression<Func<T, object>> expression)[] vertifies)
        {
            foreach (var vertify in vertifies) PhoneVertify(vertifior, vertify);
        }
        #endregion
        /// <summary>  </summary>
        public string TableName => AttributesHelper<T>.TableName;
        /// <summary>  </summary>
        public string GetColumnName(Expression<Func<T, object>> expression) => new AttributesHelper<T>(Instance.GetMemberName(expression)).ColumnName;
    }
    internal static class NameReaderExtensions
    {
        static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
        static readonly string invalidExpressionMessage = "Invalid expression.";
        #region Method
        internal static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression.Body);
        }
        internal static List<string> GetMemberNames<T>(this T instance, params Expression<Func<T, object>>[] expressions)
        {
            List<string> memberNames = [];
            foreach (var cExpression in expressions)
            {
                memberNames.Add(GetMemberName(cExpression.Body));
            }
            return memberNames;
        }
        internal static string GetMemberName<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetMemberName(expression.Body);
        }
        static string GetMemberName(Expression expression)
        {
            if (expression == null) throw new ArgumentException(expressionCannotBeNullMessage);
            switch (expression)
            {
                case MemberExpression:
                    {
                        // Reference type property or field
                        var memberExpression = (MemberExpression)expression;
                        return memberExpression.Member.Name;
                    }
                case MethodCallExpression:
                    {
                        // Reference type method
                        var methodCallExpression = (MethodCallExpression)expression;
                        return methodCallExpression.Method.Name;
                    }
                case UnaryExpression:
                    {
                        // Property, field of method returning value type
                        var unaryExpression = (UnaryExpression)expression;
                        return GetMemberName(unaryExpression);
                    }
                default:
                    throw new ArgumentException(invalidExpressionMessage);
            }
        }
        static string GetMemberName(UnaryExpression unaryExpression) => unaryExpression.Operand is MethodCallExpression methodExpression
            ? methodExpression.Method.Name
            : ((MemberExpression)unaryExpression.Operand).Member.Name;
        #endregion
    }
}
