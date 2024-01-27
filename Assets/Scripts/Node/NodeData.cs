using UnityEngine;

[System.Serializable]
public class NodeData
{
    public uint m_index;
    public int m_bonus;
    public int m_x;
    public int m_y;
    public Sprite m_node_sprite;
    public bool m_is_begin;
    public bool m_is_end;
    public StoryData m_story_data;
}

[System.Serializable]
public class LinkData
{
    public uint m_node_index_a;
    public uint m_node_index_b;
    public int m_cost;
}
