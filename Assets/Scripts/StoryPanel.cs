public class StoryPanel : MouseClickInteraction
{
    public void Awake()
    {
        m_click_callback = ClickToDestroy;
    }

    public void ClickToDestroy()
    {
        Destroy(gameObject);
    }
}
