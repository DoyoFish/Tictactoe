using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIBinding
{

    public class ActiveBinding : BooleanBinding
    {
        
        protected override bool SetActive { set { this.gameObject.SetActive(value); } }
    }

}