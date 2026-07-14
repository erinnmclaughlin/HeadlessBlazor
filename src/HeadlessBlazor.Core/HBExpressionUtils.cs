using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace HeadlessBlazor;

internal static class HBExpressionUtils
{
    // Keyed by (model type, dotted member path from the root parameter to the parent of the accessed
    // field, e.g. "Address" for `m => m.Address.City`) so repeated calls for the same nested property
    // path - typically re-evaluated on every Add/Clear call with a freshly-built expression tree - reuse
    // a compiled delegate instead of paying Expression.Compile() each time.
    private static readonly ConcurrentDictionary<(Type ModelType, string Path), Delegate> OwnerDelegateCache = new();

    /// <summary>
    /// Returns the accessed member's name, or <see langword="null"/> if <paramref name="fieldExpression"/>
    /// is not a supported member-access expression - including a static member access, which has no
    /// owning instance for <see cref="GetFieldOwner{T, TProperty}"/> to resolve.
    /// </summary>
    public static string? GetFieldName<T, TProperty>(Expression<Func<T, TProperty>> fieldExpression)
    {
        var memberExpression = GetMemberExpression(fieldExpression);
        return memberExpression?.Expression is null ? null : memberExpression.Member.Name;
    }

    /// <summary>
    /// Resolves the object that directly owns the accessed member, so that nested member expressions
    /// (e.g. <c>m => m.Address.City</c>) resolve to the <c>Address</c> instance rather than <paramref name="model"/>
    /// itself. Only returns <see langword="null"/> when <paramref name="fieldExpression"/> is a supported
    /// shape (i.e. <see cref="GetFieldName{T, TProperty}"/> returned non-null) but an intermediate member
    /// genuinely evaluated to <see langword="null"/> at runtime - callers should treat that as distinct
    /// from an unsupported expression shape.
    /// </summary>
    public static object? GetFieldOwner<T, TProperty>(T model, Expression<Func<T, TProperty>> fieldExpression)
    {
        var memberExpression = GetMemberExpression(fieldExpression);

        if (memberExpression?.Expression is null)
        {
            return null;
        }

        var parentExpression = memberExpression.Expression;
        var root = fieldExpression.Parameters[0];

        if (parentExpression == root)
        {
            return model;
        }

        // Only simple member chains (m.A.B.C) produce a stable path we can safely cache by; anything
        // else (indexers, method calls) is compiled fresh every time, same as before caching existed.
        var ownerDelegate = TryGetMemberPath(parentExpression, root, out var path)
            ? (Func<T, object>)OwnerDelegateCache.GetOrAdd(
                (typeof(T), path),
                _ => CompileOwnerDelegate<T>(parentExpression, fieldExpression.Parameters))
            : CompileOwnerDelegate<T>(parentExpression, fieldExpression.Parameters);

        return ownerDelegate(model);
    }

    private static Func<T, object> CompileOwnerDelegate<T>(Expression parentExpression, IReadOnlyCollection<ParameterExpression> parameters)
    {
        return Expression.Lambda<Func<T, object>>(
            Expression.Convert(parentExpression, typeof(object)),
            parameters).Compile();
    }

    private static bool TryGetMemberPath(Expression expression, ParameterExpression root, out string path)
    {
        var names = new List<string>();
        var current = expression;

        while (current != root)
        {
            if (current is not MemberExpression member)
            {
                path = "";
                return false;
            }

            names.Add(member.Member.Name);
            current = member.Expression!;
        }

        names.Reverse();
        path = string.Join('.', names);
        return true;
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
