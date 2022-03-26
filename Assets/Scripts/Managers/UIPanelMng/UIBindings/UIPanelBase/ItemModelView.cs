using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModelView : ModelView
{
    public ItemContext ItemContext;

    public override object Instance
    {
        get
        {
            return ItemContext;
        }
    }

    protected override void OnValidate()
    {
        if (!ItemContext)
        {
            ItemContext = this.GetComponent<ItemContext>();
        }
    }
}