namespace FilterExpressionParser.Filters;

public class LeafOperatorNode(string op, string leftOperand, string rightOperand) : OperatorNode(op, false)
{
    public string LeftOperand { get; } = leftOperand;
    public string RightOperand { get; } = rightOperand;
}