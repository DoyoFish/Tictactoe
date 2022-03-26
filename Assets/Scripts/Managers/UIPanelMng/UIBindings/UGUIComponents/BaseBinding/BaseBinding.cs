using Doyo.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UIBinding
{

    public abstract class BaseBinding : MonoBehaviour
    {

        private readonly List<Type> _textPropertyTypes = new List<Type>()
        {
            typeof(string), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(bool)
        };

        private readonly List<Type> _numericPropertyTypes = new List<Type>()
        {
            typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double)
        };

        [HideInInspector]
        public bool Binded = false;
        public ModelView ModelView;

        protected bool IgnoreChanges;

        [NonSerialized]
        public bool IsModelViewFound = false;
        [NonSerialized]
        public bool IsPropertyFound = false;

        protected abstract void Bind();

        protected abstract void UnBind();

        public virtual void OnChanged(bool forceChange) { }

        public void UpdateBinding()
        {
            UnBind();
            Bind();
            OnChanged(false);
            Binded = false;
        }

        public virtual void Start()
        {
            if (!Binded)
            {
                UpdateBinding();
            }
            else
            {
                OnChanged(false);
            }
        }

        public virtual void OnDestroy()
        {
            UnBind();
        }

        protected ModelView FindModelView(ref string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            if (ModelView)
            {
                IsModelViewFound = true;
                return ModelView;
            }

            ModelView = GetItemModelView<ItemModelView>(ref path);
            if (!ModelView)
            {
                ModelView = GetItemModelView<PanelModelView>(ref path);
            }

            IsModelViewFound = ModelView;
            return ModelView;
        }

        private T GetItemModelView<T>(ref string path) where T : ModelView
        {
            string origin = path;
            var modelView = GetComponentInParent<T>();
            while (path.StartsWith("/"))
            {
                if (modelView)
                {
                    modelView = modelView.GetComponentInParent<T>();
                    path = path.Substring(1);
                }
                else
                {
                    break;
                }
            }
            if (modelView)
            {
                return modelView;
            }
            else
            {
                path = origin;
                return null;
            }
        }

        protected Property FillTextProperties(string path, string processPath)
        {
            var modelView = FindModelView(ref path);
            if (!modelView)
            {
                return null;
            }

            var property = modelView.FindProperty(path, processPath);

            if (property == null)
            {
                return null;
            }

            if (_textPropertyTypes.Contains(property.GetPropertyType()))
            {
                return property;
            }

            return null;
        }

        protected static Message GetTextValue(Property property)
        {
            if (property == null)
            {
                return null;
            }
            if (property is Property<string> stringP)
            {
                return stringP.GetValue();
            }
            if (property is Property<float> floatP)
            {
                return floatP.GetValue();
            }
            if (property is Property<double> doubleP)
            {
                return doubleP.GetValue();
            }
            if (property is Property<byte> byteP)
            {
                return byteP.GetValue();
            }
            if (property is Property<short> shortP)
            {
                return shortP.GetValue();
            }
            if (property is Property<int> intP)
            {
                return intP.GetValue();
            }
            if (property is Property<long> longP)
            {
                return longP.GetValue();
            }
            return null;
        }

        protected void SetTextValue(Property property, string val)
        {
            if (property is Property<string> stringP)
            {
                stringP.SetValue(val);
            }
            else if (property is Property<float> floatP)
            {
                floatP.SetValue(val.ParseFloat());
            }
            else if (property is Property<double> doubleP)
            {
                doubleP.SetValue(val.ParseDouble());
            }
            else if (property is Property<byte> byteP)
            {
                byteP.SetValue(val.ParseByte());
            }
            else if (property is Property<short> shortP)
            {
                shortP.SetValue(val.ParseShort());
            }
            else if (property is Property<int> intP)
            {
                intP.SetValue(val.ParseInt());
            }
            else if (property is Property<long> longP)
            {
                longP.SetValue(val.ParseLong());
            }
        }

        protected T FillProperty<T>(string path, string processPath) where T : Property
        {
            var modelView = FindModelView(ref path);
            if (!modelView)
            {
                return null;
            }
            var property = modelView.FindProperty(path, processPath);
            if (property == null)
            {
                return null;
            }
            var result = property as T;
            if (result == null)
            {
                Debug.LogError(string.Format("path : {0} , processedPath : {1} Property Found But Not UIBinding Property {2}", path,
                    processPath, typeof(T)), gameObject);
                return null;
            }
            return result;
        }

        protected Property FillNumericProperties(string path, string processPath)
        {
            var modelView = FindModelView(ref path);
            if (!modelView)
            {
                return null;
            }

            var property = modelView.FindProperty(path, processPath);
            if (property == null)
            {
                return null;
            }

            if (_numericPropertyTypes.Contains(property.GetPropertyType()))
            {
                return property;
            }
            return null;
        }

        protected static Double GetNumericValue(Property property)
        {
            if (property == null)
            {
                return 0;
            }
            if (property is Property<byte> byteP)
            {
                return byteP.GetValue();
            }
            if (property is Property<short> shortP)
            {
                return shortP.GetValue();
            }
            if (property is Property<int> intP)
            {
                return intP.GetValue();
            }
            if (property is Property<long> longP)
            {
                return longP.GetValue();
            }
            if (property is Property<float> floatP)
            {
                return floatP.GetValue();
            }
            if (property is Property<double> doubleP)
            {
                return doubleP.GetValue();
            }
            return 0;
        }

        protected static void SetNumericValue(Property property, double val)
        {
            if (property is Property<byte> byteP)
            {
                byteP.SetValue((byte)val);
            }
            else if (property is Property<short> shortP)
            {
                shortP.SetValue((short)val);
            }
            else if (property is Property<int> intP)
            {
                intP.SetValue((int)val);
            }
            else if (property is Property<long> longP)
            {
                longP.SetValue((long)val);
            }
            else if (property is Property<float> floatP)
            {
                floatP.SetValue((float)val);
            }
            else if (property is Property<double> doubleP)
            {
                doubleP.SetValue(val);
            }
        }

        public static string GetDisplayHierarchy(Transform transform)
        {
            if (transform == null) return "null";

            if (transform.parent != null) return GetDisplayHierarchy(transform.parent) + "\\" + transform.name;

            return transform.name;
        }
    }

}