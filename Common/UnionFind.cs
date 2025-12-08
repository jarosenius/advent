using System.Collections.Generic;

namespace Advent.Common;

public class UnionFind
{
    private readonly int[] _parent;
    private readonly int[] _rank;

    public UnionFind(int size)
    {
        _parent = new int[size];
        _rank = new int[size];

        for (var i = 0; i < size; i++)
        {
            _parent[i] = i;
            _rank[i] = 0;
        }
    }
    public int Find(int x)
    {
        if (_parent[x] != x)
            _parent[x] = Find(_parent[x]);
        return _parent[x];
    }

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

        return true;
    }

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
}