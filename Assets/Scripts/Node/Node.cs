using System;
using UnityEngine;

public class Node : MouseClickInteraction
{
    private NodeData m_data;
    private Action m_click_callback;

    public void Initialize(NodeData data, Action click_callback)
    {
        m_data = data;
        SetClickAction(OnNodeClicked);
        m_click_callback = click_callback;
        transform.localPosition += new Vector3(data.m_x, data.m_y, 0);
    }

    public void OnNodeClicked()
    {
        GameManager.GetInstance().SetActiveNodeIndex(m_data.m_index);
        m_click_callback?.Invoke();
    }
}
