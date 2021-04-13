using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator pathCreator;
    Path path;
    int currentSegment = -1;
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
            if (currentSegment != -1)
            {
                Undo.RecordObject(pathCreator, "Spliting Segment");
                path.SplitSegment(mousePos, currentSegment);
            }
            else if (!path.isClosed)
            {
                Undo.RecordObject(pathCreator, "Add Segment");
                path.AddSegment(mousePos);
            }
            
        }
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.control)
        {
            Undo.RecordObject(pathCreator, "Deleting Segment");
            int j = 0;
            
            for (j = 0; j < path.NumPoints; j++)
            {
                
                if (Vector2.Distance(mousePos, path[j]) <= 0.5f)
                {
                    //j = 5;
                    path.DeleteSegment(j);
                }
            }
            
        }
        //guiEvent.type == EventType.MouseMove && 
        if (guiEvent.type == EventType.MouseMove && guiEvent.button == 1 && guiEvent.control && guiEvent.shift)
        {
            int j = 0;
            int newSegment = -1;

            for (j = 0; j < path.NumSegments; j++)
            {
                Vector2[] points = path.GetPointsInSegment(j);
                float distance = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (distance <= 0.5f)
                {
                    //j = 5;
                    path.SplitSegment(mousePos, j);
                    newSegment = j;
                }
            }
            if(newSegment != currentSegment)
            {
                currentSegment = newSegment;
                HandleUtility.Repaint();
            }
            if (currentSegment != -1)
            {
                Undo.RecordObject(pathCreator, "Spliting Segment");
                path.SplitSegment(mousePos, currentSegment);
            }
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
            Color segmentColor = (i == currentSegment && Event.current.shift) ? Color.red : Color.green;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2);
            
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
