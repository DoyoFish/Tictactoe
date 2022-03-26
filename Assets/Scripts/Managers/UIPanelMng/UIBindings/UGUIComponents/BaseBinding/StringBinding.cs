using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIBinding
{
    public abstract class StringBinding : SingleBinding
    {
        private Property _property;

        protected override void UnBind()
        {
            if(_property != null)
            {
                _property.OnChanged -= OnChanged;
                _property = null;
            }
        }

        protected override void Bind()
        {
            _property = FillTextProperties(Path, ProcessPath);
            if(_property != null)
            {
                _property.OnChanged += OnChanged;
                IsPropertyFound = true;
            }
            else
            {
                UnityEngine.Debug.LogWarning(string.Format("Property is null ({0})", Path), gameObject);
                IsPropertyFound = false;
            }
        }

        protected void SetValue(string newValue)
        {
            IgnoreChanges = true;
            SetTextValue(_property, newValue);
            IgnoreChanges = false;
        }

        private string GetRawValue()
        {
            var msg = GetTextValue(_property);
            return msg != null ? (string)msg : null;
        }

        public sealed override void OnChanged(bool forceChange)
        {
            if(IgnoreChanges && !forceChange)
            {
                return;
            }
            var newValue = GetRawValue();
            ApplyNewValue(newValue);
        }

        protected abstract void ApplyNewValue(string newValue);
    }

}