using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour
{
    [SerializeField] private Transform midPoint;

    [SerializeField] private LineRenderer lineRendererLeft;
    [SerializeField] private LineRenderer lineRendererRight;

    private void Awake()
    {
        Vector3 leftPoint  = new Vector3(lineRendererLeft.transform.position.x,
                                         lineRendererLeft.transform.position.y,
                                        -1f);

        Vector3 rightPoint = new Vector3(lineRendererRight.transform.position.x,
                                         lineRendererRight.transform.position.y,
                                        -1f);


        lineRendererLeft.SetPosition(0, leftPoint);
        lineRendererLeft.SetPosition(1, new Vector3(midPoint.position.x, midPoint.position.y, -1f));

        lineRendererRight.SetPosition(0, rightPoint);
        lineRendererRight.SetPosition(1, new Vector3(midPoint.position.x, midPoint.position.y, -1f));
    }

    private void Update()
    {
        lineRendererLeft.SetPosition (1, new Vector3(midPoint.position.x, midPoint.position.y, -1f));
        lineRendererRight.SetPosition(1, new Vector3(midPoint.position.x, midPoint.position.y, -1f));
    }
}
