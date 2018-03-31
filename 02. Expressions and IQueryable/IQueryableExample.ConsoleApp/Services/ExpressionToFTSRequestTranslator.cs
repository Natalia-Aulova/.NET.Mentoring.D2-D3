using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace IQueryableExample.ConsoleApp.Services
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

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (!(node.Left.NodeType == ExpressionType.MemberAccess))
                        throw new NotSupportedException(string.Format("Left operand should be property or field", node.NodeType));

                    if (!(node.Right.NodeType == ExpressionType.Constant))
                        throw new NotSupportedException(string.Format("Right operand should be constant", node.NodeType));

                    Visit(node.Left);
                    resultString.Append("(");
                    Visit(node.Right);
                    resultString.Append(")");
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
            var attr = member.GetCustomAttribute<JsonPropertyAttribute>();

            return attr == null
                ? member.Name
                : attr.PropertyName;
        }
    }
}
