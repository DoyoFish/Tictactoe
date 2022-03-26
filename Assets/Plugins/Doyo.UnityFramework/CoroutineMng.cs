using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doyo.UnityFramework
{
    public class CoroutineMng : MonoSingleton<CoroutineMng>
    {

        protected override void Construct()
        {
        }

        public static new Coroutine StartCoroutine(IEnumerator itor)
        {
            return Instance.StartCoroutineInternal(itor);
        }

        public static new Coroutine StartCoroutine(string methodName)
        {
            return Instance.StartCoroutineInternal(methodName);
        }

        public static new Coroutine StartCoroutine(string methodName, object value)
        {
            return Instance.StartCoroutineInternal(methodName, value);
        }

        public static new void StopCoroutine(Coroutine coroutine)
        {
            Instance.StopCoroutineInternal(coroutine);
        }

        public static new void StopCoroutine(IEnumerator itor)
        {
            Instance.StopCoroutineInternal(itor);
        }

        public static new void StopCoroutine(string methodName)
        {
            Instance.StopCoroutineInternal(methodName);
        }

        public static new void StopAllCoroutines()
        {
            Instance.StopAllCoroutinesInternal();
        }

        private Coroutine StartCoroutineInternal(IEnumerator itor)
        {
            return base.StartCoroutine(itor);
        }

        private Coroutine StartCoroutineInternal(string methodName)
        {
            return base.StartCoroutine(methodName);
        }

        private Coroutine StartCoroutineInternal(string methodName, object value)
        {
            return base.StartCoroutine(methodName, value);
        }

        private void StopCoroutineInternal(Coroutine coroutine)
        {
            base.StopCoroutine(coroutine);
        }

        private void StopCoroutineInternal(string methodName)
        {
            base.StopCoroutine(methodName);
        }

        private void StopCoroutineInternal(IEnumerator itor)
        {
            base.StopCoroutine(itor);
        }

        private void StopAllCoroutinesInternal()
        {
            base.StopAllCoroutines();
        }

        protected override void Release()
        {
        }
    }

    public static class CoroutineExtension
    {
        public static Coroutine StartCoroutine(this IEnumerator itor)
        {
            return CoroutineMng.StartCoroutine(itor);
        }

        public static void Stop(this Coroutine coroutine)
        {
            CoroutineMng.StopCoroutine(coroutine);
        }
    }
}

