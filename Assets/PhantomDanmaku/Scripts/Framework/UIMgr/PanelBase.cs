using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour
{
    private Dictionary<string,List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    protected virtual void Awake()
    {
        FindControl<Button>();
        FindControl<Text>();
        FindControl<Slider>();
        FindControl<Toggle>();
        FindControl<Image>();
        FindControl<ScrollRect>();
        FindControl<InputField>();
    }
    //Button的监听
    protected virtual void OnClick(string buttonName)
    {

    }
    //Toggle的监听
    protected virtual void OnValueChanged(string toggleName,bool value)
    {

    }
    //Slider的监听
    protected virtual void OnValueChanged(string sliderName,float value)
    {

    }
    //InputField的监听
    protected virtual void OnValueChanged(string inputName,string value)
    {

    }
    
    //找到所有控件，存到字典里，键是对象名,值是控件们
    private void FindControl<T>() where T : UIBehaviour
    {
        //找到所有T类型的控件,比如Button
        T[] controls = transform.GetComponentsInChildren<T>();
        foreach(var control in controls)
        {
            string objName = control.gameObject.name;
            //如果字典里没有键，就新增一个
            if(!controlDic.ContainsKey(objName))
            {
                controlDic.Add(objName,new List<UIBehaviour>(){ control });
            }
            //如果字典里有键，就直接加入List里面
            else
            {
                controlDic[objName].Add(control);
            }
            //为某些控件添加事件监听
            if(control is Button)
            {
                (control as Button).onClick.AddListener(()=>
                {
                    OnClick(objName);
                });
            }
            else if(control is Toggle)
            {
                (control as Toggle).onValueChanged.AddListener((value)=>
                {
                    OnValueChanged(objName,value);
                });
            }
            else if(control is Slider)
            {
                (control as Slider).onValueChanged.AddListener((value)=>
                {
                    OnValueChanged(objName,value);
                });
            }
            else if(control is InputField)
            {
                (control as InputField).onValueChanged.AddListener((value)=>
                {
                    OnValueChanged(objName,value);
                });
            }
        }
    }

    public T GetControl<T>(string name) where T : UIBehaviour
    {
        if(controlDic.ContainsKey(name))
        {
            foreach(var item in controlDic[name])
            {
                if(item is T)
                {
                    return item as T;
                }
            }
        }
        return null;
    }

    public virtual void Show()
    {
    }
    public virtual void Hide()
    {
    }

}
