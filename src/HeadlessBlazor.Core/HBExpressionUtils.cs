using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

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

    /// <summary>
    /// Returns the name of the component parameter selected by <paramref name="parameterExpression"/>
    /// (e.g. <c>"Title"</c> for <c>x => x.Title</c>), for use as a key in a parameter dictionary.
    /// Unlike <see cref="GetFieldName{T, TProperty}"/>, an unsupported shape throws rather than
    /// returning <see langword="null"/>: the only shape that yields a usable key is a
    /// <see cref="ParameterAttribute"/>-marked property accessed directly on the lambda's own
    /// parameter, so anything else is a call-site mistake, and throwing reports it there instead of
    /// as an obscure failure when the component is later rendered.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// <paramref name="parameterExpression"/> does not access a property directly on its own lambda
    /// parameter, or that property is not marked with <see cref="ParameterAttribute"/>.
    /// </exception>
    public static string GetComponentParameterName<TComponent, TValue>(Expression<Func<TComponent, TValue>> parameterExpression)
    {
        // Unwrap the conversion the compiler inserts when TValue is not the property's exact type
        // (e.g. `x => x.Count` where the value is supplied as object), so it reads as a bare access.
        var body = parameterExpression.Body is UnaryExpression { NodeType: ExpressionType.Convert } conversion
            ? conversion.Operand
            : parameterExpression.Body;

        // Requiring the owner to be the lambda's own parameter rejects the shapes whose member name
        // would be a misleading key: a nested access (x => x.Foo.Bar, whose name is Bar), a static
        // member, and a captured variable.
        if (body is not MemberExpression member || member.Expression != parameterExpression.Parameters[0])
        {
            throw new ArgumentException(
                $"Expected a parameter of {typeof(TComponent).Name} to be accessed directly (for example, x => x.Title), but found '{parameterExpression}'.",
                nameof(parameterExpression));
        }

        if (member.Member is not PropertyInfo property || !property.IsDefined(typeof(ParameterAttribute), inherit: true))
        {
            throw new ArgumentException(
                $"'{typeof(TComponent).Name}.{member.Member.Name}' is not a component parameter. Mark it with [Parameter] to bind a value to it.",
                nameof(parameterExpression));
        }

        return property.Name;
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
