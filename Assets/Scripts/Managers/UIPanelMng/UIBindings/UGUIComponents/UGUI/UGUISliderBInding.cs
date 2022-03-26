using Doyo.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIBinding
{

    public class UGUISliderBInding : SingleBinding
    {
        public Slider Slider;

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
            if (_property != null)
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

        public sealed override void OnChanged(bool forceChange)
        {
            if (IgnoreChanges && !forceChange)
            {
                return;
            }

            float value = (float)GetRawValue();
            Slider.value = value;
        }
    }


}