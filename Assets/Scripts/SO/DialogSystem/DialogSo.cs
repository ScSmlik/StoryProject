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
    //�ı�
    [TextArea]
    public string text;
    //������
    public SpeakerName speaker;
    [SerializeField]
    //��֧ѡ��
    public DialogOption[] options;
}

[Serializable]
public class DialogOption
{
    //��ť��ʾ�ı�
    public string optionText;
    //��ť�Ի�
    public DialogSo optionDialog;
    //TODO:��ť�����¼� 
}

