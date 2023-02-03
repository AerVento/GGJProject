using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Base class for all UI panels.
/// </summary>
public class BasePanel : MonoBehaviour
{

    protected Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChildrenControls<Button>();
        FindChildrenControls<Image>();
        FindChildrenControls<Slider>();
        FindChildrenControls<TextMeshProUGUI>();
        FindChildrenControls<Dropdown>();
        FindChildrenControls<InputField>();
    }

    /// <summary>
    /// Find all UI behaviours with specified UI type.
    /// </summary>
    /// <typeparam name="T">The UI type to be found.</typeparam>
    protected void FindChildrenControls<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        foreach (T control in controls)
        {
            string objName = control.gameObject.name;

            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(control);
            else
            {
                controlDic.Add(objName, new List<UIBehaviour>());
                controlDic[objName].Add(control);
            }
        }
    }

    /// <summary>
    /// Search all children of panel and find a UI component on the GameObject with the given name.
    /// </summary>
    /// <typeparam name="T">The type of UI component.</typeparam>
    /// <param name="controlName">The name of GameObject.</param>
    /// <returns>The UI component.</returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            foreach (UIBehaviour behaviour in controlDic[controlName])
            {
                if (behaviour is T)
                    return behaviour as T;
            }
        }

        return null;
    }

    public void AddButtonListener(string ButtonName, UnityAction call)
    {
        GetControl<Button>(ButtonName).onClick.AddListener(call);
    }

    public void AddSliderListener(string sliderName, UnityAction<float> call)
    {
        GetControl<Slider>(sliderName).onValueChanged.AddListener(call);
    }

    #region Show
    protected virtual void BeforeShow()
    {

    }
    protected virtual void AfterShow()
    {

    }

    public void Show()
    {
        BeforeShow();
        gameObject.SetActive(true);
        AfterShow();
    }
    #endregion

    #region Hide
    protected virtual void BeforeHide()
    {

    }
    protected virtual void AfterHide()
    {

    }
    public void Hide()
    {
        BeforeHide();
        gameObject.SetActive(false);
        AfterHide();
    }
    #endregion
}
