namespace FilterExpressionParser.Filters;

public class OperatorNode
{
    public OperatorNode(string op)
    {
        NodeType = Enum.Parse<NodeType>(op[..1].ToUpper() + op[1..]);
        IsIntermediate = true;
    }
    protected OperatorNode(string op, bool isIntermediate) : this(op)
    {
        IsIntermediate = isIntermediate;
    }

    public NodeType NodeType { get; }
    public bool IsIntermediate { get; }
}