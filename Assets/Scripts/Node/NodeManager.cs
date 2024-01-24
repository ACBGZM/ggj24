using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private static NodeManager instance = new NodeManager();
    public static NodeManager GetInstance() => instance;

    public GameObject m_node_prefab;
    public GameObject m_story_panel_prefab;

    public NodeGraph m_node_graph;

    private uint m_current_node_index;
    private int m_value;

    public void Awake()
    {
        m_node_prefab = Resources.Load<GameObject>("Node");
        m_story_panel_prefab = Resources.Load<GameObject>("StoryPanel");

        m_node_graph = Resources.Load<NodeGraph>("NodeGraphTest");
    }

    public void Start()
    {
        foreach (NodeData node_data in m_node_graph.m_node_data_list)
        {
            // todo: show begin node only
            GameObject node_object = Instantiate(m_node_prefab, transform);
            Node node = node_object.GetComponent<Node>();
            node.Initialize(node_data, NodeClickCallback);
        }
    }

    public void NodeClickCallback()
    {
        GameObject story_panel_object = Instantiate(m_story_panel_prefab, transform);
        StoryPanel story_panel = story_panel_object.GetComponent<StoryPanel>();
        story_panel.Initialize(StoryFinishCallback);
    }

    public void SetActiveNode(uint index)
    {
        m_current_node_index = index;
    }

    public void StoryFinishCallback()
    {
        NodeData node_data = m_node_graph.m_node_data_list.Find(data=>data.m_index == m_current_node_index);
        if (node_data != null)
        {
            m_value += node_data.m_bonus;

            if(m_value > 0)
            {
                // todo: game over
            }

            // todo: show new nodes
        }
    }

}
