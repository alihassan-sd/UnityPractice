using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator pathCreator;
    Path path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Creat New"))
        {
            pathCreator.CreatePath();
            path = pathCreator.path;
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Toggle Closed"))
        {
            path.ToggleClosed();
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.shift)
        {
            Undo.RecordObject(pathCreator, "Add Segment");
            path.AddSegment(mousePos);
        }
    }

    void Draw()
    {
        //Drawing lines
        for (int i=0; i < path.NumSegments; i++ )
        {
            Vector2[] points = path.GetPointsInSegment(i);
            Handles.color = Color.blue;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
            
        }

        //Drawing Path points
        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, .1f, Vector2.zero, Handles.CylinderHandleCap);
            if (path[i] != newPos)
            {
                Undo.RecordObject(pathCreator, "Move Point");
                path.MovePoint(i, newPos);

            }
        }
    }

    void OnEnable ()
    {
        pathCreator = (PathCreator)target; //targetting the parhcreator on unity as a component
        if(pathCreator.path == null)
        {
            pathCreator.CreatePath();
        }
        path = pathCreator.path;
    }
}
