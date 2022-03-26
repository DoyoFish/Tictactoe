using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Doyo.UnityFramework
{
    public static class JavaObjectExtension
    {
        public static void SafeCall(this AndroidJavaObject javaObject, string methodName, params object[] args)
        {
            if (javaObject == null)
            {
                return;
            }
            javaObject.Call(methodName, args);
        }

        public static T SafeCall<T>(this AndroidJavaObject javaObject, string methodName, params object[] args)
        {
            if (javaObject == null)
            {
                return default(T);
            }
            return javaObject.Call<T>(methodName, args);
        }

        public static void SafeCallStatic(this AndroidJavaObject javaObject, string methodName, params object[] args)
        {
            if (javaObject == null)
            {
                return;
            }
            javaObject.CallStatic(methodName, args);
        }

        public static T SafeCallStatic<T>(this AndroidJavaObject javaObject, string methodName, params object[] args)
        {
            if (javaObject == null)
            {
                return default(T);
            }
            return javaObject.CallStatic<T>(methodName, args);
        }
    }
}
