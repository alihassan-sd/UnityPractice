using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    List<Vector2> points;
    [SerializeField, HideInInspector]
    public bool isClosed = false;
    public GameObject gameObject;
    public float pointDistance = 1f;
    public float half = 0.5f;
    public int pointsInSegment = 4;
    public Path(Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre + new Vector2(-pointDistance,0),
            centre + new Vector2(-pointDistance*half, pointDistance*half),
            centre + new Vector2(pointDistance*half, -pointDistance*half),
            centre + new Vector2(pointDistance,0)
        };
    }

    public void AddSegment(Vector2 newAnchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        //points.Add((newAnchorPos + points[points.Count - 1])/2);
        points.Add(newAnchorPos + new Vector2(-pointDistance * half, -pointDistance * half));
        points.Add(newAnchorPos);

    }


    public Vector2[] GetPointsInSegment(int i) // First segment is at index '0' so will need to multiply by pointsInSegment-1
    {

        //Vector2 singlePoint = new Vector2(-1, 0);
        Vector2[] pointsToReturn = new Vector2[pointsInSegment];// { singlePoint, singlePoint, singlePoint, singlePoint};
        for (int j = 0; j < pointsInSegment; j++)
        {
            if (j == 3)
            {
                pointsToReturn[j] = points[LoopIndex(i * (pointsInSegment - 1) + j)];
            }
            else
            {
                pointsToReturn[j] = points[i * (pointsInSegment - 1) + j];
            }
        }

        return pointsToReturn;

        //return new Vector2[] { points[i * 3 + 0], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };
    }

    public int NumSegments
    {
        get
        {
            return (points.Count) / 3;
        }
    }
    public int NumPoints
    {
        get
        {
            return (points.Count);
        }
    }
    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public void MovePoint(int i, Vector2 newPos)
    {
        if (i % 3 == 0 && i != points.Count - 1) //Moving Anchor Point, except last
        {
            points[i - 1] = points[i - 1] + newPos - points[i];
            points[i + 1] = points[i + 1] + newPos - points[i];
            points[i] = newPos;
        }
        else if ((i % 3 == 0 || isClosed) && i == points.Count - 1) //Moving Last Anchor Point
        {
            if (!isClosed)
            {
                points[i - 1] = points[i - 1] + newPos - points[i];
                points[i] = newPos;
            }
            else
            {
                points[1] = points[1] - newPos + points[i];
                points[i] = newPos;
            }
        }
        else if (i % 3 != 0 && i != points.Count - 2 && i != 1) //Moving Control point
        //else if (i % 3 != 0 && i != 1 || isClosed) //Moving Control point
        {
            if ((i + 1) % 3 == 0) //Moving Control Point previous to anchor point
            {
                //points[i + 1] = points[i + 1] + newPos - points[i];
                points[i + 2] = points[i + 2] - (newPos - points[i]);
                points[i] = newPos;
            }
            else //Moving Control Point next to anchor point
            {
                //points[i - 1] = points[i - 1] + newPos - points[i];
                points[i - 2] = points[i - 2] - (newPos - points[i]);
                points[i] = newPos;
            }
        }
        
        else if (i == points.Count - 2 || i == 1) //Moving Last Control Point
        {
            if (!isClosed)
            {
                points[i] = newPos;
            }
            else
            {
                points[i-2] = points[i-2] - (newPos - points[i]);
                points[i] = newPos;
            }
        }

    }

    public void ToggleClosed()
    {
        isClosed = !isClosed;
        if (isClosed)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add(points[0] * 2 - points[1]);
        }
        else
        {
            points.RemoveRange(points.Count - 2, 2);
        }
    }
    /*
    int LoopIndex (int i)
    {
        return (i + points.Count) % points.Count;
    }*/
}
