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
    void Start()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick,Dragggg);
    }
    void Update()
    {
       
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
            if(Input.GetMouseButtonUp(0))
            {
               
                isspring = false;
            }
        }
        else if (coll.gameObject.tag == gameObject.tag)
        {
            if (Input.GetMouseButtonUp(0))
            {
                transform.position = startpos;
            }
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
    void Dragggg()
    {
        isspring = true;
    }
}
