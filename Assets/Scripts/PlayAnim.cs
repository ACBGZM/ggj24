using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;
using System;
//using static System.Net.Mime.MediaTypeNames;

public class PlayAnim : MonoBehaviour
{
    public string Folder;//图片的文件名
    RawImage RawImageObject;//使用RawImage加载
    public float time;//图片时间间隔
    public bool isLoop;

    public Action OnAnimEnd;
    IEnumerator enumerator;
    // Start is called before the first frame update
    void Awake()
    {
        texs = new List<Texture>();
        RawImageObject = GetComponent<RawImage>();
        enumerator = LoadTexture2D(Application.dataPath + "/Resources/Image/" + Folder);
        //StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {

    }
    List<Texture> texs;

    public IEnumerator LoadTexture2D(string path)
    {
        UnityWebRequest request;
        DirectoryInfo direction = new DirectoryInfo(path);
        //文件夹下一层的所有子文件
        //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
        //SearchOption.AllDirectories：这个选项会取其下所有的子文件
        if (texs != null)
            texs.Clear();
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {

            if (files[i].Name.EndsWith(".JPG"))//取脚本文件
            {
                string respath = files[i].Name;
                request = UnityWebRequestTexture.GetTexture(Application.dataPath + "/Resources/Image/" + Folder + '/' + respath);
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var texture = DownloadHandlerTexture.GetContent(request);
                    texs.Add(texture);
                }
                else
                {
                    Debug.LogError(Application.dataPath + "/Resources/Image" + Folder + '/' + respath + "图片错误：" + request.result);
                }
            }

        }
        for (int i = 0; i < texs.Count; i++)//按时间间隔切换图片
        {
            RawImageObject.texture = texs[i];
            yield return new WaitForSeconds(time);
            if (isLoop && i == texs.Count - 1)
            {
                i = 0;
            }
        }
        if (OnAnimEnd != null) OnAnimEnd();
    }

    public void RefreshAnim()
    {


        if (enumerator != null)
        {
            StopCoroutine(enumerator);
            enumerator = null;
        }

        // StopCoroutine(enumerator);
        enumerator = LoadTexture2D(Application.dataPath + "/Resources/Image/" + Folder);
        StartCoroutine(enumerator);
    }

}
