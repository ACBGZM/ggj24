using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject m_story_panel_prefab;

    private List<Node> m_nodes;

    public void Awake()
    {
        m_story_panel_prefab = Resources.Load<GameObject>("StoryPanel");
        m_nodes = new List<Node>();
    }

    public void Start()
    {
        m_nodes.AddRange(FindObjectsOfType<Node>());

        foreach (Node node in m_nodes)
        {
            node.m_click_callback = OnNodeClicked;
        }
    }

    public void OnNodeClicked()
    {
        GameObject story_panel = Instantiate(m_story_panel_prefab, transform);
    }
}
