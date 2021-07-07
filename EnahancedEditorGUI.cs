using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EnahancedEditorGUI
{
    public delegate void InitElemenet<T>(Rect rect, T element);
    public static void ScroledMultiRowList<T>(Rect rect, T[] list, InitElemenet<T> element, Vector2 elemetsize, Vector2 gapeSize)
    {
        float gape = rect.width - (2 * gapeSize.x);
        int max = 0;
        while (gape >= (128))
        {
            gape -= (128 + gapeSize.x);
            max++;
        }
        if (max == 0)
            max = 1;
        float yPos;
        float xPos;
        int elementPosition = 0;
        int rows = 0;
        int maxRows = (int)list.Length / max;
        if (list.Length % max != 0)
            maxRows++;
        Rect newRect;
        EditorGUILayout.LabelField("", GUILayout.Height((gapeSize.y + elemetsize.y) * maxRows + gapeSize.y+elemetsize.y), GUILayout.Width((gapeSize.x + elemetsize.x) * max + gapeSize.x));

        while (elementPosition < list.Length)
        {
            yPos = rows * (elemetsize.y + gapeSize.y) + gapeSize.y +rect.y;
            for (int i = 0; i < max; i++)
            {
                if (list.Length <= elementPosition)
                {
                    break;
                }
                xPos = (gapeSize.x + elemetsize.x) * i + gapeSize.x;
                newRect = new Rect(xPos, yPos, elemetsize.x, elemetsize.y);
                element(newRect, list[elementPosition]);
                elementPosition++;
            }
            rows++;
        }

    }

    public static void ScroledMultiRowList<T>(Rect rect, List<T> list, InitElemenet<T> element, Vector2 elemetsize, Vector2 gapeSize)
    {
        ScroledMultiRowList<T>(rect, list.ToArray(), element, elemetsize, gapeSize);
    }
}