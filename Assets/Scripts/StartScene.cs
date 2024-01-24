using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public MouseClickInteraction startbutton;
    public PlayAnim BackAnim;
    // Start is called before the first frame update
    void Start()
    {
        BackAnim.isLoop = true;
        BackAnim.Folder = "StartLoop";
        BackAnim.RefreshAnim();
        startbutton.SetClickAction(() =>
        {
            BackAnim.isLoop = false;
            BackAnim.Folder = "Start";
            BackAnim.OnAnimEnd = () =>
            {
                SceneManager.LoadScene("PrimaryScene");
            };
            BackAnim.RefreshAnim();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

