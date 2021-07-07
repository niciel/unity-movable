using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FBXMeshExtractor : EditorWindow
{
    private static string _progressTitle = "Extracting Meshes";
    private static string _sourceExtension_1 = ".fbx";
    private static string _sourceExtension_2 = ".FBX";
    private static string _targetExtension = ".asset";

    public delegate bool ExtractElement(FBXMeshExtractor extractor, Object obj);


    [MenuItem("Assets/Extract From FBX", validate = true)]
    private static bool ExtractMeshesMenuItemValidate()
    {
        if (GetAssetObject() == null)
            return false;
        return true;
    }

    private static Object GetAssetObject()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            if (AssetDatabase.GetAssetPath(Selection.objects[i]).EndsWith(_sourceExtension_1) )
                return Selection.objects[i];
            else if (AssetDatabase.GetAssetPath(Selection.objects[i]).EndsWith(_sourceExtension_2))
                return Selection.objects[i];
        }
        return null;

    }

    protected Object selectedAsset;

    [MenuItem("Assets/Extract From FBX")]
    private static void ExtractMeshesMenuItem()
    {
        FBXMeshExtractor windo = EditorWindow.GetWindow<FBXMeshExtractor>();
        windo.FBX = GetAssetObject();
        windo.path = AssetDatabase.GetAssetPath(windo.FBX);
        windo.guid = AssetDatabase.AssetPathToGUID(windo.guid);
        windo.init();
    }

    public Object FBX;
    public string guid;
    public string path;



    private bool[] selected;
    private Object[] allAssets;


    

    void init()
    {
        allAssets = AssetDatabase.LoadAllAssetsAtPath(path);
        Debug.Log("paths " + allAssets.Length + " " + path);
        selected = new bool[allAssets.Length];

    }
    Vector2 scroll = Vector2.zero;
    void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);
        for (int i = 0; i < allAssets.Length; i++)
        {
            selected[i] = EditorGUILayout.Toggle(allAssets[i].name, selected[i]);
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("extract"))
        {
            for (int i = 0; i < allAssets.Length; i++)
            {
                if (selected[i] == false)
                    continue;
                Extract(allAssets[i]);
            }
            Close();
        }
    }

    public void Extract(Object[] objs)
    {
        EditorUtility.DisplayProgressBar(_progressTitle, "", 0);
        for (int i = 0; i < objs.Length; i++)
        {
            EditorUtility.DisplayProgressBar(_progressTitle, Selection.objects[i].name, (float)i / (objs.Length - 1));
            Extract(objs[i]);
        }
        EditorUtility.ClearProgressBar();
    }


    private static void Extract(Object selectedObject)
    {
        //Create Folder Hierarchy
        string selectedObjectPath = AssetDatabase.GetAssetPath(selectedObject);
        string parentfolderPath = selectedObjectPath.Substring(0, selectedObjectPath.Length - (selectedObject.name.Length + 5));
        string objectFolderName = selectedObject.name;
        string meshFolderName = "Meshes";
        string meshFolderPath = parentfolderPath + "/" + meshFolderName;

        if (!AssetDatabase.IsValidFolder(meshFolderPath))
        {
            AssetDatabase.CreateFolder(parentfolderPath, meshFolderName);
        }

        //Create Meshes
        Object[] objects = AssetDatabase.LoadAllAssetsAtPath(selectedObjectPath);

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] is Mesh)
            {
                EditorUtility.DisplayProgressBar(_progressTitle, selectedObject.name + " : " + objects[i].name, (float)i / (objects.Length - 1));

                Mesh mesh = Object.Instantiate(objects[i]) as Mesh;

                AssetDatabase.CreateAsset(mesh, meshFolderPath + "/" + objects[i].name + _targetExtension);
            }
        }

        //Cleanup
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }



}