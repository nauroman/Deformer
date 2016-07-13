using UnityEngine;
using System.Collections;
using System;

public class CachedMagnitude
{
    public const int DEFAULT_SIZE = 64;
    static public int size = 0;
    static int length;
    static byte[] cache;

    static int Length{ get { return length; } }

    static public int Magnitude(Vector3 a)
    {
        if (size <= 0)
        {
            UpdateCashe(DEFAULT_SIZE);
        }

        var index = GetIndex(a.x, a.y, a.z);

        if (index == -1)
            return -1;

        return cache [index];
//        return Vector3.Magnitude(a);
    }

    public static void UpdateCashe(int size)
    {
        CachedMagnitude.size = size;
        length = size * size * size;
        cache = new byte[length];

        for (float x = 0; x < size; x++)
            for (float y = 0; y < size; y++)
                for (float z = 0; z < size; z++)
                {
                    var index = GetIndex(x, y, z);
                    cache [index] = (byte)Vector3.Magnitude(new Vector3(x, y, z));
                }
    }


    public static int GetIndex(float x, float y, float z)
    {
        if (x < 0)
            x = -x;

        if (x >= size)
            return -1;

        if (y < 0)
            y = -y;

        if (y >= size)
            return -1;
        
        if (z < 0)
            z = -z;

        if (z >= size)
            return -1;
        
        return (int)x + (int)z * size + (int)y * size * size;

//        return index >= length ? length - 1 : index;
    }

}
