using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool m_is_mouse_over_panel;

    private Action m_on_click;

    public void SetMouseNotOverPanel()
    {
        m_is_mouse_over_panel = false;
    }

    public void SetClickAction(Action action)
    {
        m_on_click = action;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_is_mouse_over_panel = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_is_mouse_over_panel = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_is_mouse_over_panel && GameManager.GetInstance().m_mouse_interact_enable)
        {
            m_on_click?.Invoke();
        }
    }
}
