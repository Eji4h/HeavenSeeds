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
     
	}
}
