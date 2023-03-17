namespace DoraTheExplorer.Structure;

public class Vertex<T>
    where T : notnull
{
    private static int id = 0;
    public readonly int Id;
    public T Info;
    private Vertex<T>? left;
    private Vertex<T>? up;
    private Vertex<T>? right;
    private Vertex<T>? down;

    public Vertex(T info)
    {
        this.Info = info;
        this.Id = id;
        id++;
    }

    public Vertex<T>? Left
    {
        get => left;
    }
    public Vertex<T>? Right
    {
        get => right;
    }
    public Vertex<T>? Up
    {
        get => up;
    }
    public Vertex<T>? Down
    {
        get => down;
    }

    public void ConnectLeft(Vertex<T> left)
    {
        this.left = left;
        left.right = this;
    }

    public void ConnectRight(Vertex<T> right)
    {
        this.right = right;
        right.left = this;
    }

    public void ConnectUp(Vertex<T> up)
    {
        this.up = up;
        up.down = this;
    }

    public void ConnectDown(Vertex<T> down)
    {
        this.down = down;
        down.up = this;
    }
}
