using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MouseClickInteraction
{
    private NodeData m_data;
    private Action m_click_callback;
    private Animator m_animator;

    public void Initialize(NodeData data, Action click_callback)
    {
        m_data = data;
        SetClickAction(OnNodeClicked);
        m_click_callback = click_callback;
        transform.localPosition += new Vector3(data.m_x, data.m_y, 0);
        GetComponentInChildren<Image>().sprite = m_data.m_story_sprite;

        // debug
        GetComponentInChildren<TextMeshProUGUI>().text = m_data.m_index.ToString() + "(" + m_data.m_bonus.ToString() + ")";

        m_animator = GetComponentInChildren<Animator>();
        m_animator.enabled = false;
    }

    public void OnNodeClicked()
    {
        if (GameManager.GetInstance().SetActiveNodeIndex(m_data.m_index))
        {
            m_click_callback?.Invoke();
        }
    }

    public uint GetNodeIndex()
    {
        return m_data.m_index;
    }

    public void SetActiveNode(bool active)
    {
        if (active)
        {
            m_animator.enabled = true;
        }
        else
        {
            m_animator.enabled = false;
        }
    }
}
