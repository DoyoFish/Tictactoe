using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIBinding
{

    public class Property<T> : Property
    {
        private readonly bool _isValueType;

        private bool _brocasting;

        protected T Value;

        public Property() 
        {
            _isValueType = typeof(T).IsValueType;
        }

        public Property(T value) : this()
        {
            Value = value;
        }

        public bool IsDestinationType<U>()
        {
            return typeof(U) == typeof(T);
        }

        protected bool IsValueDifferent(T value)
        {
            return !Value.Equals(value);
        }

        private bool IsClassDifferent(T value)
        {
            return !Value.Equals(value);
        }

        public virtual void SetValue(T value, bool forceUpdate = false)
        {
            if(_brocasting && !forceUpdate)
            {
                return;
            }

            _brocasting = true;

            bool changed = false;

            if (_isValueType)
            {
                changed = IsValueDifferent(value);
            }
            else
            {
                changed = (value == null && Value != null)
                    || (value != null && Value == null)
                    || (Value != null && IsClassDifferent(value));
            }

            if (changed)
            {
                Value = value;
                Apply(forceUpdate);
            }

            _brocasting = false;
        }

        public T GetValue()
        {
            return Value;
        }

        public override Type GetPropertyType()
        {
            return typeof(T);
        }
    }

}