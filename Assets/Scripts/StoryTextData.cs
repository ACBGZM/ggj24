using UnityEngine;

[System.Serializable]
public class StoryTextData
{
    [TextArea] public string m_text_content;
    public int m_x;
    public int m_y;
    public float m_wait_time;
}
