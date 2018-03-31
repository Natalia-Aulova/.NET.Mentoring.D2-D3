using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace IQueryableExample.Services
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        StringBuilder resultString;

        public string Translate(Expression exp)
        {
            resultString = new StringBuilder();
            Visit(exp);

            return resultString.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                VisitStringInclusionOperations(node);

                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    VisitBinaryEqual(node);
                    break;
                case ExpressionType.AndAlso:
                    VisitBinaryAndAlso(node);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            resultString.Append(GetName(node.Member)).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            resultString.Append(node.Value);

            return node;
        }

        private string GetName(MemberInfo member)
        {
            var attribute = member.GetCustomAttribute<JsonPropertyAttribute>();

            return attribute == null
                ? member.Name
                : attribute.PropertyName;
        }

        private BinaryOperationType GetBinaryOperationType(BinaryExpression node)
        {
            var leftExpressionType = node.Left.NodeType;
            var rightExpressionType = node.Right.NodeType;

            if (leftExpressionType == ExpressionType.MemberAccess &&
                rightExpressionType == ExpressionType.Constant)
            {
                return BinaryOperationType.DirectOrder;
            }

            if (leftExpressionType == ExpressionType.Constant &&
                rightExpressionType == ExpressionType.MemberAccess)
            {
                return BinaryOperationType.ReverseOrder;
            }

            return BinaryOperationType.Unspecified;
        }

        private void VisitStringInclusionOperations(MethodCallExpression node)
        {
            if (node.Object.NodeType != ExpressionType.MemberAccess)
                throw new NotSupportedException($"Operation {node.Object.NodeType} for method {node.Method.Name} is not supported");

            if (node.Arguments == null || node.Arguments.Count != 1)
                throw new NotSupportedException($"Operand for method {node.Method.Name} should be property or field");

            var argument = node.Arguments[0];

            if (argument.NodeType != ExpressionType.Constant)
                throw new NotSupportedException($"A parameter to the method {node.Method.Name} should be a constant");

            Visit(node.Object);
            resultString.Append("(");

            switch (node.Method.Name)
            {
                case "StartsWith":
                    Visit(argument);
                    resultString.Append("*");
                    break;
                case "EndsWith":
                    resultString.Append("*");
                    Visit(argument);
                    break;
                case "Contains":
                    resultString.Append("*");
                    Visit(argument);
                    resultString.Append("*");
                    break;
                default:
                    throw new NotSupportedException($"Method {node.Method.Name} is not supported");
            }

            resultString.Append(")");
        }

        private void VisitBinaryEqual(BinaryExpression node)
        {
            Expression firstNode = null;
            Expression secondNode = null;

            switch (GetBinaryOperationType(node))
            {
                case BinaryOperationType.Unspecified:
                    throw new NotSupportedException("One of operands should be property or field, another one should be constant");
                case BinaryOperationType.DirectOrder:
                    firstNode = node.Left;
                    secondNode = node.Right;
                    break;
                case BinaryOperationType.ReverseOrder:
                    firstNode = node.Right;
                    secondNode = node.Left;
                    break;
            }

            Visit(firstNode);
            resultString.Append("(");
            Visit(secondNode);
            resultString.Append(")");
        }

        private void VisitBinaryAndAlso(BinaryExpression node)
        {
            resultString.Append("(");
            Visit(node.Left);
            resultString.Append(") AND (");
            Visit(node.Right);
            resultString.Append(")");
        }
    }
}
