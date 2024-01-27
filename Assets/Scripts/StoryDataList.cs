using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryDataList", menuName = "GameData/StoryDataList")]
public class StoryDataList : ScriptableObject
{
    public List<StoryData> m_story_list;
}
