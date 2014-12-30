using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicCircle : MonoAndCoroutinePauseBehaviour
{
    #region Struct
    struct RotationCircleArguments
    {
        public int indexChange;
        public float timePerMove;
    }
    #endregion

    #region Variable
    Transform thisTransform;

    //Data Structor
    MagicPoint[] listMagicPoint;
    int index, capacity;

    //Rotation
    float rotateAnglePerMove;
    bool nowRotate = false;
    Queue<RotationCircleArguments> queueRotationArguments =
        new Queue<RotationCircleArguments>(8);

    public bool isCircleOut;
    #endregion

    #region Properties
    //Data Structor
    public MagicPoint[] ListMagicPoint
    {
        get { return listMagicPoint; }
    }

    public int Index
    {
        get { return index; }
        set
        {
            try
            {
                index = value;
                while(index < 0)
                    index = value + capacity;
                index %= capacity;
            }
            catch
            {

            }
        }
    }

    //Rotate
    public bool NowRotate
    {
        get { return nowRotate; }
    }
    #endregion

    public void SetMagicPoint(MagicPoint[] listMagicPoint)
    {
        this.listMagicPoint = listMagicPoint;
        capacity = listMagicPoint.Length;
    }

    public MagicPoint this[int index]
    {
        get
        {
            int selectedIndex = index - this.index;
            while (selectedIndex < 0)
                selectedIndex += listMagicPoint.Length;
            return listMagicPoint[selectedIndex % capacity];
        }
    }

    public void RotateCircle(int indexChange, float timePerMove)
    {
        RotationCircleArguments rotationCircleArgements;
        rotationCircleArgements.indexChange = indexChange;
        rotationCircleArgements.timePerMove = timePerMove;
        queueRotationArguments.Enqueue(rotationCircleArgements);
        if (!nowRotate)
            StartCoroutine(RotateCircleUpdate());
        index += indexChange;
    }

    IEnumerator RotateCircleUpdate()
    {
        nowRotate = true;
        while (queueRotationArguments.Count > 0)
        {
            RotationCircleArguments rotationCircleArguments = queueRotationArguments.Dequeue();
            int indexChange = rotationCircleArguments.indexChange;
            float timePerMove = rotationCircleArguments.timePerMove,
                sumAngleChange = 0f,
                angleChange = indexChange * rotateAnglePerMove,
                angleChangeAbs = Mathf.Abs(angleChange),
                speedRotate = angleChangeAbs / timePerMove,
                finalZRotation = thisTransform.rotation.eulerAngles.z - angleChange;
            Vector3 directionRotate = indexChange > 0 ? Vector3.back : Vector3.forward;

            while (sumAngleChange < angleChangeAbs)
            {
                float currentAngleChange = Time.deltaTime * speedRotate;
                sumAngleChange += currentAngleChange;
                thisTransform.Rotate(directionRotate, currentAngleChange);

                foreach (MagicPoint magicPoint in listMagicPoint)
                    magicPoint.transform.rotation = Quaternion.identity;

                yield return null;
            }

            thisTransform.rotation = Quaternion.Euler(0, 0, finalZRotation);
        }
        nowRotate = false;
    }

    // Use this for initialization
    void Start()
    {
        thisTransform = transform;
        rotateAnglePerMove = isCircleOut ? 45f : 90f;
    }
}
