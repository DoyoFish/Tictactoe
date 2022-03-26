using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UIBinding
{
    public abstract class ActionBinding : CommandBinding
    {
        protected MethodInfo _method;

        protected override void UnBind()
        {
            _method = null;
        }

        protected override void Bind()
        {
            var modelView = FindModelView(ref Path);
            if (!modelView)
            {
                return;
            }
            _method = ModelView.FindAction(Path);
            if(_method != null)
            {
                IsPropertyFound = true;
            }
            else
            {
                IsPropertyFound = false;
                UnityEngine.Debug.LogWarning(string.Format("Action is null ({0})", Path), gameObject);
            }
        }

        protected Action CreateAction()
        {
            return _method.CreateDelegate(typeof(Action), ModelView.Instance) as Action;
        }

        protected Action<T> CreateAction<T>()
        {
            return _method.CreateDelegate(typeof(Action<T>), ModelView.Instance) as Action<T>;
        }
    }

}