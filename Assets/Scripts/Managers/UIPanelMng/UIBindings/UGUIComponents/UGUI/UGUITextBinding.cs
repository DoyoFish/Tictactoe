using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIBinding
{
    public class UGUITextBinding : TextBinding
    {
        public Text Text;

        void Awake()
        {
            OnValidate();
        }


        private void Reset()
        {
            OnValidate();
        }


        protected override void OnValidate()
        {
            base.OnValidate();
            if (!Text)
            {
                Text = GetComponent<Text>();
            }

            if (string.IsNullOrEmpty(Path))
            {
                if (!Format.Contains("$"))
                {
                    Debug.LogError(string.Format("Path Is Null And Format({0}) Dont Contain '$' \n{1}", Format,
                        GetDisplayHierarchy(transform)));
                }
                else
                {
                    Debug.LogWarning("Path Is Null Only Use For Localize \n" + GetDisplayHierarchy(transform));
                }
            }
        }

        protected override void OnValueChanged(string txt)
        {
            if(Text)
            {
                Text.text = txt;
            }
        }
    }

}