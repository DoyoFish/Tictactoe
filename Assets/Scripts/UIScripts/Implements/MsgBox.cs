using System;
using UIBinding;

public class MsgBox : UIPanelBase
{
    public override UICommon UIPanelName => UICommon.MsgBox;

    public override PanelType PanelType => PanelType.TipsPanel;

    public override UIStackLevel StackLevel => UIStackLevel.TopMost;

    public string Title
    {
        get { return _propertyTitle.GetValue(); }
        set { _propertyTitle.SetValue(value); }
    }
    private readonly Property<string> _propertyTitle = new Property<string>();

    public string Content
    {
        get { return _propertyContent.GetValue(); }
        set { _propertyContent.SetValue(value); }
    }
    private readonly Property<string> _propertyContent = new Property<string>();

    public bool CloseVisible
    {
        get { return _propertyCloseVisible.GetValue(); }
        set { _propertyCloseVisible.SetValue(value); }
    }
    private readonly Property<bool> _propertyCloseVisible = new Property<bool>();

    public bool ConfirmVisible
    {
        get { return _propertyConfirmVisible.GetValue(); }
        set { _propertyConfirmVisible.SetValue(value); }
    }
    private readonly Property<bool> _propertyConfirmVisible = new Property<bool>();

    public bool CancelVisible
    {
        get { return _propertyCancelVisible.GetValue(); }
        set { _propertyCancelVisible.SetValue(value); }
    }
    private readonly Property<bool> _propertyCancelVisible = new Property<bool>();

    private Action _confirm;
    private Action _cancel;
    private Action _close;


    public override void OnInit(object caller)
    {
    }

    public override void OnShow(object caller)
    {
        _confirm = Msg["confirm"].As<Action>();
        _close = Msg["close"].As<Action>();
        _cancel = Msg["cancel"].As<Action>();

        Title = Msg["title"].ToString();
        Content = Msg["content"].ToString();

        ConfirmVisible = _confirm != null;
        CloseVisible = _close != null;
        CancelVisible = _cancel != null;
    }

    public void OnTapConfirm()
    {
        if (_confirm != null)
        {
            _confirm.Invoke();
            CloseSelf();
        }
        else
        {
            CloseSelf();
        }
    }

    public void OnTapCancel()
    {
        if (_cancel != null)
        {
            _cancel.Invoke();
            CloseSelf();
        }
        else
        {
            CloseSelf();
        }
    }

    public void OnTapClose()
    {
        if (_close != null)
        {
            _close.Invoke();
            CloseSelf();
        }
        else
        {
            CloseSelf();
        }
    }

    private void CloseSelf()
    {
        UIPanelMng.Instance.ClosePanel(this, UIPanelName, false, true);
    }

    public override void OnClose(object caller)
    {
    }
}
