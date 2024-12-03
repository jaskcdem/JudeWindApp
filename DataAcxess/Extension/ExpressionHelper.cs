using System.Linq.Expressions;

namespace DataAcxess.Extension
{
    /// <summary>  </summary>
    public static class ExpressionHelper
    {
        /// <summary>  </summary>
        public static (string data, Expression<Func<T, object>> expression) CreateVertify<T>(this string data, Expression<Func<T, object>> expression) => (data, expression);
        /// <summary>  </summary>
        public static (int data, Expression<Func<T, object>> expression) CreateVertify<T>(this int data, Expression<Func<T, object>> expression) => (data, expression);
    }
}
