using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
public class Boarder : MonoBehaviour {

    public enum WeaponClass { Sword, Bow, Wand, Scroll, Shield, };
   public  WeaponClass weapon;
   public GameObject btn,colli,lokinbtn;
   private bool hasCard;

  void Update()
  {

      EventDelegate.Add(lokinbtn.GetComponent<UIButton>().onClick, lockinBtnOnclick);
  }
   void lockinBtnOnclick()
   {
       
           if (lockinbtn.islockin)
           {
               
               switch (weapon)
               {
                   case WeaponClass.Sword:
                       if (hasCard)
                           PlayerPrefs.SetString("SwordCharacter", colli.name);
                            print(colli.name + "Sword");
                       break;
                   case WeaponClass.Bow:
                       if (hasCard)
                           PlayerPrefs.SetString("BowCharacter", colli.name);
                            print(colli.name + "Bow");
                       break;
                   case WeaponClass.Wand:
                       if (hasCard)
                           PlayerPrefs.SetString("WandCharacter", colli.name);
                           print(colli.name + "Wand");
                       break;
                   case WeaponClass.Shield:
                       if (hasCard)
                           PlayerPrefs.SetString("ShieldCharacter", colli.name);
                            print(colli.name + "Shield");
                       break;
                   case WeaponClass.Scroll:
                       if (hasCard)
                           PlayerPrefs.SetString("ScrollCharacter", colli.name);
                            print(colli.name + "Scroll");
                       break;
               }

           }
       
   }
  void OnEnable()
   {
       
           switch (weapon)
           {

               case WeaponClass.Sword:
                   if (PlayerPrefs.HasKey("SwordCharacter") && PlayerPrefs.GetString("SwordCharacter") != "")
                   {
                       GameObject.Find(PlayerPrefs.GetString("SwordCharacter")).transform.parent = transform;
                       GameObject.Find(PlayerPrefs.GetString("SwordCharacter")).transform.localPosition = Vector3.zero;
                   }
                   break;
               case WeaponClass.Bow:
                   if (PlayerPrefs.HasKey("BowCharacter") && PlayerPrefs.GetString("BowCharacter") != "")
                   {
                       GameObject.Find(PlayerPrefs.GetString("BowCharacter")).transform.parent = transform;
                       GameObject.Find(PlayerPrefs.GetString("BowCharacter")).transform.localPosition = Vector3.zero;
                   }
                   break;
               case WeaponClass.Wand:
                   if (PlayerPrefs.HasKey("WandCharacter") && PlayerPrefs.GetString("WandCharacter") != "")
                   {
                       GameObject.Find(PlayerPrefs.GetString("WandCharacter")).transform.parent = transform;
                       GameObject.Find(PlayerPrefs.GetString("WandCharacter")).transform.localPosition = Vector3.zero;
                   }
                   break;
               case WeaponClass.Shield:
                   if (PlayerPrefs.HasKey("ShieldCharacter") && PlayerPrefs.GetString("ShieldCharacter") != "")
                   {
                       GameObject.Find(PlayerPrefs.GetString("ShieldCharacter")).transform.parent = transform;
                       GameObject.Find(PlayerPrefs.GetString("ShieldCharacter")).transform.localPosition = Vector3.zero;
                   }
                   break;
               case WeaponClass.Scroll:
                   if (PlayerPrefs.HasKey("ScrollCharacter") && PlayerPrefs.GetString("ScrollCharacter") != "")
                   {
                       GameObject.Find(PlayerPrefs.GetString("ScrollCharacter")).transform.parent = transform;
                       GameObject.Find(PlayerPrefs.GetString("ScrollCharacter")).transform.localPosition = Vector3.zero;
                   }
                   break;
           }
      
     
   }
   void OnTriggerStay(Collider coll)
    {  
            if (coll.gameObject.tag == "Card")
            {
                if (Input.GetMouseButtonUp(0))
                {
                    coll.transform.parent = transform;
                    coll.transform.localPosition = Vector3.zero;
                    hasCard = true;
                    colli = coll.gameObject;
                }
            } 
    }

}
