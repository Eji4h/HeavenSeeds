using UnityEngine;
using System;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TestPlayerPrefs : MonoBehaviour
{
    public GUIText appDataPathGUIText;
    // Use this for initialization
    void Start()
    {
        appDataPathGUIText.text = Application.persistentDataPath + "\n" +
#if UNITY_WEBPLAYER
            "Web";
#else
            "Not Web";
#endif
        PlayerPrefs.EnableEncryption(true);
        for (int i = 1; i < 1000; i++)
        {
            PlayerPrefs.SetInt("Point" + i, i % 5);
            PlayerPrefs.SetString("Hello" + i, i.ToString());
        }
        //appDataPathGUIText.text = Application.persistentDataPath;
        //appDataPathGUIText.text = UnityEngine.PlayerPrefs.GetInt("deltaTime").ToString();
        
        
        //UnityEngine.PlayerPrefs.SetInt("deltaTime", (DateTime.Now - dateTimeStartBeforeFlush).Milliseconds);
        StartCoroutine(TestFlushTime());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TestFlushTime()
    {
        yield return new WaitForSeconds(5f);
        for(;;)
        {
            DateTime dateTimeStartBeforeFlush = DateTime.Now;

            PlayerPrefs.Flush();
            appDataPathGUIText.text = Application.persistentDataPath + "\n" +
                ((DateTime.Now - dateTimeStartBeforeFlush).TotalMilliseconds).ToString();
            yield return new WaitForSeconds(5f);
        }
    }
}
