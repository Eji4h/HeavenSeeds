using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
    public GameObject current;
    public GameObject next;
    private GameObject cam;
    void Start()
    {
        cam = GameObject.Find("Camera");
        EventDelegate.Add(GetComponent<UIButton>().onClick, BtnOnClick);
    }

    void BtnOnClick()
    {
        StartCoroutine(fin());
        next.active = true;
        cam.GetComponent<TweenTransform>().ResetToBeginning();
        cam.GetComponent<TweenTransform>().delay = 0f;
       cam.GetComponent<TweenTransform>().from  = current.transform;
       cam.GetComponent<TweenTransform>().to= next.transform;
       cam.GetComponent<TweenTransform>().Play( true);
     //  EventDelegate.Add(cam.GetComponent<TweenTransform>().onFinished, fin);
     //  cam.GetComponent<TweenTransform>().ResetToBeginning();
     
      
      
    }
   IEnumerator fin()
    {
        yield return new WaitForSeconds(.5f);
        current.active = false;
        StopAllCoroutines();
    }
}
