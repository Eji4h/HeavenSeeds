using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using PayUnity;

public class MagicPoint : MonoBehaviour
{
    #region Static Variable
    static float fireEndPercentRange,
        waterEndPercentRange,
        earthEndPercentRange,
        woodEndPercentRange;

    static ElementBarController fireElementBarController,
        waterElementBarController,
        earthElementBarController,
        woodElementBarController;
    #endregion

    #region Static Method
    public static void SetInit()
    {
        float minimumValue = 10f,
            defaultSliderValue = 0.5f,
            multiplyValue = 30f;

        fireEndPercentRange = minimumValue + 
            PlayerPrefs.GetFloat("FireOrbValue", defaultSliderValue) * multiplyValue;

        waterEndPercentRange = fireEndPercentRange + minimumValue + 
            PlayerPrefs.GetFloat("WaterOrbValue", defaultSliderValue) * multiplyValue;

        earthEndPercentRange = waterEndPercentRange + minimumValue + 
            PlayerPrefs.GetFloat("EarthOrbValue", defaultSliderValue) * multiplyValue;

        woodEndPercentRange = woodEndPercentRange + minimumValue + 
            PlayerPrefs.GetFloat("WoodOrbValue", defaultSliderValue) * multiplyValue;

        fireElementBarController = GameObject.Find("FireElementBar").GetComponent<ElementBarController>();
        waterElementBarController = GameObject.Find("WaterElementBar").GetComponent<ElementBarController>();
        earthElementBarController = GameObject.Find("EarthElementBar").GetComponent<ElementBarController>();
        woodElementBarController = GameObject.Find("WoodElementBar").GetComponent<ElementBarController>();
    }
    #endregion

    #region Variable
    Transform thisTransform,
        thisParentTransform;
    UISprite uiSprite,
        uiSelectedLight;
    bool isSelected;

    ElementBarController elementBarController;

    ElementType element;
    TweenPosition tweenPos;
    TweenScale tweenScale;
    Vector3 localPos;
    #endregion

    #region Properties
    public bool IsSelected
    {
        get { return isSelected; }
        set 
        { 
            isSelected = value;
            if (uiSelectedLight != null)
                uiSelectedLight.alpha = isSelected ? 1f : 0f;
        }
    }

    public ElementType Element
    {
        get { return element; }
        set 
        {
            element = value;
            if (uiSprite != null)
            {
                uiSprite.spriteName = element.ToString() + "Orb";

                if (element == ElementType.None)
                {
                    uiSprite.width = 35;
                    uiSprite.height = 35;
                }
                else
                {
                    uiSprite.width = 45;
                    uiSprite.height = 45;
                }
            }
        }
    }
    #endregion

    void Awake()
    {
        thisTransform = transform;
        thisParentTransform = transform.parent;
        localPos = thisTransform.localPosition;
        uiSprite = GetComponent<UISprite>();
        uiSelectedLight = transform.Find("SelectedLight").GetComponent<UISprite>();
        IsSelected = false;

        tweenPos = GetComponent<TweenPosition>();
        tweenPos.from = localPos;
        EventDelegate.Add(tweenPos.onFinished, OnFinishTweenPos);
        tweenScale = GetComponent<TweenScale>();
    }

    void Start()
    {
        RandomElement();
    }

    public void RandomElement()
    {
        float randomNum = Random.Range(0f, 100f);

        if (OftenMethod.InRandomRange(randomNum, 0f, fireEndPercentRange))
            Element = ElementType.Fire;
        else if (OftenMethod.InRandomRange(randomNum, fireEndPercentRange, waterEndPercentRange))
            Element = ElementType.Water;
        else if (OftenMethod.InRandomRange(randomNum, waterEndPercentRange, earthEndPercentRange))
            Element = ElementType.Earth;
        else
            Element = ElementType.Wood;
    }

    public void UseMagicPoint()
    {
        switch (element)
        {
            case ElementType.Fire:
                elementBarController = fireElementBarController;
                break;
            case ElementType.Water:
                elementBarController = waterElementBarController;
                break;
            case ElementType.Earth:
                elementBarController = earthElementBarController;
                break;
            case ElementType.Wood:
                elementBarController = woodElementBarController;
                break;
        }

        Transform targetTransform = elementBarController.transform,
            targetParentTransform = elementBarController.transform.parent;

        thisTransform.parent = null;
        elementBarController.transform.parent = null;
        tweenPos.from = thisTransform.position;
        tweenPos.to = elementBarController.transform.position;

        elementBarController.transform.parent = targetParentTransform;

        tweenPos.ResetToBeginning();
        tweenPos.PlayForward();
        tweenScale.SetEndToCurrentValue();
        tweenScale.Toggle();
    }

    void OnFinishTweenPos()
    {
        IsSelected = false;
        elementBarController.IncreaseOneStepValue();
        thisTransform.parent = thisParentTransform;
        thisTransform.localPosition = localPos;
        RandomElement();
        tweenScale.to = Vector3.one;
        tweenScale.Toggle();
    }
}
