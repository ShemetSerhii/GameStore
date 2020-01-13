using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline
{
    public class ExpressionMergingVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ExpressionMergingVisitor(Expression oldValue,
            Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }

    }
}
