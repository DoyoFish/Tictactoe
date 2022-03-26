using Doyo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UIPanelBase : MonoBehaviour
{
    public Message Msg { get; set; }

    public abstract UICommon UIPanelName { get; }

    public virtual UIStackLevel StackLevel { get { return UIStackLevel.BottomMost; } }

    public abstract PanelType PanelType { get; }

    public abstract void OnInit(object caller);

    public abstract void OnShow(object caller);

    public virtual void OnRefresh(object caller)
    {

    }

    public virtual void OnPause()
    {

    }

    public virtual void OnContinue()
    {

    }

    public virtual void OnEscapeCommand(object caller, EscapeCommandEventArgs e) { }

    public abstract void OnClose(object caller);

    public T GetUI<T>(string uiName) where T : Component
    {
        return transform.GetController<T>(uiName);
    }

    public Transform GetUI(string uiName)
    {
        return transform.GetController(uiName);
    }

    public virtual void OnDestroy()
    {

    }
}

