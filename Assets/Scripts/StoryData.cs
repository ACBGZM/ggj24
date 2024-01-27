using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StoryDataList", menuName = "GameData/StoryDataList")]
public class StoryData : ScriptableObject
{
    public Sprite m_story_image;
    public List<StoryTextData> m_story_list;
}
