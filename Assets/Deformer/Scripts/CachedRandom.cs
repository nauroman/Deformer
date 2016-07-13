using UnityEngine;
using System.Collections;

public class CachedRandom
{
    const int DEFAULT_LENGTH = 255;
    static float[] cache;
    static int index = 0;

    public static void UpdateCashe(int length, float min, float max)
    {
        cache = new float[length];

        for (int i = 0; i < length; i++)
            cache [i] = Random.Range(min, max);
    }

    public static void Clear()
    {
        cache = new float[0];
    }

    public static float Get(int seed)
    {
        if (cache.Length <= 0)
            UpdateCashe(DEFAULT_LENGTH, 0, 1);

        return cache [GetIndex(seed)];
    }

    public static float GetNext()
    {
        if (cache.Length <= 0)
        {
            UpdateCashe(DEFAULT_LENGTH, 0, 1);
            index = 0;
        }

        if (index >= cache.Length)
            index = 0;

        return cache [index++];
    }

    static int GetIndex(int i)
    {
        if (i < cache.Length && i >= 0)
            return i;

        return i % cache.Length;
    }


    public class RNG
    {
        // Seeds
        static uint m_w = 362436069;
        /* must not be zero */
        static uint m_z = 521288629;
        /* must not be zero */

        public int NextRandom()
        {
            m_z = 36969 * (m_z & 65535) + (m_z >> 16);
            m_w = 18000 * (m_w & 65535) + (m_w >> 16);
            return (int)((m_z << 16) + m_w);
        }
    }

}
