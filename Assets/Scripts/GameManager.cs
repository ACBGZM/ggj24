using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameManager>();

            if (instance == null)
            {
                GameObject gameObject = new GameObject("GameManager");
                instance = gameObject.AddComponent<GameManager>();
            }

        }
        return instance;
    }

    private void Awake()
    {
        m_adjacency = new Adjacency();
        m_is_first_click = true;

        m_value = 0;
        AddToValue(0);

        m_mouse_interact_enable = true;
    }

    private uint m_current_node_index;

    private uint m_previous_node_index;

    [HideInInspector] public bool m_is_first_click;

    private int m_value;

    public Adjacency m_adjacency { get; set; }

    public Action m_value_change_event { get; set; }

    public bool m_mouse_interact_enable { get; private set; }

    public int GetValue() => m_value;

    public void AddToValue(int offset)
    {
        m_value += offset;

        if(m_value < 0)
        {
            // game over
        }

        m_value_change_event?.Invoke();
    }

    public bool SetActiveNodeIndex(uint index)
    {
        if(m_is_first_click || m_adjacency.GetWeight(m_current_node_index, index, out int weight))
        {
            if (!m_is_first_click)
            {
                m_previous_node_index = m_current_node_index;
            }
            m_current_node_index = index;

            return true;
        }

        return false;
    }

    public uint GetActiveNodeIndex() => m_current_node_index;
    public uint GetPreviousNodeIndex() => m_previous_node_index;

    public void MoveToActiveNode()
    {
        if(m_adjacency.GetWeight(m_previous_node_index, m_current_node_index, out int weight))
        {
            AddToValue(weight);
            // Debug.Log(m_previous_node_index.ToString() + " -> " + m_current_node_index.ToString() + "   " + weight.ToString());
        }

        m_adjacency.SetWeightToZero(m_previous_node_index, m_current_node_index);
    }

    public void EnableMouseInteraction()
    {
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        m_mouse_interact_enable = true;
    }

    public void DisableMouseInteraction()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        m_mouse_interact_enable = false;
    }

    public void CheckEndGame(Action callback)
    {
        if(m_value < 0)
        {
            callback?.Invoke();
        }
    }
}
