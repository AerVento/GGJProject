using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Manager of all UI panels.
/// </summary>
public class UIManager:Singleton<UIManager>
{
    const string UI_PATH = "Prefabs/UI/";

    /// <summary>
    /// The layer of UI elements.
    /// </summary>
    public enum UILayer
    {
        /// <summary>
        /// Top layer.
        /// </summary>
        Top,
        /// <summary>
        /// Middle layer.
        /// </summary>
        Mid,
        /// <summary>
        /// Bottom layer.
        /// </summary>
        Bot,
        /// <summary>
        /// System layer.
        /// </summary>
        System
    }

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform top;  
    private Transform mid; 
    private Transform bot;
    private Transform system;
    /// <summary>
    /// The main canvas of UI.
    /// </summary>
    public Canvas Canvas { get; private set; }

    /// <summary>
    /// Initialization of UI manager.
    /// </summary>
    public UIManager()
    {
        GameObject obj = null;
        obj = ResourceManager.Instance.Load<GameObject>(UI_PATH + "Canvas");
        GameObject canvas = GameObject.Instantiate(obj);
        Canvas = canvas.GetComponent<Canvas>();
        GameObject.DontDestroyOnLoad(canvas);

        top = canvas.transform.Find("Top");
        mid = canvas.transform.Find("Mid");
        bot = canvas.transform.Find("Bot");
        system = canvas.transform.Find("System");

        obj = ResourceManager.Instance.Load<GameObject>(UI_PATH + "EventSystem");
        GameObject eventSystem = GameObject.Instantiate(obj);
        GameObject.DontDestroyOnLoad(eventSystem);
    }

    /// <summary>
    /// Get transform of the UI layer.
    /// </summary>
    /// <param name="layer">The UI layer.</param>
    /// <returns>The transform of the UILayer.</returns>
    public Transform GetUILayer(UILayer layer)
    {
        switch (layer)
        {
            case UILayer.Top:
                return top;

            case UILayer.Mid:
                return mid;

            case UILayer.Bot:
                return bot;

            case UILayer.System:
                return system;
        }
        return null;
    }

    #region PanelOperate
    
    /// <summary>
    /// Show panel with the type of panel.
    /// </summary>
    /// <typeparam name="T">Panel type.</typeparam>
    /// <param name="panelName">The name of panel instance.</param>
    /// <param name="layer">The UI layer to put the panel into.</param>
    /// <param name="callback">Function called before the panel was shown.</param>
    public void ShowPanel<T>(string panelName, UILayer layer = UILayer.Mid, UnityAction<T> callback = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Show();
            panelDic[panelName].transform.SetParent(GetUILayer(layer));
            if (callback != null)
                callback(panelDic[panelName] as T);

            return;
        }

        ResourceManager.Instance.LoadAsync<GameObject>(UI_PATH + "Panels/" + panelName, (obj) =>
        {
            GameObject instantiated = GameObject.Instantiate(obj, GetUILayer(layer));

            //Get the component.
            T panel = instantiated.GetComponent<T>();

            panel.Show();

            // Callbacks.
            callback?.Invoke(panel);


            //Save the panel.
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// Hide the panel.
    /// </summary>
    /// <param name="panelName">The name of panel instance.</param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide();
        }
    }
    /// <summary>
    /// Hide all panels.
    /// </summary>
    public void HideAllPanel()
    {
        foreach (var item in panelDic)
        {
            item.Value.Hide();
        }
    }
    /// <summary>
    /// Get a panel already shown.
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }
    #endregion

    /// <summary>
    /// Add a custom event listener to a UI component.
    /// </summary>
    /// <param name="control">The UI component to be added.</param>
    /// <param name="type">The event type.</param>
    /// <param name="call">The react call.</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> call)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(call);

        trigger.triggers.Add(entry);
    }

}
