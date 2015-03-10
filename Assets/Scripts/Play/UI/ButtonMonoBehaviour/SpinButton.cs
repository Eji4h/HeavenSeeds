using UnityEngine;
using System.Collections;
using PayUnity;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SpinButton : UIButtonMonoBehaviour
{
    #region Variable
    int spinAmount;
    float timeMove = 1.5f;

    UILabel spinAmountLabel;
    UIWidget spinAmountLabelWidget;
    TweenRotation tweenRotation;
    #endregion

    #region Properties
    public int SpinAmount
    {
        get { return spinAmount / NumberSecurity.RandomNumSecurity; }
        set 
        { 
            spinAmount = value * NumberSecurity.RandomNumSecurity;
            PlayerPrefs.SetInt("SpinAmount", SpinAmount);
            if (SpinAmount > 0)
            {
                spinAmountLabelWidget.width = 25;
                spinAmountLabel.text = SpinAmount.ToString();
            }
            else
            {
                spinAmountLabelWidget.width = 50;
                spinAmountLabel.text = "BUY";
            }
        }
    }

    #endregion

    #region Method
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        spinAmountLabel = GetComponentInChildren<UILabel>();
        spinAmountLabelWidget = spinAmountLabel.GetComponent<UIWidget>();
        tweenRotation = GetComponent<TweenRotation>();
        tweenRotation.duration = timeMove;
        //SpinAmount = PlayerPrefs.GetInt("SpinAmount", 3);
        SpinAmount = 3;
    }

    protected override void OnClickBehaviour()
    {
        if (SpinAmount > 0)
        {
            tweenRotation.ResetToBeginning();
            tweenRotation.PlayForward();
            StartCoroutine(RandomElement(timeMove));
            SceneController.MagicFieldController.RotateMagicCircle(
                RandomNumberSpin(9, 11), RandomNumberSpin(9, 11), timeMove);
        }
        else
            ;//Display Buy spin point popup
    }

    int RandomNumberSpin(int minNum, int maxNum)
    {
        return Random.Range(0, 2) == 0 ?
            Random.Range(-maxNum, -minNum) : Random.Range(minNum, maxNum);
    }

    IEnumerator RandomElement(float timeMove)
    {
        float totalTimeElapsed = 0f,
            timeInLoop = timeMove * 0.75f,
            spaceTimePerRandom = 0.05f;

        var listMagicPoints = SceneController.MagicFieldController.listMagicPoints;

        while (totalTimeElapsed < timeInLoop)
        {
            foreach (var magicPoint in listMagicPoints)
            {
                magicPoint.RandomElement();
                totalTimeElapsed += spaceTimePerRandom;
                yield return new WaitForSeconds(spaceTimePerRandom);
                if (totalTimeElapsed >= timeInLoop)
                    break;
            }
        }

        SpinAmount--;

        if(OftenMethod.RandomPercent(30f))
        {
            ElementType elementToChange = (ElementType)Random.Range(1, 5);
            listMagicPoints.ForEach(magicPoint =>
                magicPoint.SetElement(elementToChange));
        }
    }
    #endregion
}
