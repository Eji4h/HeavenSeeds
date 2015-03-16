using UnityEngine;
using System.Collections;

public class Callpopup : MonoBehaviour {
    public GameObject a;
    public GameObject  Pic ,Thumb,Lv,Swrd,Bw,Wnd,Shd,Scl;
    
    void Start()
    {
       
        EventDelegate.Add(GetComponent<UIButton>().onClick, CallOnClick);
    }

    void CallOnClick()
    {

        //Name.GetComponent<UILabel>().text = transform.name;
        
  
      
        Pic.GetComponent<UISprite>().spriteName = "Card_info_"+transform.name;
        Thumb.GetComponent<UISprite>().spriteName = "Card_tribe_"+transform.name;
        Lv.GetComponent<UILabel>().text = GetComponent<CharacterStatus>().Lv.ToString();
        Swrd.GetComponent<UILabel>().text = GetComponent<CharacterStatus>().SwordValue.ToString();
        Bw.GetComponent<UILabel>().text = GetComponent<CharacterStatus>().BowValue.ToString();
        Wnd.GetComponent<UILabel>().text = GetComponent<CharacterStatus>().WandValue.ToString();
        Shd.GetComponent<UILabel>().text = GetComponent<CharacterStatus>().ShiedlValue.ToString();
        Scl.GetComponent<UILabel>().text = GetComponent<CharacterStatus>().ScrollValue.ToString();
        a.SetActive(true);
    }
}
