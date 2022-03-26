using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIBinding
{

    [RequireComponent(typeof(Selectable))]
    public class UGUIInteractiveBinding : BooleanBinding
    {
        private Selectable _selectable;

        public override void Start()
        {
            if (!_selectable)
            {
                _selectable = this.GetComponent<Selectable>();
            }
            base.Start();
        }

        protected override bool SetActive { set { _selectable.interactable = value; } }
    }

}