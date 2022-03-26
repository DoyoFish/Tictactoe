using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIBinding
{
    public enum ActiveConditionType
    {
        Boolean,
        EqualToReference,
        GreaterThanReference,
        LessThanReference,
        Between,
        EqualToString,
        IsEmpty,
    }

    public abstract class BooleanBinding : SingleBinding
    {

        public bool UseIntReference { get { return _referenceType == ReferenceType.IntReference; } }
        public bool UseBetweenReference { get { return _referenceType == ReferenceType.BetweenReference; } }
        public bool UseStringReference { get { return _referenceType == ReferenceType.StringReference; } }

        public ActiveConditionType ActiveConditionType;
        public bool Invert;

        private enum ReferenceType
        {
            None,
            IntReference,
            StringReference,
            BetweenReference
        }

        private ReferenceType _referenceType = ReferenceType.None;

        [HideInInspector]
        public int IntReference;
        [HideInInspector]
        public int MaxReference;
        [HideInInspector]
        public int MinReference;
        [HideInInspector]
        public string StringReference;

        protected Property _property;

        protected override void Bind()
        {
            if (ActiveConditionType == ActiveConditionType.IsEmpty ||
                ActiveConditionType == ActiveConditionType.Boolean ||
                ActiveConditionType == ActiveConditionType.EqualToString)
            {
                _property = FillTextProperties(Path, ProcessPath);
            }
            else
            {
                _property = FillNumericProperties(Path, ProcessPath);
            }
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
            if (ActiveConditionType == ActiveConditionType.EqualToReference
                || ActiveConditionType == ActiveConditionType.GreaterThanReference
                || ActiveConditionType == ActiveConditionType.LessThanReference)
            {
                _referenceType =  ReferenceType.IntReference;
            }
            else if (ActiveConditionType == ActiveConditionType.Between)
            {
                _referenceType = ReferenceType.BetweenReference;
            }
            else if(ActiveConditionType == ActiveConditionType.EqualToString 
                ||  ActiveConditionType == ActiveConditionType.IsEmpty)
            {
                _referenceType = ReferenceType.StringReference;
            }
            else
            {
                _referenceType = ReferenceType.None;
            }
        }

        public override void OnChanged(bool forceChange)
        {
            base.OnChanged(forceChange);
            bool result = true;
            switch (ActiveConditionType)
            {
                case ActiveConditionType.Boolean:
                    result = (_property as Property<bool>).GetValue() ;
                    break;
                case ActiveConditionType.EqualToReference:
                    result = (_property as Property<int>).GetValue() == IntReference;
                    break;
                case ActiveConditionType.GreaterThanReference:
                    result = (_property as Property<int>).GetValue() > IntReference;
                    break;
                case ActiveConditionType.LessThanReference:
                    result = (_property as Property<int>).GetValue() < IntReference;
                    break;
                case ActiveConditionType.Between:
                    int value = (_property as Property<int>).GetValue();
                    result = MinReference <= value && value <= MaxReference;
                    break;
                case ActiveConditionType.EqualToString:
                    result = (string)GetTextValue(_property) == StringReference;
                    break;
                case ActiveConditionType.IsEmpty:
                    result = string.IsNullOrEmpty(GetTextValue(_property).ToString());
                    break;
            }

            result = Invert ? !result : result;
            SetActive = result;
        }

        protected abstract bool SetActive { set; }
    } 
}
