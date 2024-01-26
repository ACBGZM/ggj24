using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject m_node_prefab;
    public GameObject m_link_prefab;
    public GameObject m_story_panel_prefab;
    public GameObject m_value_panel;

    public NodeGraph m_node_graph;

    private Adjacency m_adjacency;

    private List<uint> m_found_node_index_list;

    private List<Link> m_link_list;

    private uint m_previous_node_index;

    public void Awake()
    {
        m_found_node_index_list = new List<uint>();
        m_link_list = new List<Link>();

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
            SetOtherObjectActive(false);

            m_found_node_index_list.Add(GameManager.GetInstance().GetActiveNodeIndex());

            GameObject story_panel_object = Instantiate(m_story_panel_prefab, transform);
            StoryPanel story_panel = story_panel_object.GetComponent<StoryPanel>();
            story_panel.Initialize(StoryFinishCallback);
        }
    }

    private void CalculateValueAfterMove()
    {
        if(m_adjacency.GetWeight(m_previous_node_index, GameManager.GetInstance().GetActiveNodeIndex(), out int cost))
        {
            GameManager.GetInstance().AddToValue(cost);

            UpdateLinks();
        }
        m_previous_node_index = GameManager.GetInstance().GetActiveNodeIndex();
    }

    public void StoryFinishCallback()
    {
        SetOtherObjectActive(true);

        // calculalte value of previous move and update links
        CalculateValueAfterMove();

        StartCoroutine(ShowAdjacencyNodes());
    }

    private void CreateNodeInstance(NodeData node_data)
    {
        if (m_found_node_index_list.Contains(node_data.m_index))
        {
            return;
        }

        GameObject node_object = Instantiate(m_node_prefab, transform);
        Node node = node_object.GetComponent<Node>();
        node.Initialize(node_data, NodeClickCallback);
    }

    private void CreateLinkInstance(NodeData data_a, NodeData data_b, int m_weight)
    {
        if (HasShowLink(data_a.m_index, data_b.m_index))
        {
            return;
        }

        GameObject link_object = Instantiate(m_link_prefab, transform);
        Link link = link_object.GetComponent<Link>();
        link.Initialize(data_a, data_b, m_weight);

        m_link_list.Add(link);
    }

    private void UpdateLinks()
    {
        foreach(Link link in m_link_list)
        {
            link.UpdateCost();
        }
    }

    private void SetOtherObjectActive(bool active)
    {
        m_value_panel.SetActive(active);

        foreach(Link link in m_link_list)
        {
            link.gameObject.SetActive(active);
        }
    }

    IEnumerator WaitForSomeSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }

    IEnumerator ShowAdjacencyNodes()
    {
        yield return new WaitForSecondsRealtime(1.2f);

        NodeData node_data = m_node_graph.m_node_data_list.Find(data => data.m_index == GameManager.GetInstance().GetActiveNodeIndex());
        if (node_data != null)
        {
            // calculate value of current node
            GameManager.GetInstance().AddToValue(node_data.m_bonus);

            // show new nodes and links
            List<Edge> adjacent_edges = new List<Edge>();
            if (m_adjacency.GetAdjacentEdges(GameManager.GetInstance().GetActiveNodeIndex(), out adjacent_edges))
            {
                foreach (Edge edge in adjacent_edges)
                {
                    NodeData adjacent_node_data = m_node_graph.m_node_data_list.Find(data => data.m_index == edge.m_target_index);
                    if (adjacent_node_data == null)
                    {
                        continue;
                    }
                    CreateNodeInstance(adjacent_node_data);
                    CreateLinkInstance(node_data, adjacent_node_data, edge.m_weight);
                }
            }

            UpdateLinks();
        }
        yield return null;
    }

    private bool HasShowLink(uint index_a, uint index_b)
    {
        uint a = index_a;
        uint b = index_b;
        if (a > b)
        {
            uint temp = a;
            a = b; b = temp;
        }

        return m_link_list.Find(edge => (edge.m_node_index_a == a && edge.m_node_index_b == b)) != null;
    }
}
