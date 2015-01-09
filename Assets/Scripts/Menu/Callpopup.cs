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
        
  
        switch (transform.name)
        {
            case ("Goma"):
                Pic.GetComponent<UISprite>().spriteName = "Card_info_mayom_01";
                Thumb.GetComponent<UISprite>().spriteName = "Card_tribe_mayom_01";
                Lv.GetComponent<UILabel>().text =  GetComponent<GomaStatus>().Lv.ToString();
                Swrd.GetComponent<UILabel>().text = GetComponent<GomaStatus>().SwordValue.ToString();
                Bw.GetComponent<UILabel>().text = GetComponent<GomaStatus>().BowValue.ToString();
                Wnd.GetComponent<UILabel>().text = GetComponent<GomaStatus>().WandValue.ToString();
                Shd.GetComponent<UILabel>().text = GetComponent<GomaStatus>().ShiedlValue.ToString();
                Scl.GetComponent<UILabel>().text = GetComponent<GomaStatus>().ScrollValue.ToString();
                break;
            case ("Hansa"):
                Pic.GetComponent<UISprite>().spriteName = "Card_info_rambut_01";
                Thumb.GetComponent<UISprite>().spriteName = "Card_tribe_rambut_01";
                break;
            case ("Yana"):
                Pic.GetComponent<UISprite>().spriteName = "Card_info_mangst_01";
                Thumb.GetComponent<UISprite>().spriteName = "Card_tribe_mangst_01";
                break;
            case ("Sansa"):
                Pic.GetComponent<UISprite>().spriteName = "Card_info_orang_01";
                Thumb.GetComponent<UISprite>().spriteName = "Card_tribe_orang_01";
                break;
            case ("Shun"):
                Pic.GetComponent<UISprite>().spriteName = "Card_info_durian_01";
                Thumb.GetComponent<UISprite>().spriteName = "Card_tribe_raseapp_01";
                break;
        }
        a.active = true;
    }
}
