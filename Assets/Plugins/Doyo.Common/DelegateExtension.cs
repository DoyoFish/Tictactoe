using System;
using System.Reflection;

namespace Doyo.Common
{
    public static class DelegateExtension
    {
        public static T GetDelegateInInstance<T>(this object instance, string methodName, BindingFlags flags) where T : class
        {
            if (instance == null)
            {
                return null;
            }
            var instanceType = instance.GetType();
            var methodInfo = instanceType.GetMethod(methodName, flags);
            if (methodInfo == null)
            {
                return null;
            }
            var method = Delegate.CreateDelegate(typeof(T), instance, methodInfo) as T;
            return method;
        }

        public static T GetDelegateInStaticClass<T>(this Type type, string methodName, BindingFlags flags) where T : class
        {
            if (type == null)
            {
                return null;
            }
            var methodInfo = type.GetMethod(methodName, flags);
            if (methodInfo == null)
            {
                return null;
            }
            var method = Delegate.CreateDelegate(typeof(T), methodInfo) as T;
            return method;
        }
    }
}
