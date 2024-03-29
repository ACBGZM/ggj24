﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryPanel : MouseClickInteraction
{
    private Action m_callback;
    public StoryText stext;
    public RawImage BackGround;
    public GameObject root;
    GameObject ui_cam;
    ScreenBlurEffect blurtool;

    private StoryData m_story_data;
    [SerializeField] private Image m_show_image;

    public void Awake()
    {
        SetClickAction(OnClickStoryImage);
        Canvas cave = GameObject.Find("Canvas").GetComponent<Canvas>();
        ui_cam = GameObject.Find("Main Camera");
        if (ui_cam != null)
        {
            cave.worldCamera = ui_cam.GetComponent<Camera>();
            BlurData blur_data = new BlurData();
            blur_data.blur_spread = 1;
            blur_data.blur_iteration = 4;
            blur_data.blur_size = 1;
            blur_data.blur_down_sample = 4;
          
            blurtool = ui_cam.GetComponent<ScreenBlurEffect>();
            blurtool.EnableBlurRender(blur_data, (e) => {
                this.gameObject.SetActive(true);
                BackGround.texture = e; 
            });
            //this.gameObject.SetActive(false);//注释掉模糊会带底板，不注释没法开启协程，之后加生成文本框逻辑时候再改
        }
       
    }
   
    public void Initialize(Action callback)
    {
        m_callback = callback;

        StartCoroutine(LoadText());

    }

    public void SetStoryDataList(StoryData story_data_list)
    {
        m_story_data = story_data_list;
        if(story_data_list.m_story_image == null)
        {
            m_show_image.gameObject.SetActive(false);
        }
        else
        {
            m_show_image.sprite = story_data_list.m_story_image;
        }
    }

    public void SetFullScreenImage()
    {
        m_show_image.rectTransform.sizeDelta = new Vector2(1920, 1080);
        // m_story_data.m_story_list.Clear();
    }

    public IEnumerator LoadText()//生成文本框
    {
        GameManager.GetInstance().DisableMouseInteraction();

        if(m_story_data != null)
        {
            foreach (StoryTextData story_data in m_story_data.m_story_list)
            {
                yield return new WaitForSeconds(story_data.m_wait_time / 1.5f);
                AddText(story_data.m_text_content, new Vector3(story_data.m_x, story_data.m_y, 0.0f));

            }
        }

        GameManager.GetInstance().EnableMouseInteraction();
    }
    void AddText(string text,Vector2 offset)
    {
        GameObject story_panel_object = Instantiate(stext.gameObject, transform);
        story_panel_object .transform.parent = root.transform;
        story_panel_object.transform.localPosition= offset;
        StoryText story_panel = story_panel_object.GetComponent<StoryText>();
        story_panel.SetText(text);
    }
    public void OnClickStoryImage()
    {
        Destroy(gameObject);
        m_callback?.Invoke();
    }
}
