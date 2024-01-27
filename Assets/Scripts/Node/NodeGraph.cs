using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeGraph", menuName = "GameData/NodeGraph")]
public class NodeGraph : ScriptableObject
{
    public List<NodeData> m_node_data_list;

    public List<LinkData> m_link_data_list;
}
