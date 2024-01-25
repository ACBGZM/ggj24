using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject m_node_prefab;
    public GameObject m_story_panel_prefab;

    public NodeGraph m_node_graph;

    private Adjacency m_adjacency;

    private List<uint> m_found_node_index_list;
    private int m_value;    // todo: move to game manager

    public void Awake()
    {
        m_found_node_index_list = new List<uint>();

        m_node_prefab = Resources.Load<GameObject>("Node");
        m_story_panel_prefab = Resources.Load<GameObject>("StoryPanel");

        m_node_graph = Resources.Load<NodeGraph>("NodeGraphTest");

        m_adjacency = new Adjacency();

        // initialize adjacency
        foreach (LinkData link_data in m_node_graph.m_link_data_list)
        {
            m_adjacency.AddEdge(link_data);
        }
    }

    public void Start()
    {
        foreach (NodeData node_data in m_node_graph.m_node_data_list)
        {
            if (node_data.m_is_begin)
            {
                CreateNodeInstance(node_data);
                break;
            }
        }
    }

    public void NodeClickCallback()
    {
        if (!m_found_node_index_list.Contains(GameManager.GetInstance().GetActiveNodeIndex()))
        {
            m_found_node_index_list.Add(GameManager.GetInstance().GetActiveNodeIndex());

            GameObject story_panel_object = Instantiate(m_story_panel_prefab, transform);
            StoryPanel story_panel = story_panel_object.GetComponent<StoryPanel>();
            story_panel.Initialize(StoryFinishCallback);
        }
    }

    public void StoryFinishCallback()
    {
        NodeData node_data = m_node_graph.m_node_data_list.Find(data=>data.m_index == GameManager.GetInstance().GetActiveNodeIndex());
        if (node_data != null)
        {
            m_value += node_data.m_bonus;

            if(m_value < 0)
            {
                // todo: game over
            }

            List<Edge> adjacent_edges = new List<Edge>();
            if(m_adjacency.GetAdjacentEdges(GameManager.GetInstance().GetActiveNodeIndex(), out adjacent_edges))
            {
                foreach (Edge edge in adjacent_edges)
                {
                    NodeData adjacent_node_data = m_node_graph.m_node_data_list.Find(data => data.m_index == edge.m_target_index);
                    CreateNodeInstance(adjacent_node_data);
                }
            }
        }
    }

    private void CreateNodeInstance(NodeData node_data)
    {
        GameObject node_object = Instantiate(m_node_prefab, transform);
        Node node = node_object.GetComponent<Node>();
        node.Initialize(node_data, NodeClickCallback);
    }
}
