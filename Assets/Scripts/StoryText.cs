using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using static System.Net.Mime.MediaTypeNames;

public class StoryText : MonoBehaviour
{
    public Image bg;
    public TextMeshProUGUI textMesh;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int tid;
    string Str;
    public void SetText(string text)
    {
        Str = text;

        textMesh.text = Str;
        bg.rectTransform.sizeDelta = textMesh.GetPreferredValues() + new Vector2(80f, 40f);
        //bg.rectTransform.sizeDelta = new Vector2(Str.Length * 30 + 200, bg.rectTransform.sizeDelta.y);
        textMesh.text = "";
        StartCoroutine(LoadText());

    }
    public IEnumerator LoadText()
    { 
        for(int i = 0; i <= Str.Length; i++)
        {
            textMesh.text = Str.Substring(0,i);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
