using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace GameCoreUtils
{

    public class NormalizeMeshSize : EditorWindow {


        public GameObject gm;



        public void OnGUI()
        {
                        
        }


        [MenuItem("Assets/normalize mesh", validate = true)]
        public static bool CanNormalizeMeshScale()
        {
            if (Selection.activeObject != null)
            {
                if (Selection.activeObject is GameObject)
                {
                    return true;
                }
            }
            return false;
        }


        [MenuItem("Assets/normalize mesh")]
        public static void NormalizeMeshScale()
        {
            GameObject gm = (GameObject)Selection.activeObject;
            Mesh m = gm.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] tris = m.vertices;
            Vector3 v;
            Vector3 s = gm.transform.localScale;
            gm.transform.localScale = Vector3.one;
            for (int i = 0; i < tris.Length; i++)
            {
                v = tris[i];
                v.x = v.x * s.x;
                v.y = v.y * s.y;
                v.z = v.z * s.z;
                tris[i] = v;
            }
            m.vertices = tris;
            m.RecalculateBounds();
        }

    }

public static class GCoreUtil
    {





        public static T[] CopyAndAdd<T>(T[] tocopy, params T[] add)
        {
            T[] ret = new T[tocopy.Length + add.Length];
            for (int i = 0; i < tocopy.Length; i++)
                ret[i] = tocopy[i];
            int newpos = tocopy.Length;
            for (int i = 0; i < add.Length; i++)
            {
                ret[newpos] = add[i];
                newpos++;
            }
            return ret;
        }

        public static T[] AddElement<T>(T[] target, params T[] items)
        {
            if (target == null)
            {
                target = new T[] { };
            }
            if (items == null)
            {
                items = new T[] { };
            }
            T[] result = new T[target.Length + items.Length];
            target.CopyTo(result, 0);
            items.CopyTo(result, target.Length);
            return result;
        }


        public static void CreateFolder( string path)
        {
            string[] elements = path.Split('/');
            StringBuilder sb = new StringBuilder(elements[0]);
            string last;
            for (int i = 1; i < elements.Length; i++)
            {
                last = sb.ToString();
                sb.Append('/').Append(elements[i]);
                if (AssetDatabase.IsValidFolder(sb.ToString()) == false ) {
                    AssetDatabase.CreateFolder(last, elements[i]);
                }
            }

        }
    }
}

