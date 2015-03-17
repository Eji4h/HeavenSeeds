using UnityEngine;
using System.Collections;

public class UIOffset : MonoBehaviour
{
    public Vector3 offset;
    public Transform targetTransform;
    Camera mainCamera, uiCamera;

    public void SetInit(Vector3 offset, Transform targetTransform, 
        Camera mainCamera, Camera uiCamera)
    {
        this.offset = offset;
        this.targetTransform = targetTransform;
        this.mainCamera = mainCamera;
        this.uiCamera = uiCamera;
        StartCoroutine(UpdateOffset());
    }

    IEnumerator UpdateOffset()
    {
        for (; ; )
        {
            Vector3 targetScreenPoint = mainCamera.WorldToScreenPoint(targetTransform.position),
                UIPos = uiCamera.ScreenToWorldPoint(targetScreenPoint);

            Vector3 SumPos = UIPos + offset;

            transform.position = new Vector2(SumPos.x, SumPos.y);
            yield return null;
        }
    }
}
