using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelModelView : ModelView
{
    public override object Instance
    {
        get
        {
            if (Panel)
            {
                return Panel;
            }
            return null;
        }
    }

    public UIPanelBase Panel;

    protected override void OnValidate()
    {
        if (!Panel)
        {
            Panel = this.GetComponent<UIPanelBase>();
        }

    }
}