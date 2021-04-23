using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;
    public bool isClosed;

    public void CreatePath()
    {
        //path = new Path(transform.position);
        path = new Path(new Vector2(0,0));
    }
    

   
}
    