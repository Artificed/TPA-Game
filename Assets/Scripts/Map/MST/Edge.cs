
public class Edge
{
    private int _from;
    private int _to;
    private float _weight;

    public Edge(int from, int to, float weight)
    {
        _from = from;
        _to = to;
        _weight = weight;
    }

    public int From
    {
        get => _from;
        set => _from = value;
    }

    public int To
    {
        get => _to;
        set => _to = value;
    }

    public float Weight
    {
        get => _weight;
        set => _weight = value;
    }
}

