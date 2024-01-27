using TMPro;
using UnityEngine;

public class Link : MonoBehaviour
{
    public int m_cost { get; private set; }
    public uint m_node_index_a { get; private set; }
    public uint m_node_index_b { get; private set; }

    private LineRenderer m_line_renderer;

    private Vector3 m_direction;

    public Texture2D m_texture;

    public void Initialize(NodeData data_a, NodeData data_b, int m_weight)
    {
        m_cost = m_weight;
        m_node_index_a = data_a.m_index;
        m_node_index_b = data_b.m_index;
        if(m_node_index_a > m_node_index_b)
        {
            uint temp = m_node_index_a;
            m_node_index_a = m_node_index_b;
            m_node_index_b = temp;
        }

        m_line_renderer = GetComponent<LineRenderer>();
        if (m_line_renderer == null)
        {
            m_line_renderer = gameObject.AddComponent<LineRenderer>();
        }

        m_texture = Resources.Load<Texture2D>("Line");

        m_line_renderer.material.mainTexture = m_texture;
        m_line_renderer.startWidth = 0.15f;
        m_line_renderer.endWidth = 0.15f;

        Vector3 position_a = new Vector3(data_a.m_x, data_a.m_y, 0.0f);
        Vector3 position_b = new Vector3(data_b.m_x, data_b.m_y, 0.0f);
        m_direction = Vector3.Normalize(position_a - position_b);

        float r;
        r = 70.0f;

        m_line_renderer.SetPosition(0, position_a - m_direction * r);
        m_line_renderer.SetPosition(1, position_b + m_direction * r);
    }

    public void UpdateCost()
    {
        if(GameManager.GetInstance().m_adjacency.GetWeight(m_node_index_a, m_node_index_b, out int weight))
        {
            m_cost = weight;
        }

        // debug
        GetComponentInChildren<TextMeshProUGUI>().text = m_cost.ToString();
        GetComponentInChildren<TextMeshProUGUI>().gameObject.transform.localPosition =
            new Vector3(
                (m_line_renderer.GetPosition(0).x + m_line_renderer.GetPosition(1).x) / 2 + ((m_direction.x > 0.0f) ? (50.0f) : (-50.0f)),
                (m_line_renderer.GetPosition(0).y + m_line_renderer.GetPosition(1).y) / 2 + 50.0f,
                0.0f
            );
    }
}
