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

            DontDestroyOnLoad(instance.gameObject);
        }
        return instance;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private uint m_current_node_index;

    private int m_value;

    public Action m_value_change_event { get; set; }

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

    public void SetActiveNodeIndex(uint index) => m_current_node_index = index;
    public uint GetActiveNodeIndex() => m_current_node_index;
}
