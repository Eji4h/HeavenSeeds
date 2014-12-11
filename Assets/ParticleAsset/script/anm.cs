using UnityEngine;
using System.Collections;

public class anm : MonoBehaviour
{
    public GameObject[] part;
    void Skill0()
    {
        part[0].SetActive(true);

    }
    void Skill1()
    {
        part[1].SetActive(true);
     
    }
    void Skill2()
    {
        part[2].SetActive(true);
    }
    void Skill3()
    {
        part[3].SetActive(true);
    }
    void Skill4()
    {
        part[4].SetActive(true);
    }
    void Ulti()
    {
        part[5].SetActive(true);
    }
}