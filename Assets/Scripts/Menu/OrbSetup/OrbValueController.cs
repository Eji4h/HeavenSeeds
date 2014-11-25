using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class OrbValueController : MonoBehaviour
{
    #region Static Variable
    static float startSliderValue = 0.5f;
    static UISlider fireSilder,
        waterSilder,
        earthSilder,
        woodSilder;

    static float fireValue,
        waterValue,
        earthValue,
        woodValue;
    #endregion

    #region Static Properties
    public static float FireValue
    {
        get { return OrbValueController.fireValue; }
    }

    public static float WaterValue
    {
        get { return OrbValueController.waterValue; }
    }

    public static float EarthValue
    {
        get { return OrbValueController.earthValue; }
    }

    public static float WoodValue
    {
        get { return OrbValueController.woodValue; }
    }
    #endregion

    #region Static Method
    public static void SetToPlayerPref()
    {
        PlayerPrefs.SetFloat("FireOrbValue", fireValue);
        PlayerPrefs.SetFloat("WaterOrbValue", waterValue);
        PlayerPrefs.SetFloat("EarthOrbValue", earthValue);
        PlayerPrefs.SetFloat("WoodOrbValue", woodValue);
    }

    public static void UpdateSlider()
    {
        fireSilder.value = fireValue;
        waterSilder.value = waterValue;
        earthSilder.value = earthValue;
        woodSilder.value = woodValue;
    }

    public static void ResetOrbValue()
    {
        fireValue = startSliderValue;
        waterValue = startSliderValue;
        earthValue = startSliderValue;
        woodValue = startSliderValue;
        UpdateSlider();
    }

    static void UpdateElementValue(ref float elementValue1,
        ref float elementValue2, ref float elementValue3,
        ref float delta)
    {
        float deltaShare = 0f;

        if (delta > 0f)
        {
            int minCount = 0;
            bool element1IsMin = false,
                element2IsMin = false,
                element3IsMin = false;

            for (int i = 0; i < 2; i++)
            {
                deltaShare = delta / (3f - minCount);

                if (!element1IsMin &&
                    elementValue1 < deltaShare)
                {
                    delta -= elementValue1;
                    elementValue1 = 0f;
                    element1IsMin = true;
                    minCount++;
                }

                if (!element2IsMin &&
                    elementValue2 < deltaShare)
                {
                    delta -= elementValue2;
                    elementValue2 = 0f;
                    element2IsMin = true;
                    minCount++;
                }

                if (!element3IsMin &&
                    elementValue3 < deltaShare)
                {
                    delta -= elementValue3;
                    elementValue3 = 0f;
                    element3IsMin = true;
                    minCount++;
                }
            }

            deltaShare = delta / (3f - minCount);

            if (!element1IsMin)
            {
                delta -= deltaShare;
                elementValue1 -= deltaShare;
            }

            if (!element2IsMin)
            {
                delta -= deltaShare;
                elementValue2 -= deltaShare;
            }

            if (!element3IsMin)
            {
                delta -= deltaShare;
                elementValue3 -= deltaShare;
            }
        }
        else
        {
            delta *= -1f;
            int maxCount = 0;
            bool element1IsMax = false,
                element2IsMax = false,
                element3IsMax = false;

            for (int i = 0; i < 2; i++)
            {
                deltaShare = delta / (3f - maxCount);

                if (!element1IsMax &&
                    elementValue1 + deltaShare > 1f)
                {
                    delta -= 1f - elementValue1;
                    elementValue1 = 1f;
                    element1IsMax = true;
                    maxCount++;
                }

                if (!element2IsMax &&
                    elementValue2 + deltaShare > 1f)
                {
                    delta -= 1f - elementValue2;
                    elementValue2 = 1f;
                    element2IsMax = true;
                    maxCount++;
                }

                if (!element3IsMax &&
                    elementValue3 + deltaShare > 1f)
                {
                    delta -= 1f - elementValue3;
                    elementValue3 = 1f;
                    element3IsMax = true;
                    maxCount++;
                }
            }

            deltaShare = delta / (3f - maxCount);

            if (!element1IsMax)
            {
                delta -= deltaShare;
                elementValue1 += deltaShare;
            }

            if (!element2IsMax)
            {
                delta -= deltaShare;
                elementValue2 += deltaShare;
            }

            if (!element3IsMax)
            {
                delta -= deltaShare;
                elementValue3 += deltaShare;
            }
        }
    }
    #endregion

    #region Variable
    public ElementType elementType;
    #endregion

    // Use this for initialization
    void Start()
    {
        UISlider uiSlider = GetComponent<UISlider>();
        uiSlider.value = startSliderValue;
        EventDelegate.Add(uiSlider.onChange, OnValueChange);

        switch (elementType)
        {
            case ElementType.Fire:
                fireSilder = uiSlider;
                fireValue = startSliderValue;
                break;
            case ElementType.Water:
                waterSilder = uiSlider;
                waterValue = startSliderValue;
                break;
            case ElementType.Earth:
                earthSilder = uiSlider;
                earthValue = startSliderValue;
                break;
            case ElementType.Wood:
                woodSilder = uiSlider;
                woodValue = startSliderValue;
                break;
        }
    }

    void OnValueChange()
    {
        float delta = 0f;

        switch (elementType)
        {
            case ElementType.Fire:
                delta = fireSilder.value - fireValue;
                fireValue = fireSilder.value;
                UpdateElementValue(ref waterValue, ref earthValue, ref woodValue, ref delta);
                break;
            case ElementType.Water:
                delta = waterSilder.value - waterValue;
                waterValue = waterSilder.value;
                UpdateElementValue(ref fireValue, ref earthValue, ref woodValue, ref delta);
                break;
            case ElementType.Earth:
                delta = earthSilder.value - earthValue;
                earthValue = earthSilder.value;
                UpdateElementValue(ref fireValue, ref waterValue, ref woodValue, ref delta);
                break;
            case ElementType.Wood:
                delta = woodSilder.value - woodValue;
                woodValue = woodSilder.value;
                UpdateElementValue(ref fireValue, ref waterValue, ref earthValue, ref delta);
                break;
        }

        UpdateSlider();
        SetToPlayerPref();
    }
}
