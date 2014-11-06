using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterActionState
{
    SwordAction, 
    BowAction,
    WandAction,
    ShieldAction,
    ScrollAction
}

public class MagicFieldController : MonoAndCoroutinePauseBehaviour
{
    #region Variable
    Camera uiCamera;

    public List<MagicPoint> listMagicPoints = new List<MagicPoint>(13),
        listMagicPointsUpstairs = new List<MagicPoint>(9);

    int isSelectedCount = 0;

    int[] swordPointNumSet = new int[5] { 2, 5, 6, 7, 8 },
        bowPointNumSet = new int[5] { 1, 2, 3, 6, 8 },
        wandPointNumSet = new int[5] { 0, 4, 5, 7, 8 },
        shieldPointNumSet = new int[5] { 0, 1, 2, 3, 4 },
        scrollPointNumSet = new int[5] { 1, 3, 5, 7, 8 };

    MagicCircle magicCircleOut, magicCircleIn;
    MagicPoint magicPointCenter;

    float defaultTimePerMove = 1f,
        timePerMove = 1f;
    int magicCircleOutIndexChangePerMove = 3,
        magicCircleInIndexChangePerMove = -1;
    bool normalDirectionalRotation = true;

    GameObject selectedFxAnimation,
        swordFxAnimation,
        bowFxAnimation,
        wandFxAnimation,
        shieldFxAnimation,
        scrollFxAnimation;

    MagicFieldState mgFieldState;
    CharacterActionState chaActionState;
    bool randomChaActionState = false;
    int randomChaActionStateCount = 0;
    #endregion

    #region EnumType
    public enum MagicFieldState
    {
        WaitingCommand,
        WaitingRotation,
        WaitingMonsterTurn
    }
    #endregion

    #region Properties
    public CharacterActionState ChaActionState
    {
        get { return chaActionState; }
        set
        {
            CharacterController selectedCharacterController;
            chaActionState = randomChaActionState ? (CharacterActionState)Random.Range(0, 5) : value;
            switch (chaActionState)
            {
                case CharacterActionState.SwordAction:
                    selectedFxAnimation = swordFxAnimation;
                    selectedCharacterController = SceneController.SwordCharacterController;
                    break;
                case CharacterActionState.BowAction:
                    selectedFxAnimation = bowFxAnimation;
                    selectedCharacterController = SceneController.BowCharacterController;
                    break;
                case CharacterActionState.WandAction:
                    selectedFxAnimation = wandFxAnimation;
                    selectedCharacterController = SceneController.WandCharacterController;
                    break;
                case CharacterActionState.ShieldAction:
                    selectedFxAnimation = shieldFxAnimation;
                    selectedCharacterController = SceneController.ShieldCharacterController;
                    break;
                case CharacterActionState.ScrollAction:
                    selectedFxAnimation = scrollFxAnimation;
                    selectedCharacterController = SceneController.ScrollCharacterController;
                    break;
                default:
                    selectedFxAnimation = swordFxAnimation;
                    selectedCharacterController = SceneController.SwordCharacterController;
                    break;
            }
            listMagicPointsUpstairs.ForEach(magicPoint =>
                {
                    if (magicPoint.IsSelected)
                        magicPoint.UseMagicPoint();
                });
            selectedCharacterController.Action();
            //selectedFxAnimation.SetActive(false);
            if (randomChaActionState && randomChaActionStateCount > 0)
                RandomChaActionStateCount--;
            mgFieldState = MagicFieldState.WaitingRotation;
            //selectedFxAnimation.SetActive(true);
            magicCircleOut.RotateCircle(normalDirectionalRotation ?
                magicCircleOutIndexChangePerMove : -magicCircleOutIndexChangePerMove, timePerMove);
            magicCircleIn.RotateCircle(normalDirectionalRotation ?
                magicCircleInIndexChangePerMove : -magicCircleInIndexChangePerMove, timePerMove);
        }
    }

    public int RandomChaActionStateCount
    {
        get { return randomChaActionStateCount; }
        set
        {
            randomChaActionStateCount = value;
            if (randomChaActionStateCount < 1)
                randomChaActionState = false;
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        uiCamera = SceneController.UICamera;
        for (int i = 1; i < 14; i++)
            listMagicPoints.Add(GameObject.Find("Point" + i).GetComponent<MagicPoint>());

        MagicPoint[] magicPointsOut = new MagicPoint[8],
            magicPointsIn = new MagicPoint[4];

        int indexMagicPoint = 0;

        for (int i = 0; i < 8; i++)
            magicPointsOut[i] = listMagicPoints[indexMagicPoint++];
        for (int i = 0; i < 4; i++)
            magicPointsIn[i] = listMagicPoints[indexMagicPoint++];
        magicPointCenter = listMagicPoints[indexMagicPoint];

        magicCircleOut = GameObject.Find("CircleOut").GetComponent<MagicCircle>();
        magicCircleIn = GameObject.Find("CircleIn").GetComponent<MagicCircle>();

        magicCircleOut.SetMagicPoint(magicPointsOut);
        magicCircleIn.SetMagicPoint(magicPointsIn);

        StartCoroutine(MagicFieldFSM());
    }

    void SetMagicPointsUpstairs()
    {
        SetMagicPointCollider(false);
        listMagicPointsUpstairs.Clear();
        for (int i = 0; i < 5; i++)
            listMagicPointsUpstairs.Add(magicCircleOut[i]);
        for (int i = 0; i < 3; i++)
            listMagicPointsUpstairs.Add(magicCircleIn[i]);
        listMagicPointsUpstairs.Add(magicPointCenter);
        listMagicPointsUpstairs.ForEach(magicPoint =>
            magicPoint.collider2D.enabled = true);
    }

    public void ChangeStateToWaitingCommand()
    {
        mgFieldState = MagicFieldState.WaitingCommand;
    }

    IEnumerator MagicFieldFSM()
    {
        for (; ; )
            yield return StartCoroutine(mgFieldState.ToString());
    }

    #region Waiting Command Method
    IEnumerator WaitingCommand()
    {
        ResetMagicPointIsSelected();
        SetMagicPointsUpstairs();
        CharacterTurnEffectDecrease();
        while (mgFieldState == MagicFieldState.WaitingCommand)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(0))
                CheckRayHitMagicPoint(Input.mousePosition);
            if (Input.GetKey(KeyCode.S))
                ChaActionState = CharacterActionState.SwordAction;
            else if (Input.GetKey(KeyCode.B))
                ChaActionState = CharacterActionState.BowAction;
            else if (Input.GetKey(KeyCode.W))
                ChaActionState = CharacterActionState.WandAction;
            else if (Input.GetKey(KeyCode.D))
                ChaActionState = CharacterActionState.ShieldAction;
            else if (Input.GetKey(KeyCode.F))
                ChaActionState = CharacterActionState.ScrollAction;
#else
            if (Input.touchCount > 0)
                foreach (Touch touch in Input.touches)
                    CheckRayHitMagicPoint(touch.position);
#endif
            yield return null;
            yield return _sync();
        }
    }

    void CheckRayHitMagicPoint(Vector3 posCheck)
    {
        Ray ray = uiCamera.ScreenPointToRay(posCheck);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        if (hit)
        {
            if (hit.transform.tag == "MagicPoint")
            {
                MagicPoint hitMagicPoint = hit.transform.GetComponent<MagicPoint>();
                if (hitMagicPoint.IsSelected == false)
                {
                    hitMagicPoint.IsSelected = true;
                    if (++isSelectedCount >= 5)
                        CheckSelectedMagicPoint();
                }
            }
        }
    }

    void CheckSelectedMagicPoint()
    {
        if (CheckCompletePointNumSet(swordPointNumSet))
        {
            print("Sword");
            ChaActionState = CharacterActionState.SwordAction;
        }
        else if (CheckCompletePointNumSet(bowPointNumSet))
        {
            print("Bow");
            ChaActionState = CharacterActionState.BowAction;
        }
        else if (CheckCompletePointNumSet(wandPointNumSet))
        {
            print("Wand");
            ChaActionState = CharacterActionState.WandAction;
        }
        else if (CheckCompletePointNumSet(shieldPointNumSet))
        {
            print("Shield");
            ChaActionState = CharacterActionState.ShieldAction;
        }
        else if (CheckCompletePointNumSet(scrollPointNumSet))
        {
            print("Scroll");
            ChaActionState = CharacterActionState.ScrollAction;
        }
        else if (isSelectedCount > 5)
        {
            ResetMagicPointIsSelected();
            mgFieldState = MagicFieldState.WaitingCommand;
            StartCoroutine(mgFieldState.ToString());
        }
    }

    bool CheckCompletePointNumSet(int[] pointNumSetCheck)
    {
        if (listMagicPointsUpstairs[pointNumSetCheck[0]].IsSelected &
            listMagicPointsUpstairs[pointNumSetCheck[1]].IsSelected &
            listMagicPointsUpstairs[pointNumSetCheck[2]].IsSelected &
            listMagicPointsUpstairs[pointNumSetCheck[3]].IsSelected &
            listMagicPointsUpstairs[pointNumSetCheck[4]].IsSelected)
        {
            for (int i = 0; i < listMagicPointsUpstairs.Count; i++)
            {
                bool hasEqual = false;
                foreach (int j in pointNumSetCheck)
                {
                    if (i == j)
                    {
                        hasEqual = true;
                        break;
                    }
                }
                if (!hasEqual)
                    listMagicPointsUpstairs[i].IsSelected = false;
            }
            return true;
        }
        return false;
    }
    #endregion

    #region Waiting Rotation Method
    IEnumerator WaitingRotation()
    {
        while (magicCircleOut.NowRotate && magicCircleIn.NowRotate)
            yield return null;
        mgFieldState = MagicFieldState.WaitingMonsterTurn;
    }
    #endregion

    #region Waiting Monster Turn Method

    IEnumerator WaitingMonsterTurn()
    {
        yield return null;
    }

    #endregion

    public void ResetMagicPointIsSelected()
    {
        isSelectedCount = 0;
        listMagicPoints.ForEach(magicPoint =>
            magicPoint.IsSelected = false);
    }

    void CharacterTurnEffectDecrease()
    {
        CharacterController.TurnEffectDecrease();
    }

    #region MagicPoint Collider Controller
    void SetMagicPointCollider(bool enabled)
    {
        listMagicPoints.ForEach(magicPoint =>
            magicPoint.collider2D.enabled = enabled);
    }

    public void DisableColliderMagicPoint(float timeDisable)
    {
        StartCoroutine(DelayMagicPointColliderToEnabled(timeDisable));
    }

    IEnumerator DelayMagicPointColliderToEnabled(float timeDisable)
    {
        SetMagicPointCollider(false);
        yield return new WaitForSeconds(timeDisable);
        SetMagicPointCollider(true);
    }
    #endregion
    #region MagicCircle TimePerMove Controller
    public void TimePerMoveSlowdown(float slowdownMultiply, float timeSlow)
    {
        if (slowdownMultiply < 1f)
        {
            timePerMove *= slowdownMultiply;
            StartCoroutine(DelayToDefaultTimePerMove(timeSlow));
        }
    }

    IEnumerator DelayToDefaultTimePerMove(float timeSlow)
    {
        yield return new WaitForSeconds(timeSlow);
        timePerMove = defaultTimePerMove;
    }
    #endregion
    #region Random Character Action Controller
    public void RandomChaActionState(float timeRandom)
    {
        randomChaActionState = true;
        StartCoroutine(DelayToDefaultTimePerMove(timeRandom));
    }

    IEnumerator DelayToNotRandomChaActionState(float timeRandom)
    {
        yield return new WaitForSeconds(timeRandom);
        randomChaActionState = false;
    }

    public void RandomChaActionState(int randomCount)
    {
        randomChaActionState = true;
        randomChaActionStateCount = randomCount;
    }
    #endregion
    #region RotateMagicCircle Controller
    public void RotateMagicCircle(int indexMagicCircleOutChange, int indexMagicCircleInChange)
    {
        RotateMagicCircle(indexMagicCircleOutChange, indexMagicCircleInChange, timePerMove);
    }

    public void RotateMagicCircle(int indexMagicCircleOutChange, int indexMagicCircleInChange, float timePerMove)
    {
        magicCircleOut.RotateCircle(normalDirectionalRotation ?
            indexMagicCircleOutChange : -indexMagicCircleOutChange, timePerMove);
        magicCircleIn.RotateCircle(normalDirectionalRotation ?
            indexMagicCircleInChange : -indexMagicCircleInChange, timePerMove);
        mgFieldState = MagicFieldState.WaitingRotation;
    }
    #endregion
}
