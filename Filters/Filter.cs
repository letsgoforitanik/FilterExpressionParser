using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FilterExpressionParser.Filters;

public class Filter<T> : IDictionary<string, string[]>, IOperatorTree
{
    private readonly string[] validPropertyNames;
    private readonly Dictionary<string, string[]> pairs = [];
    private int currentIndex;

    public Filter()
    {
        validPropertyNames = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(prop => prop.Name).ToArray();
    }

    public string[] this[string key]
    {
        get => pairs[key];
        set
        {
            var keyPascalCase = ToPascalCase(key);

            if (!validPropertyNames.Contains(keyPascalCase)) throw new ArgumentException("Invalid Argument");
            pairs[keyPascalCase] = value;
        }
    }

    public ICollection<string> Keys => pairs.Keys;

    public ICollection<string[]> Values => pairs.Values;

    public int Count => pairs.Count;

    public bool IsReadOnly => false;

    public void Add(string key, string[] value)
    {
        var keyPascalCase = ToPascalCase(key);

        if (!validPropertyNames.Contains(keyPascalCase)) throw new ArgumentException("Invalid Argument");
        pairs.Add(keyPascalCase, value);
    }

    public void Add(KeyValuePair<string, string[]> item)
    {
        var keyPascalCase = ToPascalCase(item.Key);

        if (!validPropertyNames.Contains(keyPascalCase)) throw new ArgumentException("Invalid Argument");
        pairs.Add(keyPascalCase, item.Value);
    }

    public void Clear() => pairs.Clear();

    public bool Contains(KeyValuePair<string, string[]> item)
    {
        return pairs.ContainsKey(item.Key) && pairs[item.Key].SequenceEqual(item.Value);
    }

    public bool ContainsKey(string key) => pairs.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator() => pairs.GetEnumerator();

    public bool Remove(string key) => pairs.Remove(key);

    public bool Remove(KeyValuePair<string, string[]> item) => pairs.Remove(item.Key);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string[] value)
    {
        return pairs.TryGetValue(ToPascalCase(key), out value);
    }

    IEnumerator IEnumerable.GetEnumerator() => pairs.GetEnumerator();
    private static string ToPascalCase(string input)
    {
        return input[..1].ToUpper() + input[1..];
    }
    private OperatorNode[] GetOperatorSequence()
    {
        // this method returns Preorder sequence (Root, Left, Right) of the tree 
        // supports only 'and' among the intermediate operators

        var operatorNodes = new List<OperatorNode>();

        foreach (var key in pairs.Keys)
        {
            var values = pairs[key];

            foreach (var value in values)
            {
                var parts = value.Split(':');
                var op = parts[0];
                var leftOperand = key;
                var rightOperand = parts[1];
                operatorNodes.Add(new LeafOperatorNode(op, leftOperand, rightOperand));
            }

        }

        var count = operatorNodes.Count;

        for (int i = 1; i < count; i++)
        {
            operatorNodes.Insert(0, new OperatorNode("and"));
        }

        return [.. operatorNodes];
    }
    private TreeNode GetTree(OperatorNode[] operatorNodes)
    {
        currentIndex++;

        if (operatorNodes[currentIndex] is LeafOperatorNode leafNode)
        {
            return new TreeNode
            {
                NodeType = leafNode.NodeType,
                Left = new TreeNode(NodeType.Property, leafNode.LeftOperand),
                Right = new TreeNode(NodeType.Operand, leafNode.LeftOperand + ":" + leafNode.RightOperand)
            };
        }

        return new TreeNode
        {
            NodeType = operatorNodes[currentIndex].NodeType,
            Left = GetTree(operatorNodes),
            Right = GetTree(operatorNodes)
        };
    }
    
    public TreeNode GetOperatorTree()
    {
        var operatorSequence = GetOperatorSequence();

        currentIndex = -1;

        return GetTree(operatorSequence);

    }
    
}