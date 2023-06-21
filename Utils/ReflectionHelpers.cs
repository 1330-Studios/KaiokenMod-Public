using System;
using System.Reflection;

namespace KaiokenMod.Utils;

internal static class ReflectionHelpers {
    internal const BindingFlags ALL = (BindingFlags)(-1);

    internal static T Invoke<T>(this object obj, string methodName, params object[] args) {
        return (T)obj.GetType().GetMethod(methodName, ALL)?.Invoke(obj, args);
    }

    internal static void Invoke(this Type obj, string methodName, params object[] args) {
        obj.GetMethod(methodName, ALL)?.Invoke(null, args);
    }

    internal static T Invoke<T>(this TypeInfo obj, string methodName, params object[] args) {
        return (T)obj.AsType().GetMethod(methodName, ALL)?.Invoke(null, args);
    }

    internal static void Invoke(this TypeInfo obj, string methodName, params object[] args) {
        obj.AsType().GetMethod(methodName, ALL)?.Invoke(null, args);
    }
}