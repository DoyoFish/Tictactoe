using Doyo.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIBinding
{

    public abstract class TextBinding : SingleBinding
    {
        public string Format = "{0}";

        private Property _property;

        protected override void Bind()
        {
            _property = FillTextProperties(Path, ProcessPath);
            if (_property != null)
            {
                _property.OnChanged += OnChanged;
                IsPropertyFound = true;
            }
            else
            {
                Debug.LogWarning(string.Format("Property is null ({0})", Path), gameObject);
                IsPropertyFound = false;
            }
            var modelView = FindModelView(ref Path);
            if (!modelView)
            {
                return;
            }
            ModelView = modelView;
        }

        protected override void UnBind()
        {
            if(_property != null)
            {
                _property.OnChanged -= OnChanged;
                _property = null;
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!string.IsNullOrEmpty(Path))
            {
                Path = Path.Trim();
                if (Path.IndexOf('.') < 0)
                {
                    ProcessPath = string.Format("_property{0}", Path);
                }
            }
        }

        private Message GetRawValue()
        {
            return GetTextValue(_property);
        }

        public sealed override void OnChanged(bool forceUpdate)
        {
            if (IgnoreChanges && !forceUpdate)
            {
                return;
            }

            string txt = Formatter(Format, GetRawValue().ToString());
            OnValueChanged(txt);
        }

        protected abstract void OnValueChanged(string txt);

        public static string Formatter(string format, object value)
        {
            return string.Format(format, value);
        }
    }

}