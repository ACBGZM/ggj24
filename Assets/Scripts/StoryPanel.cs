using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

public class StoryPanel : MouseClickInteraction
{
    private Action m_callback;
    public StoryText stext;
    public RawImage BackGround;
    public GameObject root;
    GameObject ui_cam;
    ScreenBlurEffect blurtool;


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
    public IEnumerator LoadText()//生成文本框
    {

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1f);
            AddText("fefefafefaf", new Vector2(100*i,200* i));
           
        }
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
class StoryTextData
{

}