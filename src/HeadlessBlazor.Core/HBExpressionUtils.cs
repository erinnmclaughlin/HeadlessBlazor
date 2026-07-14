using System.Linq.Expressions;

namespace HeadlessBlazor;

internal static class HBExpressionUtils
{
    public static string? GetFieldName<T, TProperty>(Expression<Func<T, TProperty>> fieldExpression)
    {
        return GetMemberExpression(fieldExpression)?.Member.Name;
    }

    /// <summary>
    /// Resolves the object that directly owns the accessed member, so that nested member expressions
    /// (e.g. <c>m => m.Address.City</c>) resolve to the <c>Address</c> instance rather than <paramref name="model"/>
    /// itself.
    /// </summary>
    public static object? GetFieldOwner<T, TProperty>(T model, Expression<Func<T, TProperty>> fieldExpression)
    {
        var memberExpression = GetMemberExpression(fieldExpression);

        if (memberExpression?.Expression is null)
        {
            return null;
        }

        if (memberExpression.Expression == fieldExpression.Parameters[0])
        {
            return model;
        }

        var ownerLambda = Expression.Lambda<Func<T, object>>(
            Expression.Convert(memberExpression.Expression, typeof(object)),
            fieldExpression.Parameters);

        return ownerLambda.Compile().Invoke(model);
    }

    private static MemberExpression? GetMemberExpression<T, TProperty>(Expression<Func<T, TProperty>> fieldExpression)
    {
        return fieldExpression.Body switch
        {
            MemberExpression memberExpression => memberExpression,
            UnaryExpression { Operand: MemberExpression memberOperand } => memberOperand,
            _ => null
        };
    }
}
