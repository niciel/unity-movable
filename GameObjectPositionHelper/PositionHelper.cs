using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PositionHelper 
{

    static PositionHelper()
    {
        SceneView.duringSceneGui += SceneView_duringSceneGui;
    }

    private static float range = 100;

    private static void SceneView_duringSceneGui(SceneView obj)
    {
        GameObject[] selected = Selection.gameObjects;
        if (selected == null)
        {
            return;
        }
        foreach (GameObject gm in selected)
        {
            Vector3 last;
            Ray r = new Ray(gm.transform.position, Vector3.down);
            float distance;
            if (Physics.Raycast(r , out RaycastHit hit , range))
            {
                last = hit.point;
                Handles.color = Color.green;
                distance = (gm.transform.position - last).magnitude;
                Handles.Label(last, distance + "");
            }
            else
            {
                last = gm.transform.position + Vector3.down * range;
                Handles.color = Color.red;
                distance = range;
            }
            Handles.DrawLine(gm.transform.position, last);
        }

    }
}
