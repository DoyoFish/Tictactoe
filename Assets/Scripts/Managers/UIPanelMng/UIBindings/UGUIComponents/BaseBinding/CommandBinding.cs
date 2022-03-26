using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIBinding
{
    public abstract class CommandBinding : BaseBinding
    {
        public string Path;

        protected virtual void OnValidate()
        {
            if(string.IsNullOrEmpty(Path))
            {
                Debug.LogErrorFormat("Path is Null \n{0}", GetDisplayHierarchy(transform));
            }
            else
            {
                Path = Path.Trim();
            }
        }
    }

}