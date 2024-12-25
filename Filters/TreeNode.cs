namespace FilterExpressionParser.Filters;

public class TreeNode
{
    public TreeNode() { }

    public TreeNode(NodeType op, string value)
    {
        NodeType = op;
        Value = value;
    }

    public NodeType NodeType { get; set; }
    public string? Value { get; set; }
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }
}