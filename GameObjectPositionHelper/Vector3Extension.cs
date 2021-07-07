using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension 
{
    public static Vector3 Round(this Vector3 v , int places = 2)
    {
        float multi = Mathf.Pow(10, places);
        return new Vector3(Mathf.Round(v.x*multi)/multi ,
            Mathf.Round(v.y *multi)/multi,
            Mathf.Round(v.z*multi)/multi
            );
    }


    public static Vector3 Multiply(this Vector3 v, float value)
    {
        return new Vector3(v.x * value,
            v.y * value,
            v.z * value
            );
    }

    public static Vector3 Multiply(this Vector3 v, Vector3 value)
    {
        return new Vector3(v.x * value.x,
            v.y *value.y,
            v.z * value.z
            );
    }


    public static Vector3 Not(this Vector3 v)
    {
        Vector3 ret = Vector3.zero;
        if (v.x != 0)
            ret.x = 0;
        else
            ret.x = 1;
        //
        if (v.y != 0)
            ret.y = 0;
        else
            ret.y = 1;
        //
        if (v.z != 0)
            ret.z = 0;
        else
            ret.z = 1;
        return ret;
    }

    public static Vector3 Abs(this Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x),
            Mathf.Abs(v.y),
            Mathf.Abs(v.z)
            );
    }
}
