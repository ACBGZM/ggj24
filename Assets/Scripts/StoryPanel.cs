using System;

public class StoryPanel : MouseClickInteraction
{
    private Action m_callback;

    public void Awake()
    {
        SetClickAction(OnClickStoryImage);
    }

    public void Initialize(Action callback)
    {
        m_callback = callback;
    }

    public void OnClickStoryImage()
    {
        Destroy(gameObject);
        m_callback?.Invoke();
    }
}
