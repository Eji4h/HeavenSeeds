using UnityEngine;
using System.Collections;

public class GraphLink : MonoBehaviour
{
    public static float lineWidth = 0.01f;

    Transform thisTransform;
    public Transform linkTransform;

    LineRenderer lineRenderer;

    // Use this for initialization
    void Start()
    {
        thisTransform = transform;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended"));
        lineRenderer.SetColors(Color.black, Color.black);
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetWidth(lineWidth, lineWidth);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(thisTransform.position.x, thisTransform.position.y, 0.00001f));
        lineRenderer.SetPosition(1, new Vector3(linkTransform.position.x, linkTransform.position.y, 0.00001f));
    }
}
