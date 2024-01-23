using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class PlayAnim : MonoBehaviour
{
    public string Folder;//图片的文件名
    RawImage RawImageObject;//使用RawImage加载
    public float  time;//图片时间间隔
    // Start is called before the first frame update
    void Start()
    {
        texs = new List<Texture>();
        RawImageObject = GetComponent<RawImage>();
        StartCoroutine(LoadTexture2D(Application.dataPath + "/Resources/Image/" + Folder));
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
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {

            //if (files[i].Name.EndsWith(".jpg"))//取脚本文件
            {
                string respath = files[i].Name;
                request = UnityWebRequestTexture.GetTexture(Application.dataPath + "/Resources/Image/" + Folder+'/'+respath);
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
        }
    }
  
}
