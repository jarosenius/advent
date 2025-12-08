using System.Collections.Generic;

namespace Advent.Common;

/// <summary>
/// Union-Find for tracking connected components.
/// </summary>
public class UnionFind
{
    private readonly int[] _parent;
    private readonly int[] _rank;
    private int _componentCount;
    public UnionFind(int size)
    {
        _parent = new int[size];
        _rank = new int[size];
        _componentCount = size;

        for (var i = 0; i < size; i++)
        {
            _parent[i] = i;
            _rank[i] = 0;
        }
    }

    /// <summary>
    /// Finds the root of the component containing x.
    /// </summary>
    public int Find(int x)
    {
        if (_parent[x] != x)
            _parent[x] = Find(_parent[x]);
        return _parent[x];
    }

    /// <summary>
    /// Union of the components containing x and y.
    /// Returns true if they were in different components, false if already connected.
    /// </summary>
    public bool Union(int x, int y)
    {
        var rootX = Find(x);
        var rootY = Find(y);

        if (rootX == rootY)
            return false;

        if (_rank[rootX] < _rank[rootY])
            _parent[rootX] = rootY;
        else if (_rank[rootX] > _rank[rootY])
            _parent[rootY] = rootX;
        else
        {
            _parent[rootY] = rootX;
            _rank[rootX]++;
        }

        _componentCount--;
        return true;
    }

    /// <summary>
    /// Gets the number of distinct components.
    /// </summary>
    public int GetComponentCount() => _componentCount;

    /// <summary>
    /// Gets the sizes of all components.
    /// </summary>
    public List<int> GetComponentSizes()
    {
        var componentSizes = new Dictionary<int, int>();

        for (var i = 0; i < _parent.Length; i++)
        {
            var root = Find(i);
            componentSizes[root] = componentSizes.GetValueOrDefault(root, 0) + 1;
        }

        return [.. componentSizes.Values];
    }

    /// <summary>
    /// Gets the size of the component containing x.
    /// </summary>
    public int GetComponentSize(int x)
    {
        var root = Find(x);
        var size = 0;

        for (var i = 0; i < _parent.Length; i++)
        {
            if (Find(i) == root)
                size++;
        }

        return size;
    }

    /// <summary>
    /// Returns true if x and y are in the same component.
    /// </summary>
    public bool IsConnected(int x, int y) => Find(x) == Find(y);
}