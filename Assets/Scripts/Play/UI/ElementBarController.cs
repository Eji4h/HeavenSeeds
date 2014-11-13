using UnityEngine;
using System.Collections;

public class ElementBarController : MonoBehaviour
{
    #region Variable
    UIProgressBar progressBar;
    public ElementType element;
    GameObject elementParticleGameObject;
    float oneStepValue;

    delegate IEnumerator ElementAttackBehaviour();
    ElementAttackBehaviour elementAttackBehaviour;

    #endregion

    #region Properties
    float Value
    {
        get { return progressBar.value; }
        set 
        {
            progressBar.value = value;
            if (progressBar.value >= 0.999999f)
            {
                StartCoroutine(elementAttackBehaviour());
                progressBar.value = 0f;
            }
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        progressBar = GetComponent<UIProgressBar>();

        switch(element)
        {
            case ElementType.Fire:
                elementParticleGameObject = Instantiate(Resources.Load("Prefabs/Particle/Player/Ultimate/newFire")) as GameObject;
                //elementAttackBehaviour = FireAttackBehaviour;
                break;
            case ElementType.Water:
                elementParticleGameObject = Instantiate(Resources.Load("Prefabs/Particle/Player/Ultimate/Water")) as GameObject;
                //elementAttackBehaviour = WaterAttackBehaviour;
                break;
            case ElementType.Earth:
                elementParticleGameObject = Instantiate(Resources.Load("Prefabs/Particle/Player/Ultimate/Ground")) as GameObject;
                //elementAttackBehaviour = EarthAttackBehaviour;
                break;
            case ElementType.Wood:
                elementParticleGameObject = Instantiate(Resources.Load("Prefabs/Particle/Player/Ultimate/LeafStrom")) as GameObject;
                //elementAttackBehaviour = WoodAttackBehaviour;
                break;
        }
        elementParticleGameObject.SetActive(false);
        oneStepValue = 1f / progressBar.numberOfSteps;
    }

    public void IncreaseOneStepValue()
    {
        Value += oneStepValue * 2f;
    }

    //IEnumerator FireAttackBehaviour()
    //{
    //    SceneController.ListMonsters.ForEach(monster =>
    //        {
    //            elementParticleGameObject.transform.position = monster.transform.position;
    //            elementParticleGameObject.SetActive(false);
    //            elementParticleGameObject.SetActive(true);
    //        });
    //    yield return new WaitForSeconds(1f);
    //    SceneController.ListMonsters.ForEach(monster =>
    //        {
    //            monster.ReceiveDamage(500);
    //        });
    //}

    //IEnumerator WaterAttackBehaviour()
    //{
    //    SceneController.ListMonsters.ForEach(monster =>
    //    {
    //        elementParticleGameObject.transform.position = monster.transform.position;
    //        elementParticleGameObject.SetActive(false);
    //        elementParticleGameObject.SetActive(true);
    //    });
    //    yield return new WaitForSeconds(3.5f);
    //    for (int i = 0; i < 4; i++)
    //    {
    //        SceneController.ListMonsters.ForEach(monster =>
    //        {
    //            monster.ReceiveDamage(100);
    //        });
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    //IEnumerator EarthAttackBehaviour()
    //{
    //    SceneController.ListMonsters.ForEach(monster =>
    //    {
    //        elementParticleGameObject.transform.position = monster.transform.position;
    //        elementParticleGameObject.SetActive(false);
    //        elementParticleGameObject.SetActive(true);
    //    });
    //    yield return new WaitForSeconds(1f);
    //    for (int i = 0; i < 14; i++)
    //    {
    //        SceneController.ListMonsters.ForEach(monster =>
    //        {
    //            monster.ReceiveDamage(25);
    //        });
    //        yield return new WaitForSeconds(0.25f);
    //    }
    //}

    //IEnumerator WoodAttackBehaviour()
    //{
    //    SceneController.ListMonsters.ForEach(monster =>
    //    {
    //        elementParticleGameObject.transform.position = monster.transform.position;
    //        elementParticleGameObject.SetActive(false);
    //        elementParticleGameObject.SetActive(true);
    //    });
    //    yield return new WaitForSeconds(1.25f);
    //    SceneController.ListMonsters.ForEach(monster =>
    //    {
    //        monster.ReceiveDamage(300);
    //    });
    //}

}
