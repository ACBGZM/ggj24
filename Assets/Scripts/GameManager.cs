using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = new GameManager();
    public static GameManager GetInstance() => instance;

    private uint m_current_node_index;

    public void SetActiveNodeIndex(uint index) => m_current_node_index = index;
    public uint GetActiveNodeIndex() => m_current_node_index;
}
