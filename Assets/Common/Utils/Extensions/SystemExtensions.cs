using UnityEngine;

public static class SystemExtensions
{
    public static bool IsNull(this object obj)
    {
        return obj == null;
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
}
