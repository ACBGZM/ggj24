using System.Collections.Generic;
public class Edge
{
    public uint m_target_index;
    public int m_weight;

    public Edge(uint tareget, int weight)
    {
        m_target_index = tareget;
        m_weight = weight;
    }
}

public class Adjacency
{
    public Adjacency()
    {
        m_adjacency = new Dictionary<uint, List<Edge>>();
    }

    private Dictionary<uint, List<Edge>> m_adjacency;

    public void AddEdge(uint a, uint b, int cost)
    {
        if (a > b)
        {
            uint temp = a;
            a = b;
            b = temp;
        }

        if (!m_adjacency.ContainsKey(a))
        {
            m_adjacency[a] = new List<Edge>();
        }
        m_adjacency[a].Add(new Edge(b, cost));

        if (!m_adjacency.ContainsKey(b))
        {
            m_adjacency[b] = new List<Edge>();
        }
        m_adjacency[b].Add(new Edge(a, cost));
    }

    public void AddEdge(LinkData link_data)
    {
        AddEdge(link_data.m_node_index_a, link_data.m_node_index_b, link_data.m_cost);
    }

    public bool GetWeight(uint a, uint b, out int weight)
    {
        if (a > b)
        {
            uint temp = a;
            a = b;
            b = temp;
        }

        if (m_adjacency.ContainsKey(a))
        {
            foreach (Edge edge in m_adjacency[a])
            {
                if (edge.m_target_index == b)
                {
                    weight = edge.m_weight;
                    edge.m_weight = 0;  // set weight to zero when it is reached

                    m_adjacency[b].Find(edge => edge.m_target_index == a).m_weight = 0;

                    return true;
                }
            }
        }

        weight = -1;
        return false;
    }

    public bool GetAdjacentEdges(uint index, out List<Edge> edges)
    {
        if (m_adjacency.ContainsKey(index))
        {
            edges = m_adjacency[index];
            return true;
        }

        edges = null;
        return false;
    }
}
