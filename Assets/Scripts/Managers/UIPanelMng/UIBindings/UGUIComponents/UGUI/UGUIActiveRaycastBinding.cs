using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIBinding
{

    [RequireComponent(typeof(Graphic))]
    public class UGUIActiveRaycastBinding : BooleanBinding
    {

        private Graphic _graphic;

        public override void Start()
        {
            if (!_graphic)
            {
                _graphic = this.GetComponent<Graphic>();
            }
            base.Start();
        }

        protected override bool SetActive { set { _graphic.raycastTarget = value; } }
    }

}