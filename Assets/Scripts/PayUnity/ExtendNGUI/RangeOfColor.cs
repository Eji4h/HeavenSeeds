using UnityEngine;
using System.Collections;

public class RangeOfColor : MonoBehaviour
{
    public Color colorThisRange = Color.white;

    [Range (0f, 1f)]
    public float lowestRange = 0f, highestRange = 1f;
}
