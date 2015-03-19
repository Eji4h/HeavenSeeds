using UnityEngine;
using System.Collections;

public class ChooseMonsterController : MonoAndCoroutinePauseBehaviour
{
    Transform thisTransform;

    Monster chooseMonster;

    public Monster ChooseMonster
    {
        get { return chooseMonster; }
        set
        {
            chooseMonster = value;
            thisTransform.parent = chooseMonster.transform;
            thisTransform.localPosition = new Vector3(0f, 0.1f);
        }
    }

    void Awake()
    {
        thisTransform = transform;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CheckTouchBegin());
    }

    IEnumerator CheckTouchBegin()
    {
        for (; ; )
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(0))
                CheckRayHitMonsterInLine(Input.mousePosition);
#else
            if (Input.touchCount > 0)
                foreach (Touch touch in Input.touches)
                    if (touch.phase == TouchPhase.Began)
                        CheckRayHitMonsterInLine(touch.position);
#endif
            yield return null;
            yield return _sync();
        }
    }

    void CheckRayHitMonsterInLine(Vector3 posCheck)
    {
        Ray ray = SceneController.MainCamera.ScreenPointToRay(posCheck);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            Transform hitTransform = hitInfo.transform;
            if (hitTransform.tag == "Monster")
            {
                SceneController.ListCurrentLineMonster.ForEach(monster =>
                    {
                        if (monster.transform == hitTransform)
                            ChooseMonster = hitTransform.GetComponent<Monster>();
                    });
            }
        }
    }
}
