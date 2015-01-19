using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using PayUnity;

public class MagicPoint : MonoBehaviour
{
    static float fireEndPercentRange,
        waterEndPercentRange,
        earthEndPercentRange,
        woodEndPercentRange;

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
    }

    Transform thisTransform,
        thisParentTransform;
    UISprite uiSprite,
        uiSelectedLight;
    bool isSelected;

    ElementBarController elementBarController;

    ElementType element;
    bool isSkull;
    TweenPosition tweenPos;
    TweenScale tweenScale;
    Vector3 localPos;

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

    public bool IsSkull
    {
        get { return isSkull; }
        set 
        {
            isSkull = value; 
            if(uiSprite != null)
            {
                if (isSkull)
                {
                    uiSprite.spriteName = "SkullOrb";
                    uiSprite.width = 35;
                    uiSprite.height = 35;
                }
                else
                    Element = element;
            }
        }
    }

    public bool Collider2DEnabled
    {
        get { return collider2D.enabled; }
        set
        {
            collider2D.enabled = value;
            uiSprite.color = value ? Color.white : Color.gray;
        }
    }

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
        isSkull = false;
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

    public void SetElement(ElementType element)
    {
        isSkull = false;
        Element = element;
    }

    public void UseMagicPoint()
    {
        if (!isSkull && element != ElementType.None)
        {
            switch (element)
            {
                case ElementType.Fire:
                    elementBarController = UIController.FireElementBarController;
                    break;
                case ElementType.Water:
                    elementBarController = UIController.WaterElementBarController;
                    break;
                case ElementType.Earth:
                    elementBarController = UIController.EarthElementBarController;
                    break;
                case ElementType.Wood:
                    elementBarController = UIController.WoodElementBarController;
                    break;
            }

            Transform targetParentTransform = elementBarController.transform.parent;

            thisTransform.parent = null;
            elementBarController.transform.parent = null;
            tweenPos.from = thisTransform.position;
            tweenPos.to = elementBarController.transform.position;

            elementBarController.transform.parent = targetParentTransform;

            tweenPos.ResetToBeginning();
            tweenPos.PlayForward();
        }
        else
        {
            EventDelegate.Add(tweenScale.onFinished, OnFinishTweenScale);
            if (isSkull)
                CharacterController.ReceiveDamageByPercentOfSumMaxHp(7f);
        }
        tweenScale.SetEndToCurrentValue();
        tweenScale.Toggle();
    }

    void OnFinishTweenPos()
    {
        IsSelected = false;
        elementBarController.Increase();
        thisTransform.parent = thisParentTransform;
        thisTransform.localPosition = localPos;
        RandomElement();
        tweenScale.to = Vector3.one;
        tweenScale.Toggle();
    }

    void OnFinishTweenScale()
    {
        if (isSkull || element == ElementType.None)
        {
            IsSelected = false;
            isSkull = false;
            RandomElement();
            tweenScale.to = Vector3.one;
            tweenScale.Toggle();
        }
        else
            tweenScale.onFinished.Clear();
    }
}
