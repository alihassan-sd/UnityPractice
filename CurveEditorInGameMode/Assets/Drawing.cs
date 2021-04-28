using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    Selection selection;
    int i = 0;
    // Negative z
    Vector3 drawingPlaneVector = new Vector3(0, 0, -1); //-Vector3.forward;
    Plane drawingPlane;

    // Start is called before the first frame update
    void Start()
    {
        drawingPlane = new Plane(drawingPlaneVector, new Vector3(0, 0, 0)); // Vector3.zero);
        //selection = new Selection();
        selection = GameObject.Find("SceneManager").GetComponent<Selection>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Create a ray from the Mouse click position
        //Ray ray = new Ray();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float distance = 0.0f;

        // Raycast
        bool hit = drawingPlane.Raycast(ray, out distance);
        //Get the point that is clicked
        Vector3 drawingPlaneCoord = ray.GetPoint(distance);

        // When Right click
        if (Input.GetMouseButtonDown(1))
        {
            GameObject newAnchor = Instantiate(
                GameObject.CreatePrimitive(PrimitiveType.Sphere),
                drawingPlaneCoord, Quaternion.identity);

            if (selection.selectedGO == null)
            {
                GameObject newGO = new GameObject("PolyLineGO");
                newGO.AddComponent<PolyLine>();
                newGO.GetComponent<PolyLine>().AddAnchor(newAnchor);

                // Debug
                selection.selectedGO = newGO;
                Debug.Log(selection.selectedGO);
            }
            else
            {
                selection.selectedGO.GetComponent<PolyLine>().AddAnchor(newAnchor);
            }
            i++;
        }
        
        if (i==2)
        {
            selection.selectedGO = null;
            i = 0;
        }
    }
}
