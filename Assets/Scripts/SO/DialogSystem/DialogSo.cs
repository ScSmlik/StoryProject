using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class DialogSo : ScriptableObject
{
    [SerializeField]
    public Conversation[] conversations;
}

[Serializable]
public class Conversation
{
    //文本
    [TextArea]
    public string text;
    //发言人
    public SpeakerName speaker;
    [SerializeField]
    //分支选择
    public DialogOption[] options;
}

[Serializable]
public class DialogOption
{
    //按钮显示文本
    public string optionText;
    //按钮对话
    public DialogSo optionDialog;
    //TODO:按钮触发事件 
}

