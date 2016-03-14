using UnityEngine;
using System.Collections;

public class TestAnm : MonoBehaviour {
    public AnimationClip[] bb;
	void Start () {
	
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            GetComponent<Animation>().Play(bb[0].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
             GetComponent<Animation>().Play(bb[1].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
             GetComponent<Animation>().Play(bb[2].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
             GetComponent<Animation>().Play(bb[3].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
             GetComponent<Animation>().Play(bb[4].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
             GetComponent<Animation>().Play(bb[5].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            GetComponent<Animation>().Play(bb[6].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            GetComponent<Animation>().Play(bb[7].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            GetComponent<Animation>().Play(bb[8].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            GetComponent<Animation>().Play(bb[9].name);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            GetComponent<Animation>().Play(bb[10].name);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            GetComponent<Animation>().Play(bb[11].name);

        }
     
	}
}
