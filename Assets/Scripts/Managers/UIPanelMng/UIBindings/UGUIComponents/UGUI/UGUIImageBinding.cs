using Doyo.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIBinding
{

    public class UGUIImageBinding : SingleBinding
    {
        public static Func<string, Sprite> SpriteLoader;

        public Image Image;
        public bool PerfectPixel;
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
            if (!Image)
            {
                Image = this.GetComponent<Image>();
            }
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
            string txt = (string)GetRawValue();
            if (Image)
            {
                var size = Image.rectTransform.sizeDelta;
                var sprite = SpriteLoader.Invoke(txt);
                if (!sprite)
                {
                    Debug.LogError($"Path: ({txt}) is null", this);
                }
                Image.sprite = sprite;
                if (PerfectPixel)
                {
                    Image.SetNativeSize();
                }
                else
                {
                    Image.rectTransform.sizeDelta = size;
                }
            }
        }
    }

}