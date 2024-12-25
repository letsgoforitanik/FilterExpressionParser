using System.Linq.Expressions;
using FilterExpressionParser.Filters;

namespace FilterExpressionParser.Extensions;

public static class IQueryableExtension
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> collection, IOperatorTree tree) where T : class
    {
        var parameter = Expression.Parameter(typeof(T), "item");
        var filterExpression = BuildExpression<T>(tree.GetOperatorTree(), parameter);
        var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
        return collection.Where(lambda);
    }

    private static Expression BuildExpression<T>(TreeNode root, ParameterExpression parameter)
    {
        Expression leftExpr, rightExpr;

        switch (root.NodeType)
        {
            case NodeType.And:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.AndAlso(leftExpr, rightExpr);

            case NodeType.Operand:
                var parts = root.Value!.Split(':');
                var prop = parts[0];
                var propValue = parts[1];
                var property = typeof(T).GetProperty(prop)!;
                return Expression.Constant(Convert.ChangeType(propValue, property.PropertyType));

            case NodeType.Property:
                return Expression.Property(parameter, root.Value!);

            case NodeType.Gte:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.GreaterThanOrEqual(leftExpr, rightExpr);

            case NodeType.Lte:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.LessThanOrEqual(leftExpr, rightExpr);

            case NodeType.Lt:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.LessThan(leftExpr, rightExpr);

            case NodeType.Gt:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.GreaterThan(leftExpr, rightExpr);

            case NodeType.Eq:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.Equal(leftExpr, rightExpr);

            case NodeType.Ne:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.NotEqual(leftExpr, rightExpr);

            case NodeType.Or:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                return Expression.OrElse(leftExpr, rightExpr);

            case NodeType.Pm:
                leftExpr = BuildExpression<T>(root.Left!, parameter);
                rightExpr = BuildExpression<T>(root.Right!, parameter);
                var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
                return Expression.Call(leftExpr, containsMethod, rightExpr);

        }

        return Expression.Empty();
    }

}