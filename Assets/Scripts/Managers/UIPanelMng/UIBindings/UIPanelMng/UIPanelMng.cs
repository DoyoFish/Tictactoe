
using Doyo.Assets;
using Doyo.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public partial class UIPanelMng : MonoSingleton<UIPanelMng>
{
    public static readonly Vector2 StandaloneRect = new Vector2(1080, 1920);

    public Canvas UICanvas { get { return _uiCanvas; } }
    private Canvas _uiCanvas;

    public Camera UICamera { get { return _uiCamera; } }
    private Camera _uiCamera;

    public RectTransform BottomMostStack { get { return _bottomMostStack; } }
    private RectTransform _bottomMostStack;

    public RectTransform LowerStack { get { return _lowerStack; } }
    private RectTransform _lowerStack;

    public RectTransform HigherStack { get { return _higherStack; } }
    private RectTransform _higherStack;

    public RectTransform TopMostStack { get { return _topMostStack; } }
    private RectTransform _topMostStack;

    private readonly Dictionary<UICommon, UIPanelBase> _appearedPanels = new Dictionary<UICommon, UIPanelBase>();
    private readonly Dictionary<UIStackLevel, RectTransform> _uiStacks = new Dictionary<UIStackLevel, RectTransform>();

    private readonly Stack<UICommon> _panelStack = new Stack<UICommon>();

    protected override void Construct()
    {
        FindUICamera();
        FindUICanvas();
        FindUIStacks();
        RegisterUIStacks();
    }

    private void FindUICamera()
    {
        _uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    private void FindUICanvas()
    {
        _uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
    }

    private void FindUIStacks()
    {
        _bottomMostStack = GameObject.Find("BottomMostStack").GetComponent<RectTransform>();
        _lowerStack = GameObject.Find("LowerStack").GetComponent<RectTransform>();
        _higherStack = GameObject.Find("HigherStack").GetComponent<RectTransform>();
        _topMostStack = GameObject.Find("TopMostStack").GetComponent<RectTransform>();
    }

    private void RegisterUIStacks()
    {
        _uiStacks.Add(UIStackLevel.BottomMost, _bottomMostStack);
        _uiStacks.Add(UIStackLevel.Lower, _lowerStack);
        _uiStacks.Add(UIStackLevel.Higher, _higherStack);
        _uiStacks.Add(UIStackLevel.TopMost, _topMostStack);
    }

    public T OpenPanel<T>(object caller, Message msg = null, bool closeLast = true) where T : UIPanelBase
    {
        if (!Enum.TryParse(typeof(T).Name, out UICommon panelName))
        {
            return null;
        }
        return OpenPanel(caller, panelName, msg, closeLast) as T;
    }

    public UIPanelBase OpenPanel(object caller, UICommon panelName, Message msg = null, bool closeLast = true)
    {
        UIPanelBase panelInstance;
        if (!_appearedPanels.ContainsKey(panelName))
        {
            string panelPath = panelName.ToString();
            panelInstance = GenerateUIPanelInstance(panelPath);
            if (panelInstance == null)
            {
                Debug.LogErrorFormat("panelName:({0}) is not present, path: {1}", panelName, panelPath);
            }

            InitUIPanelInstance(caller, panelInstance);
            _appearedPanels.Add(panelName, panelInstance);
        }
        panelInstance = _appearedPanels[panelName];
        if (closeLast && _panelStack.Count > 0)
        {
            CloseLast(caller, false, false);
        }
        SetCurrentPanel(panelInstance);
        ShowUIPanelInstance(caller, panelInstance, msg);
        return panelInstance;
    }

    public UIPanelBase OpenLast(object caller, Message msg = null)
    {
        var lastPanelName = _panelStack.Peek();
        Debug.LogFormat("打开上一次的界面: {0}", lastPanelName);
        return OpenPanel(caller, lastPanelName, msg, false);
    }

    private UIPanelBase GenerateUIPanelInstance(string path)
    {
        GameObject link = Instantiate(Loader.LoadPrefab(path));
        if (!link)
        {
            return null;
        }
        UIPanelBase panelInstance = link.GetComponent<UIPanelBase>();
        return panelInstance;
    }

    private void InitUIPanelInstance(object caller, UIPanelBase panelInstance)
    {
        SetStackOfPanelInstance(panelInstance);
        panelInstance.gameObject.SetActive(true);
        panelInstance.OnInit(caller);
    }

    private void SetStackOfPanelInstance(UIPanelBase panelInstance)
    {
        UIStackLevel level = panelInstance.StackLevel;
        panelInstance.transform.SetParent(_uiStacks[level], false);
    }

    private void ShowUIPanelInstance(object caller, UIPanelBase panelInstance, Message msg)
    {
        panelInstance.gameObject.SetActive(true);
        panelInstance.transform.SetAsLastSibling();
        panelInstance.Msg = msg;
        panelInstance.OnShow(caller);
    }

    private void SetCurrentPanel(UIPanelBase panelInstance)
    {
        if (_panelStack.Count > 0)
        {
            var lastPanelName = _panelStack.Peek();
            if (panelInstance.UIPanelName == lastPanelName)
            {
                return;
            }
        }
        _panelStack.Push(panelInstance.UIPanelName);
    }

    public T GetPanelByName<T>() where T : UIPanelBase
    {
        string panelName = typeof(T).Name;
        if (!Enum.TryParse(panelName, out UICommon panel))
        {
            return null;
        }
        return GetPanelByName(panel) as T;
    }

    public UIPanelBase GetPanelByName(UICommon panelName)
    {
        if (!_appearedPanels.ContainsKey(panelName))
        {
            return null;
        }
        return _appearedPanels[panelName];
    }

    /// <summary>
    /// 关闭指定的界面
    /// </summary>
    /// <param name="caller">发起者</param>
    /// <param name="panelName">界面名称枚举</param>
    /// <param name="openLast">是否打开最近打开过的界面</param>
    /// <param name="pop">是否从栈中移除</param>
    public void ClosePanel(object caller, UICommon panelName, bool openLast = true, bool pop = true) // 优化关闭逻辑, 要求能够自动打开上一次界面
    {
        if (!_appearedPanels.ContainsKey(panelName))
        {
            return;
        }
        UIPanelBase panelInstance = _appearedPanels[panelName];
        panelInstance.OnClose(caller);
        panelInstance.gameObject.SetActive(false);
        if (pop && _panelStack.Count > 0)
        {
            Debug.LogFormat("被移出栈的Panel: {0}", _panelStack.Pop());
        }
        if (openLast)
        {
            OpenLast(caller);
        }
    }

    /// <summary>
    /// 关闭上一个打开的界面
    /// </summary>
    /// <param name="caller">发起者</param>
    /// <param name="openLast">是否打开最近打开过的界面</param>
    /// <param name="pop">是否从栈中移除</param>
    public void CloseLast(object caller, bool openLast = true, bool pop = true)
    {
        UICommon lastPanelName;
        lastPanelName = _panelStack.Peek();
        ClosePanel(caller, lastPanelName, openLast, pop);
    }

    public bool IsPanelOpened(UICommon panelName)
    {
        if (!_appearedPanels.ContainsKey(panelName))
        {
            return false;
        }
        return _appearedPanels[panelName].gameObject.activeSelf;
    }

    public void OpenPopMsg(object caller, string title, string content, Action end = null)
    {
        OpenPopMsg(caller, title, content, 2, end);
    }

    public void OpenPopMsg(object caller, string title, string content, float seconds, Action end = null)
    {
        Message msg = new Message();
        msg["title"] = title;
        msg["content"] = content;
        msg["endAction"] = new Message(end);
        UIPanelBase popMsg = OpenPanel(caller, UICommon.PopMsgView, msg, false);

        StartCoroutine(DelayClosePanel(seconds, UICommon.PopMsgView, false));
    }

    private IEnumerator DelayClosePanel(float seconds, UICommon panelName, bool pop)
    {
        yield return Yielders.GetWaitForSeconds(seconds);
        ClosePanel(this, panelName, pop);
    }

    public void OpenMsgBox(object caller, string title, string content, Action confirm = null, Action cancel = null, Action close = null)
    {
        Message msg = new Message();
        msg["title"] = title;
        msg["content"] = content;
        msg["confirm"] = new Message(confirm);
        msg["cancel"] = new Message(cancel);
        msg["close"] = new Message(close);

        OpenPanel(caller, UICommon.MsgBox, msg, false);
    }

    public void BackToMain()
    {
        foreach (RectTransform container in _uiStacks.Values)
        {
            for (int i = container.childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(container.GetChild(i).gameObject);
            }
        }
        _appearedPanels.Clear();
        _panelStack.Clear();
        OpenPanel(null, UICommon.LoginPanel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var currentPanel = _panelStack.Peek();
            var panel = GetPanelByName(currentPanel);
            if (panel)
            {
                panel.OnEscapeCommand(this, new EscapeCommandEventArgs());
            }
        }
    }

    protected override void Release()
    {

    }
}

