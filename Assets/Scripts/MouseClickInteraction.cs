using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool m_is_mouse_over_panel;

    public Action m_click_callback;

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
        if (Input.GetMouseButtonDown(0) && m_is_mouse_over_panel)
        {
            m_click_callback?.Invoke();
        }
    }
}
