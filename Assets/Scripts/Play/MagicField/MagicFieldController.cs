﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

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
    enum MagicFieldState
    {
        WaitingCommand,
        WaitingRotation
    }

    Camera uiCamera;

    public List<MagicPoint> listMagicPoints = new List<MagicPoint>(13),
        listMagicPointsUpstair = new List<MagicPoint>(9);

    int isSelectedCount = 0;

    int[] swordPointNumSet = new int[5] { 2, 5, 6, 7, 8 },
        bowPointNumSet = new int[5] { 1, 2, 3, 6, 8 },
        wandPointNumSet = new int[5] { 0, 4, 5, 7, 8 },
        shieldPointNumSet = new int[5] { 0, 1, 2, 3, 4 },
        scrollPointNumSet = new int[5] { 1, 3, 5, 7, 8 };

    MagicCircle magicCircleOut, magicCircleIn;
    MagicPoint magicPointCenter;

    UISprite magicFieldBG;

    float timePerMove = 1f;
    int magicCircleOutIndexChangePerMove = 3,
        magicCircleInIndexChangePerMove = -1;
    bool normalDirectionalRotation = true;

    MagicFieldState mgFieldState;
    CharacterActionState chaActionState;

    bool randomChaActionState = false;
    int randomChaActionStateCount = 0;

    public GameObject swordFxAnimation,
        bowFxAnimation,
        wandFxAnimation,
        shieldFxAnimation,
        scrollFxAnimation;

    MagicFieldState MgFieldState
    {
        get { return mgFieldState; }
        set 
        {
            StopAllCoroutines();
            mgFieldState = value;
            StartCoroutine(mgFieldState.ToString());
        }
    }

    public CharacterActionState ChaActionState
    {
        get { return chaActionState; }
        set
        {
            GameObject selectedFxAnimation;
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
            listMagicPointsUpstair.ForEach(magicPoint =>
                {
                    if (magicPoint.IsSelected)
                        magicPoint.UseMagicPoint();
                });
            if (selectedFxAnimation != null && !selectedCharacterController.IsFall)
            {
                selectedFxAnimation.SetActive(false);
                selectedFxAnimation.SetActive(true);
            }
            magicCircleOut.RotateCircle(normalDirectionalRotation ?
                magicCircleOutIndexChangePerMove : -magicCircleOutIndexChangePerMove, timePerMove);
            magicCircleIn.RotateCircle(normalDirectionalRotation ?
                magicCircleInIndexChangePerMove : -magicCircleInIndexChangePerMove, timePerMove);
            MgFieldState = MagicFieldState.WaitingRotation;
            selectedCharacterController.Action();
        }
    }

    public int RandomChaActionStateCount
    {
        get { return randomChaActionStateCount / NumberSecurity.RandomNumSecurity; }
        set
        {
            randomChaActionStateCount = value * NumberSecurity.RandomNumSecurity;
            randomChaActionState = RandomChaActionStateCount > 0;
        }
    }

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

        magicFieldBG = transform.Find("MagicFieldBG").GetComponent<UISprite>();
        MgFieldState = MagicFieldState.WaitingCommand;
    }

    void SetMagicPointsUpstair()
    {
        SetMagicPointsCollider(false);
        listMagicPointsUpstair.Clear();
        for (int i = 0; i < 5; i++)
            listMagicPointsUpstair.Add(magicCircleOut[i]);
        for (int i = 0; i < 3; i++)
            listMagicPointsUpstair.Add(magicCircleIn[i]);
        listMagicPointsUpstair.Add(magicPointCenter);
        listMagicPointsUpstair.ForEach(magicPoint =>
            magicPoint.Collider2DEnabled = true);
    }

    #region Waiting Command Method
    IEnumerator WaitingCommand()
    {
        WaitingCommandUIControllerClear();
        ResetMagicPointsIsSelected();
        SetMagicPointsUpstair();
        magicFieldBG.spriteName = "lightMagicBG";

        for (; ; )
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(0))
                CheckRayHitMagicPoint(Input.mousePosition);
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
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
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
            if (SceneController.SwordCharacterController.CanAction)
                ChaActionState = CharacterActionState.SwordAction;
            else
                MgFieldState = MagicFieldState.WaitingCommand;
        }
        else if (CheckCompletePointNumSet(bowPointNumSet))
        {
            if (SceneController.BowCharacterController.CanAction)
                ChaActionState = CharacterActionState.BowAction;
            else
                MgFieldState = MagicFieldState.WaitingCommand;
        }
        else if (CheckCompletePointNumSet(wandPointNumSet))
        {
            if (SceneController.WandCharacterController.CanAction)
                ChaActionState = CharacterActionState.WandAction;
            else
                MgFieldState = MagicFieldState.WaitingCommand;
        }
        else if (CheckCompletePointNumSet(shieldPointNumSet))
        {
            if (SceneController.ShieldCharacterController.CanAction)
                ChaActionState = CharacterActionState.ShieldAction;
            else
                MgFieldState = MagicFieldState.WaitingCommand;
        }
        else if (CheckCompletePointNumSet(scrollPointNumSet))
        {
            if (SceneController.ScrollCharacterController.CanAction)
                ChaActionState = CharacterActionState.ScrollAction;
            else
                MgFieldState = MagicFieldState.WaitingCommand;
        }
        else if (isSelectedCount > 5)
        {
            MgFieldState = MagicFieldState.WaitingCommand;
        }
    }

    bool CheckCompletePointNumSet(int[] pointNumSetCheck)
    {
        if (listMagicPointsUpstair[pointNumSetCheck[0]].IsSelected &
            listMagicPointsUpstair[pointNumSetCheck[1]].IsSelected &
            listMagicPointsUpstair[pointNumSetCheck[2]].IsSelected &
            listMagicPointsUpstair[pointNumSetCheck[3]].IsSelected &
            listMagicPointsUpstair[pointNumSetCheck[4]].IsSelected)
        {
            for (int i = 0; i < listMagicPointsUpstair.Count; i++)
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
                    listMagicPointsUpstair[i].IsSelected = false;
            }
            return true;
        }
        return false;
    }
    #endregion

    IEnumerator WaitingRotation()
    {
        SetMagicPointsCollider(false);
        UIController.SpinButton.Enabled = false;
        magicFieldBG.spriteName = "darkMagicBG";
        while (magicCircleOut.NowRotate && magicCircleIn.NowRotate)
            yield return null;
        MgFieldState = MagicFieldState.WaitingCommand;
    }

    public void ResetMagicPointsIsSelected()
    {
        isSelectedCount = 0;
        listMagicPoints.ForEach(magicPoint =>
            magicPoint.IsSelected = false);
    }

    void WaitingCommandUIControllerClear()
    {
        UIController.SpinButton.Enabled = true;
        UIController.SpinButton.ResetDefaultColor();
        UIController.FireElementBarController.ResetCount();
        UIController.WaterElementBarController.ResetCount();
        UIController.EarthElementBarController.ResetCount();
        UIController.WoodElementBarController.ResetCount();
    }

    void SetMagicPointsCollider(bool enabled)
    {
        listMagicPoints.ForEach(magicPoint =>
                magicPoint.Collider2DEnabled = enabled);
    }

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
        MgFieldState = MagicFieldState.WaitingRotation;
    }
    #endregion

    #region Orb Debuff
    public void OrbBurn(int numberOrbBurn)
    {
        int countOrbToBurn = 0;
        while (countOrbToBurn < numberOrbBurn)
        {
            MagicPoint magicPointSelected = listMagicPoints[Random.Range(0, listMagicPoints.Count)];
            if (magicPointSelected.Element != ElementType.None)
            {
                magicPointSelected.Element = ElementType.None;
                countOrbToBurn++;
            }
        }
    }

    public void OrbSkull(int numberOrbSkull)
    {
        int countOrbToSkull = 0;
        while (countOrbToSkull < numberOrbSkull)
        {
            MagicPoint magicPointSelected = listMagicPoints[Random.Range(0, listMagicPoints.Count)];
            if (!magicPointSelected.IsSkull)
            {
                magicPointSelected.IsSkull = true;
                countOrbToSkull++;
            }
        }
    }
    #endregion
}
