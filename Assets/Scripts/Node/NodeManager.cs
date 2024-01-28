using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeManager : MonoBehaviour
{
    public GameObject m_node_prefab;
    public GameObject m_link_prefab;
    public GameObject m_story_panel_prefab;
    public GameObject m_value_panel;

    public NodeGraph m_node_graph;

    private List<uint> m_found_node_index_list;

    private List<Node> m_node_list;
    private List<Link> m_link_list;

    public void Awake()
    {
        m_found_node_index_list = new List<uint>();
        m_node_list = new List<Node>();
        m_link_list = new List<Link>();

        m_node_prefab = Resources.Load<GameObject>("Node");
        m_story_panel_prefab = Resources.Load<GameObject>("StoryPanel");

        //m_node_graph = Resources.Load<NodeGraph>("NodeGraphTest");


        // initialize adjacency
        foreach (LinkData link_data in m_node_graph.m_link_data_list)
        {
            GameManager.GetInstance().m_adjacency.AddEdge(link_data);
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
        uint active_node_index = GameManager.GetInstance().GetActiveNodeIndex();

        // show movement and calculate link value
        if (!GameManager.GetInstance().m_is_first_click)
        {
            uint previous_node_index = GameManager.GetInstance().GetPreviousNodeIndex();
            m_node_list.Find(node => node.GetNodeIndex() == previous_node_index)?.SetActiveNode(false);

            GameManager.GetInstance().MoveToActiveNode();

            UpdateLinks();
        }

        Node active_node = m_node_list.Find(node => node.GetNodeIndex() == active_node_index);
        if (active_node == null)
        {
            return;
        }

        active_node.SetActiveNode(true);

        GameManager.GetInstance().m_is_first_click = false;

        // show story panel
        if (!m_found_node_index_list.Contains(active_node_index))
        {
            SetOtherObjectActive(false);

            m_found_node_index_list.Add(active_node_index);

            GameObject story_panel_object = Instantiate(m_story_panel_prefab, transform);
            StoryPanel story_panel = story_panel_object.GetComponent<StoryPanel>();
            NodeData data = m_node_graph.m_node_data_list.Find(node_data => node_data.m_index == active_node_index);

            story_panel.SetStoryDataList(data.m_story_data);
            story_panel.SetStoryImage(data.m_story_data.m_story_image);

            if (data.m_is_end)
            {
                // todo: ends
                story_panel.Initialize(BackToMenu);
            }
            else
            {
                story_panel.Initialize(StoryFinishCallback);
            }
        }
    }

    public void StoryFinishCallback()
    {
        SetOtherObjectActive(true);

        StartCoroutine(ShowAdjacencyNodes());
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScene");
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

        m_node_list.Add(node);
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
        GameManager.GetInstance().DisableMouseInteraction();

        yield return new WaitForSecondsRealtime(1.0f);

        NodeData node_data = m_node_graph.m_node_data_list.Find(data => data.m_index == GameManager.GetInstance().GetActiveNodeIndex());
        if (node_data != null)
        {
            // calculate value of current node
            GameManager.GetInstance().AddToValue(node_data.m_bonus);

            // show new nodes and links
            List<Edge> adjacent_edges = new List<Edge>();
            if (GameManager.GetInstance().m_adjacency.GetAdjacentEdges(GameManager.GetInstance().GetActiveNodeIndex(), out adjacent_edges))
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

        GameManager.GetInstance().EnableMouseInteraction();

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
