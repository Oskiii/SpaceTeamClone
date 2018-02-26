using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public static class MyExtensions
{
    public static T PopAt<T>(this List<T> list, int index)
    {
        T r = list[index];
        list.RemoveAt(index);
        return r;
    }

    public static void ShuffleInPlace<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static double GetLuminance(this Color color)
    {
        return 0.2126 * color.r + 0.7152 * color.b + 0.0722 * color.b;
    }

    public static float Scale(this float value, float min, float max, float minScale, float maxScale)
    {
        return (float) (minScale + (double) (value - min) / (max - min) * (maxScale - minScale));
    }

    public static int Scale(this int value, int min, int max, int minScale, int maxScale)
    {
        return Mathf.RoundToInt((float) (minScale + (double) (value - min) / (max - min) * (maxScale - minScale)));
    }

    public static Color ToColor(this int HexVal)
    {
        var R = (byte) ((HexVal >> 16) & 0xFF);
        var G = (byte) ((HexVal >> 8) & 0xFF);
        var B = (byte) (HexVal & 0xFF);
        return new Color(R, G, B, 255);
    }

    public static List<T> Splice<T>(this List<T> list, int index, int count)
    {
        List<T> range = list.GetRange(index, count);
        list.RemoveRange(index, count);
        return range;
    }
}

public static class ThreadSafeRandom
{
    [ThreadStatic] private static Random Local;

    public static Random ThisThreadsRandom
    {
        get
        {
            return Local
                   ?? (Local = new Random(
                           unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
        }
    }
}

//Extension class to provide serialize / deserialize methods to object.
//src: http://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
//NOTE: You need add [Serializable] attribute in your class to enable serialization
public static class ObjectSerializationExtension
{
    public static byte[] SerializeToByteArray(this object obj)
    {
        if (obj == null)
        {
            return null;
        }
        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static T Deserialize<T>(this byte[] byteArray) where T : class
    {
        if (byteArray == null)
        {
            return null;
        }
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(byteArray, 0, byteArray.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (T) binForm.Deserialize(memStream);
            return obj;
        }
    }
}