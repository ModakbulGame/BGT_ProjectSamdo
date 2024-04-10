using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Utils
{
    public static MethodInfo[] GetFunctions<T>()
    {
        return typeof(T).GetMethods();
    }
}
