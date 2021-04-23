
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    //public Camera camera;
    // Update is called once per frame
    //public GameObject[] camera;
    [SerializeField] private string selectedTag = "Metallic";
    [SerializeField]
    private Material previousMaterial;
    [SerializeField]
    private Material selectedmaterial;
    private GameObject[] gameObject1;
    LineRenderer lineRenderer;
    Vector2 myVector;
    Path path;
    Vector3 screenSpace;
    Vector3 offset;
    int anchorIndex = 3;
    float width = 1.0f;
    float anchorRadius = .05f;
    public int lengthOfLineRenderer;
    void Awake()
    {
        myVector = new Vector2(0, 0);
        path = new Path(new Vector2(0, 0));
        PathCreator pathCreator = new PathCreator();
        pathCreator.CreatePath();
        /*
        GameObject[] gameObject = GameObject.FindGameObjectsWithTag("Metallic");
        foreach (GameObject gameOb in gameObject)
        {
            gameOb.AddComponent<"LineRenderer">();
            lineRenderer = gameOb.GetComponent<LineRenderer>();
            selectionRenderer.material = previousMaterial;
        }
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();*/
        //pathCreator.CreatePath();
        //path = pathCreator.path;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        

        //Drawing 4 points in game mode
        for (int i = 0; i < path.NumPoints; i++)
        {
            //Vector2[] points = path.GetPointsInSegment(i);

            /*screenSpace = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 ins = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            offset = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(ins);*/

            //Debug.Log("Point = " + points[i]);
            Debug.Log("Num of Points = " + path.NumPoints);
            DrawPathPoints(path[i]);
            Debug.Log("Path value = " + path[i]);

        }
        DrawCubicCurve(path.points[0], path.points[1], path.points[2], path.points[3]);


    }
    private void Update()
    {
        
        //camera = GameObject.FindGameObjectsWithTag("MainCamera");
        //ray= new { };
        //var ray = new Ray();
        //Camera.main.ScreenPointToRay(Input.mousePosition)
        //gameObject = GameObject.FindGameObjectsWithTag("Metallic");
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            //if (selection.CompareTag(selectedTag))
            //{
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = selectedmaterial;
            }
            Debug.Log(selection);
            //}
            //selectionRenderer.material = previousMaterial;

        }
        else
        {
            GameObject[] gameObject = GameObject.FindGameObjectsWithTag("Metallic");
            foreach (GameObject gameOb in gameObject)
            {
                var selectionRenderer = gameOb.GetComponent<Renderer>();
                selectionRenderer.material = previousMaterial;
            }
            gameObject1 = GameObject.FindGameObjectsWithTag("anchor");
            if (gameObject1.Length != 0)
            {
                foreach (GameObject gameOb in gameObject1)
                {
                    var selectionRenderer = gameOb.GetComponent<Renderer>();
                    selectionRenderer.material = previousMaterial;
                }
                /*
                var selectionRenderer1 = gameObject1[0].GetComponent<Renderer>();
                selectionRenderer1.material = previousMaterial;*/
            }
        }

        //lineRenderer = GetComponent<LineRenderer>();
        //lengthOfLineRenderer = path.NumPoints;
        //lineRenderer.positionCount = lengthOfLineRenderer;
        /*
        for (int i = 0; i < path.NumPoints; i++)
        {
            //lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f));
            lineRenderer.SetPosition(i, path.points[i]);
        }*/
        
        
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("Inside Mouse Down from Update");
            //Debug.Log(Input.mousePosition);
            //transform.Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            screenSpace = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 ins = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            offset = Camera.main.ScreenToWorldPoint(ins) - Camera.main.transform.position;
            Debug.Log(offset);

            if (Physics.Raycast(ray, out hit))
            {
                var prevPoint = new Vector2(0, 0);
                int j = 0;
                GameObject selection = hit.collider.gameObject;
                for (int i = 0; i < path.NumPoints; i++)
                {
                    if (selection.transform.position.x == path.points[i].x && selection.transform.position.y == path.points[i].y)
                    {
                        j = i;
                    }
                }
                selection.transform.position = offset;

                prevPoint = path.points[j];
                path.points[j] = new Vector2(selection.transform.position.x, selection.transform.position.y);
                if(j%3 == 0 && j!=0)
                {
                    path.points[j - 1] = path.points[j - 1] - (prevPoint - path.points[j]); //(path.points[j] - path.points[j-1]); //path.points[j] + 
                    GameObject[] controlPoint = GameObject.FindGameObjectsWithTag("anchor");
                    controlPoint[j-1].transform.position = path.points[j-1];
                    DrawCubicCurve(path.points[j - 3], path.points[j - 2], path.points[j - 1], path.points[j]);
                }
                else if (j == 0)
                {
                    path.points[j + 1] = path.points[j + 1] - (prevPoint - path.points[j]); //(path.points[j] - path.points[j-1]); //path.points[j] + 
                    GameObject[] controlPoint = GameObject.FindGameObjectsWithTag("anchor");
                    controlPoint[j + 1].transform.position = path.points[j + 1];
                    DrawCubicCurve(path.points[j], path.points[j + 1], path.points[j + 2], path.points[j + 3]);
                }
                else if ((j+1)%3==0)
                {
                    DrawCubicCurve(path.points[j - 2], path.points[j - 1], path.points[j], path.points[j + 1]);
                }
                else if ((j + 2) % 3 == 0)
                {
                    DrawCubicCurve(path.points[j - 1], path.points[j], path.points[j + 1], path.points[j + 2]);
                }


            }

        }
        else if (Input.GetMouseButtonDown(1) && !Input.GetMouseButton(0))
        {
            
            //path.AddSegment(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            screenSpace = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 ins = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            offset =  Camera.main.ScreenToWorldPoint(ins) - Camera.main.transform.position;
            Debug.Log(offset);
            path.AddSegment(new Vector2(offset.x, offset.y));
            //Debug.Log(path.NumSegments);
            DrawPathPoints(new Vector2(offset.x, offset.y));
            //Vector3[3] positions = new Vector3[3](offset, offset + 1, offset + 2);
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            var t = Time.time;
            lengthOfLineRenderer = path.NumPoints;
            lineRenderer.positionCount = lengthOfLineRenderer;
            for (int i = 0; i < path.NumPoints; i++)
            {
                //lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f));
                lineRenderer.SetPosition(i, path.points[i]);
            }
            
        }
        
    }
    public void DrawPathPoints(Vector2 myVect)
    {
        var anchorPoint1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        anchorPoint1.transform.eulerAngles = new Vector3(anchorPoint1.transform.eulerAngles.x + 90, anchorPoint1.transform.eulerAngles.y,
            anchorPoint1.transform.eulerAngles.z);
        //SphereCollider sphereCollider = sphere.GetComponent<SphereCollider>();
        //sphereCollider.radius = anchorRadius;
        anchorPoint1.tag = "anchor";
        anchorPoint1.transform.position = myVect;
        //anchorPoint1.transform.position = -offset;
        /*var anchorPoint2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        anchorPoint2.transform.eulerAngles = new Vector3(anchorPoint2.transform.eulerAngles.x + 90, anchorPoint2.transform.eulerAngles.y,
            anchorPoint2.transform.eulerAngles.z);
        //SphereCollider anchorPointCollider2 = anchorpoint2.GetComponent<SphereCollider>();
        //anchorPoint2.radius = anchorRadius;
        anchorPoint2.tag = "anchor";
        anchorPoint2.transform.position = new Vector3(anchorPoint1.transform.position.x - width, anchorPoint1.transform.position.y + width, -offset.z);*/

        //anchorPoint2.transform.position = new Vector3(-anchorPoint1.transform.position.y, anchorPoint1.transform.position.x, -offset.z);

        //sphere1.transform.position = new Vector3(-sphere.transform.position.y, sphere.transform.position.x, -offset.z);
        //sphere1.transform.position = Vector3.Cross(sphere.transform.position, new Vector3(0,0,1)).normalized;
        Debug.Log("anchorIndex = " + anchorIndex);
        /*if(anchorIndex > 0)
        {
            DrawLine(anchorIndex, anchorIndex + 1);
        }*/
        anchorIndex += 3;
    }

    public void MoveAnchorPoint(Vector2 newPos)
    {
        
    }

    void DrawCubicCurve(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {

        lineRenderer.positionCount = 100;
        float s = 0f;
        Vector3 X = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            X = (1 - s) * (1 - s) * (1 - s) * point0 + 3 * (1 - s) * (1 - s) * s * point1 + 3 * (1 - s) * s * s * point2 + s * s * s * point3;

            lineRenderer.SetPosition(i, X);
            s += (1 / (float)lineRenderer.positionCount);
        }
    }

}

