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
    public string Folder;//ͼƬ���ļ���
    RawImage RawImageObject;//ʹ��RawImage����
    public float  time;//ͼƬʱ����
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
        //�ļ�����һ����������ļ�
        //SearchOption.TopDirectoryOnly�����ѡ��ֻȡ��һ������ļ�
        //SearchOption.AllDirectories�����ѡ���ȡ�������е����ļ�
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {

            //if (files[i].Name.EndsWith(".jpg"))//ȡ�ű��ļ�
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
                    Debug.LogError(Application.dataPath + "/Resources/Image" + Folder + '/' + respath + "ͼƬ����" + request.result);
                }
            }
               
        }
        for (int i = 0; i < texs.Count; i++)//��ʱ�����л�ͼƬ
        {
            RawImageObject.texture = texs[i];
            yield return new WaitForSeconds(time);
        }
    }
  
}
