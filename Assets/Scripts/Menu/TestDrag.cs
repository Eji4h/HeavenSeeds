using UnityEngine;
using System.Collections;

public class TestDrag : MonoBehaviour {
    private Vector3 startpos;
    private bool isspring = false;
    public GameObject startB;
   // public GameObject Largeicon;
    void Awake()
    {
       
        startpos = startB.transform.position;
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isspring = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(isspring)
            transform.position = startpos;
        }
        
    }
    void OnTriggerStay(Collider coll)
    {
    
        if (coll.gameObject.tag == "CharacterBoarder")
        {
           
            isspring = false;
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                transform.position = startpos;
            }
        }
    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "CharacterBoarder")
        {
            isspring = true;
        }
    }
}
