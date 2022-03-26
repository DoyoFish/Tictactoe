using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIBinding
{
    public abstract class Property
    {
        public event Action<bool> OnChanged;

        protected void Apply(bool forceUpdate)
        {
            OnChanged?.Invoke(forceUpdate);
        }

        public abstract Type GetPropertyType();
    }

}