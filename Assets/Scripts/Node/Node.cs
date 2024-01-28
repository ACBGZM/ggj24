using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MouseClickInteraction
{
    private NodeData m_data;
    private Action m_click_callback;
    private Animator m_animator;

    private int m_runtime_bonus;

    public void Initialize(NodeData data, Action click_callback)
    {
        m_data = data;
        m_runtime_bonus = m_data.m_bonus;
        SetClickAction(OnNodeClicked);
        m_click_callback = click_callback;
        transform.localPosition += new Vector3(data.m_x, data.m_y, 0);
        GetComponentInChildren<Image>().sprite = m_data.m_node_sprite;

        string str = "";
        if (m_runtime_bonus > 0)
        {
            str += "+";
        }

        str += m_runtime_bonus.ToString();
        //GetComponentInChildren<TextMeshProUGUI>().text = m_data.m_index.ToString() + "(" + str + ")";   // debug
        GetComponentInChildren<TextMeshProUGUI>().text = str;

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

    public StoryData GetNodeStoryDataList()
    {
        return m_data.m_story_data;
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

    public void ControlAnim(bool enabled)
    {
        GameObject anim_object = transform.Find("Anim")?.gameObject;

        if(anim_object != null)
        {
            anim_object.GetComponent<Animator>().enabled = enabled;
        }
    }

    public void SetNodeValue(int value)
    {
        m_runtime_bonus = value;
    }
    public int GetNodeValue()
    {
        return m_runtime_bonus;
    }

    public void UpdateCost()
    {
        string str = "";
        if (m_runtime_bonus > 0)
        {
            str += "+";
        }

        str += m_runtime_bonus.ToString();
        GetComponentInChildren<TextMeshProUGUI>().text = str;
    }
}
