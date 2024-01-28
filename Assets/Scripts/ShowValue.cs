using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValuePanel : MonoBehaviour
{
    private List<Sprite> m_value_numbers;
    private GameObject m_image_layout;
    [SerializeField] private GameObject m_value_image_prefab;

    void Start()
    {
        GameManager.GetInstance().m_value_change_event = Repaint;

        m_value_numbers = new List<Sprite>();

        for (int i = 0; i < 10; i++)
        {
            m_value_numbers.Add(Resources.Load<Sprite>("ValueNumber/" + i.ToString()));
        }

        m_image_layout = GetComponent<HorizontalLayoutGroup>().gameObject;

        Repaint();
    }

    void Update()
    {
        /*
         * test
        if(Random.value > 0.9)
        {
            GameManager.GetInstance().AddToValue(1);
        }
        */
    }

    public void Repaint()
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            Destroy(image.gameObject);
        }

        int value = GameManager.GetInstance().GetValue();

        if(value < 0)
        {
            return;
        }

        do
        {
            GameObject value_image = Instantiate(m_value_image_prefab, m_image_layout.transform);
            Image value_image_component = value_image.GetComponent<Image>();
            value_image_component.sprite = m_value_numbers[Mathf.Abs(value % 10)];
            value /= 10;
        } while (value != 0);
    }
}
