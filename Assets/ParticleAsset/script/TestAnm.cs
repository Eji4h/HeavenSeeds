using UnityEngine;
using System.Collections;

public class TestAnm : MonoBehaviour {
    public AnimationClip[] bb;
	void Start () {
	
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            animation.Play(bb[0].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
             animation.Play(bb[1].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
             animation.Play(bb[2].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
             animation.Play(bb[3].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
             animation.Play(bb[4].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
             animation.Play(bb[5].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            animation.Play(bb[6].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            animation.Play(bb[7].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            animation.Play(bb[8].name);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            animation.Play(bb[9].name);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            animation.Play(bb[10].name);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            animation.Play(bb[11].name);

        }
     
	}
}
