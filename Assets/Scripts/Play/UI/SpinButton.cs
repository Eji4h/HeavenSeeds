using UnityEngine;
using System.Collections;
using PayUnity;

public class SpinButton : MonoBehaviour
{
    UIButton uiButton;

    // Use this for initialization
    void Start()
    {
        uiButton = GetComponent<UIButton>();
        EventDelegate.Add(uiButton.onClick, SpinButtonOnClick);
    }

    void SpinButtonOnClick()
    {
        float timeMove = 1.5f;
        StartCoroutine(RandomElement(timeMove));
        SceneController.MagicFieldController.RotateMagicCircle(
            RandomNumberSpin(9, 11), RandomNumberSpin(9, 11), timeMove);
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

        if(OftenMethod.RandomPercent(30f))
        {
            ElementType elementToChange = (ElementType)Random.Range(1, 5);
            listMagicPoints.ForEach(magicPoint =>
                magicPoint.SetElement(elementToChange));
        }
    }
}
