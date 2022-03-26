using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UIBinding
{

    public abstract class SingleBinding : BaseBinding
    {
        public string Path;
        
        public string ProcessPath;

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(Path))
            {
                Debug.LogError(string.Format("Path Is Null \n{0}", GetDisplayHierarchy(transform)));
            }
            else
            {
                if (!Application.isPlaying)
                {
                    Path = Path.Trim();
                    if (Path.IndexOf('.') < 0) ProcessPath = string.Format("_property{0}", Path);
                }
            }
        }
    }

}