using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum AdjustmentHandlyType
{
    NONE,
    MOVE,
    SCALE,
    RESCALE
    
        //TODO moze w przyszlosci rozszerzanie w wybranym kierunku
}


public class BoxColliderElement
{
    Vector3 lastPosition;
    Vector3 position;
    public BoxCollider collider;
    GameObject gm;
    public bool enabled;
    public int instanceID;

    public void init(BoxCollider collider) 
    {
        this.collider = collider;
        gm = collider.gameObject;
        position = collider.center + gm.transform.position;
        lastPosition = position;
        enabled = collider.enabled;
        this.instanceID = collider.GetInstanceID();
    }

    public void onSceneGui()
    {
        if (! gm.activeSelf || !collider.enabled)
        {
            return;
        }
        Vector3 worldPosition = gm.transform.position + (gm.transform.rotation * collider.center);

        AdjustmentHandlyType mode = BoxColliderAdjustment.MainInstance.moveMode;

        Debug.Log("wtf " + (collider.center + collider.gameObject.transform.position));


        if (mode == AdjustmentHandlyType.NONE)
        {
            if (enabled)
            {
                Handles.color = Color.green;
            }
            else
            {
                Handles.color = Color.red;
            }
            float handleSize = HandleUtility.GetHandleSize(worldPosition)* 0.4f;
            if (Handles.Button(worldPosition,
                gm.transform.rotation,
                handleSize,
                handleSize,
                Handles.CubeHandleCap))
            {
                BoxColliderAdjustment.lastModifed = instanceID;
                enabled = !enabled;
            }
            Handles.color = Color.white;
        }
        else if (enabled == false)
        {
            return;
        }
        else if (mode == AdjustmentHandlyType.MOVE)
        {
            position = Handles.PositionHandle(worldPosition, gm.transform.rotation);
            if (position.Equals(worldPosition))
                return;
            else
            {
                BoxColliderAdjustment.lastModifed = instanceID;
                position -= gm.transform.position;
                position = (Quaternion.Inverse(gm.transform.rotation) * position).Round(3);
                if (!position.Equals(lastPosition))
                {
                    collider.center = position;
                }
                lastPosition = collider.center.Round(3);
                Debug.Log("debug");
            }

        }
        else if (mode == AdjustmentHandlyType.SCALE)
        {
            Vector3 scale = collider.size;
            scale = Handles.ScaleHandle(scale, worldPosition, gm.transform.rotation, HandleUtility.GetHandleSize(worldPosition));
            if (collider.size.Equals(scale))
                return;
            BoxColliderAdjustment.lastModifed = instanceID;
            if (scale.x < 0 || scale.y < 0 || scale.z < 0)
            {
                return;
            }
            collider.size = scale;
        }
        else if (mode == AdjustmentHandlyType.RESCALE)
        {
            DrawHandleDirectionalScale(worldPosition, gm.transform.rotation);
        }

    }

    private Vector3[] directions = { Vector3.up , Vector3.forward , Vector3.right , Vector3.down , Vector3.back , Vector3.left};
    public void DrawHandleDirectionalScale(Vector3 position, Quaternion rotation)
    {
        float scale;
        float lscale;
        Vector3 newColliderS;
        foreach (Vector3 dir in directions)
        {
            scale = collider.size.Multiply(dir).magnitude / 2;
            lscale = scale;
            scale = Handles.ScaleSlider(scale, position, rotation * dir, rotation, HandleUtility.GetHandleSize(position), Handles.SnapValue(1f, 1f));
            if (lscale == scale)
                continue;
            BoxColliderAdjustment.lastModifed = instanceID;
            Vector3 normalScale = collider.size/2 ;
            Vector3 afterScale = normalScale.Multiply(dir.Not());
            afterScale = afterScale + (dir.Abs() * scale);
            Vector3 dif = afterScale - normalScale;
            newColliderS = collider.size + dif / 2;
            float s = newColliderS.Multiply(dir.Abs()).magnitude;
            collider.size = newColliderS;
            collider.center = collider.center + (dif / 4).Multiply(dir);
        }
    }

}



[CustomEditor(typeof(BoxCollider))]
public class BoxColliderAdjustment : Editor 
{
    private static BoxColliderAdjustment _mainInstance;
    private static List<BoxColliderElement> lastInstance;
    public static int lastModifed = -1;
    public bool showOnlySelected = false;
    public static BoxColliderAdjustment MainInstance
    {
        get
        {
            return _mainInstance;
        }
    }

    private List<BoxColliderElement> root;

    public int lastInstanceID = -1;
    public int gameObjectInstanceID;
    public AdjustmentHandlyType moveMode = AdjustmentHandlyType.MOVE;


    Tool lastTool;

    void Awake()
    {

        if (_mainInstance == null)
        {
            lastTool = Tools.current;
            Tools.current = Tool.None;
            GameObject obj = ((BoxCollider)target).gameObject;
            _mainInstance = this;
            this.lastInstanceID = obj.GetInstanceID();
            root = new List<BoxColliderElement>();
            initList(obj);
        }
    }

    void OnDisable()
    {
        if (_mainInstance != null && _mainInstance.GetInstanceID() == this.GetInstanceID())
        {
            Tools.current = lastTool;
            lastInstance = this.root;
            _mainInstance = null;
        }
    }

    public void Duplicate( BoxCollider col)
    {
        BoxCollider ncol = col.gameObject.AddComponent<BoxCollider>();
        ncol.center = col.center;
        ncol.size = col.size;
        if (MainInstance == null)
            return;
        foreach (BoxColliderElement e in MainInstance.root)
        {
            if (e.collider.GetInstanceID() == col.GetInstanceID())
            {
                e.enabled = false;
                break;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BoxCollider col = target as BoxCollider;
        if (GUILayout.Button("duplicate"))
        {
            Duplicate(col);
        } 
    }

    public BoxColliderElement GetLastSelectedCollider()
    {
        foreach (BoxColliderElement e in root)
        {
            if (e.instanceID == lastModifed)
                return e;
        }
        return null;
    }

    public void OnSceneGUI()
    {

        if (_mainInstance == null || _mainInstance.GetInstanceID() != this.GetInstanceID())
        {
            return;
        }
        if (Tools.current != Tool.None)
        {
            Tools.current = Tool.None;
        }
        BoxColliderElement e = GetLastSelectedCollider();
        if (e != null)
        {
            DebugExtension.DebugLocalCube(e.collider.transform.root.localToWorldMatrix, e.collider.size*0.99f, Color.blue, e.collider.center , 0 , false);
            DebugExtension.DebugLocalCube(e.collider.transform.root.localToWorldMatrix, e.collider.size * 1.01f, Color.blue, e.collider.center, 0, false);
        }

        Handles.BeginGUI();
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.black;

        GUILayout.BeginVertical();
        if (GUILayout.Button("aktywna edycja colliderow " + moveMode , style, GUILayout.ExpandWidth(false)))
        {
            ChangeEditMode();
        }
        if (GUILayout.Button("show only selected " + showOnlySelected,style, GUILayout.ExpandWidth(false))) {
            showOnlySelected = ! showOnlySelected;
        }
        GUILayout.EndVertical();

        if (Event.current.isKey ) 
        {
            if (Event.current.keyCode == KeyCode.Space)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("duplikuj"), false, Funca, 0);
                menu.AddItem(new GUIContent("usun"), false, Funca, 1);
                menu.ShowAsContext();
            }
            if (Event.current.keyCode == KeyCode.X)
            {
                lastModifed = -1;
            }
        }
        Handles.EndGUI();
        if (Event.current.isKey && Event.current.type == EventType.KeyUp)
        {
            if (Event.current.keyCode == KeyCode.R)
            {
                ChangeEditMode();
            }

        }



        UnityEngine.Object obj = Selection.activeObject;
        if ( !(obj is GameObject)) 
        {
            return;
        }
        GameObject gm = (GameObject)obj;

        if (gameObjectInstanceID != gm.GetInstanceID())
        {
            initList(gm);
        }
        foreach (BoxColliderElement bca in root)
        {

            if (moveMode != AdjustmentHandlyType.NONE)
            {
                if (showOnlySelected && lastModifed != -1)
                {
                    if (bca.instanceID != lastModifed)
                        continue;
                }
            }
            bca.onSceneGui();

        }

    }


    public void ChangeEditMode()
    {
        System.Array v = typeof(AdjustmentHandlyType).GetEnumValues();
        for (int i = 0; i < v.Length; i++)
        {
            if (!v.GetValue(i).Equals(moveMode))
            {
                continue;
            }
            if (i + 1 < v.Length)
                moveMode = (AdjustmentHandlyType)v.GetValue(i + 1);
            else
                moveMode = (AdjustmentHandlyType)v.GetValue(0);
            break;
        }
    }

    private void Funca(object option)
    {
        int i = (int)option;
        BoxColliderElement selected = GetLastSelectedCollider();
        if (selected != null)
        {
            if (i == 0)
                Duplicate(selected.collider);
            else if (i ==1)
            {
                DestroyImmediate(selected.collider);
                UnityEngine.Object obj = Selection.activeObject;
                if (!(obj is GameObject))
                    return;
                GameObject gm = (GameObject)obj;
                initList(gm);
            }

        }
    }


    private void initList(GameObject gm)
    {
        root.Clear();
        BoxCollider[] colliders = gm.GetComponentsInChildren<BoxCollider>();
        BoxColliderElement element;
        gameObjectInstanceID = gm.GetInstanceID();
        foreach (BoxCollider bc in colliders)
        {
            element = null;
            if (lastInstance != null)
            {
                foreach (BoxColliderElement e in lastInstance)
                {
                    if (e.collider.GetInstanceID() == bc.GetInstanceID())
                    {
                        element = e;
                        break;
                    }
                }

            }
            if (element == null)
            {
                element = new BoxColliderElement();
                element.init(bc);
            }
            root.Add(element);
        }
    }


}
