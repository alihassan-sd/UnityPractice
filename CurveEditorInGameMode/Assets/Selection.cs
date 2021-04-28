using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public GameObject selectedGO = null;

    // Negative z
    Vector3 drawingPlaneVector = -Vector3.forward;
    Plane drawingPlane;

    // Start is called before the first frame update
    void Start()
    {
        drawingPlane = new Plane(drawingPlaneVector, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        //Create a ray from the Mouse click position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        RaycastHit hitInfo;
        float distance;

        // Raycast
        bool hit = drawingPlane.Raycast(ray, out distance);
        //Get the point that is clicked
        Vector3 drawingPlaneCoord = ray.GetPoint(distance);

        // When Right click
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                selectedGO = hitInfo.transform.gameObject;
                Debug.Log("Hitting");
            }
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedGO = null;
        }

            if (Input.GetMouseButton(0))
        {
            if (selectedGO != null)
            {
                selectedGO.transform.position = drawingPlaneCoord;
            }
        }
    }
}



