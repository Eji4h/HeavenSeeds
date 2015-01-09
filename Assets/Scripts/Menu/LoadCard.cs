using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
public class LoadCard : MonoBehaviour
{
    private string a;
    // Use this for initialization
    void Start()
    {
        print("Sword"+PlayerPrefs.GetString("SwordCharacter"));
        print("Bow"+PlayerPrefs.GetString("BowCharacter"));
        print("Wand"+PlayerPrefs.GetString("WandCharacter"));
        print("Shield"+PlayerPrefs.GetString("ShieldCharacter"));
        print("Scroll"+PlayerPrefs.GetString("ScrollCharacter"));
      
    }


}