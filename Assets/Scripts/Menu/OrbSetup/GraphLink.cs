using UnityEngine;
using System.Collections;

public class GraphLink : MonoBehaviour
{
    #region Static Variable
    public static float lineWidth = 0.01f;
    #endregion

    #region Variable
    Transform thisTransform;
    public Transform linkTransform;

    LineRenderer lineRenderer;
    #endregion

    // Use this for initialization
    void Start()
    {
        thisTransform = transform;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetWidth(lineWidth, lineWidth);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(thisTransform.position.x, thisTransform.position.y, 1f));
        lineRenderer.SetPosition(1, new Vector3(linkTransform.position.x, linkTransform.position.y, 1f));
    }
}
