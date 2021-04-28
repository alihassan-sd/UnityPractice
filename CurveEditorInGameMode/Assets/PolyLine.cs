using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyLine : MonoBehaviour
{
    List<GameObject> anchors = new List<GameObject>();
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    public void AddAnchor(GameObject anchor)
    {
        anchor.transform.parent = this.transform;
        this.anchors.Add(anchor);
    }

    void Update()
    {
        if (anchors.Count >= 2)
        {
            lr.positionCount = anchors.Count;

            List<Vector3> points = new List<Vector3>();
            foreach (GameObject anchor in anchors)
            {
                points.Add(anchor.transform.position);
            }

            lr.SetPositions(points.ToArray());
        }
    }
}
