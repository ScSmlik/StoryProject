using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : SingletonMono<DialogManager>
{
    #region UI�ؼ�
    //Canvas����
    [SerializeField]
    private GameObject DialogCanvas;
    //������UI
    [SerializeField]
    private TMP_Text SpeakerTextUI;
    //��������UI
    [SerializeField]
    private TMP_Text DialogTextUI;
    //ͷ��UI
    [SerializeField]
    private Image HeadImageUI;
    //��ťUI���
    [SerializeField]
    private GameObject OptionPanel;
    [SerializeField]
    private GameObject[] OptionButtons;
    #endregion

    #region ��ѯ��Ϣ
    //���з�����
    [SerializeField]
    private SpeakerSo[] speakers;
    private Dictionary<SpeakerName, SpeakerSo> SpeakerDic = new Dictionary<SpeakerName, SpeakerSo>();
    #endregion

    #region ״̬��Ϣ
    //�Ƿ����ڲ���
    public bool isPlaying = false;
    //�Ƿ��ڿɴ�������
    public bool isInTextArea = false;
    //��ǰ���ŷ���
    private Conversation currentConversation;
    //��ǰ���ŶԻ�
    private DialogSo currentDialog;
    //��ǰ���Ž���
    private int step = 0;
    //�Ƿ��ڷ�֧ѡ��
    private bool isInBranch = false;
    //���ֻ�Ч���Ƿ�Ӧ�ý���
    private bool isEndWritting = true;
    #endregion

    #region Э��
    //������ʱ��
    public Coroutine endPlayTimer;
    //���ֻ�Ч��
    public Coroutine writtingEffectTimer;
    #endregion

    //��ʼ��
    private void Start()
    {
        //Ĭ�ϲ���ʾUI��
        DialogCanvas.SetActive(false);
        foreach (var b in OptionButtons)
            b.SetActive(false);
        //�����ֵ䣬���ڲ�ѯ
        foreach (var c in speakers)
            SpeakerDic.Add(c.speakerName, c);
        //�տ�ʼ����������,û�жԻ�
        isInTextArea = false;
        isInBranch = false;
        isPlaying = false;
        isEndWritting = true;

        currentDialog = null;
        currentConversation = null;
        step = 0;

    }

    //��һ������
    private void Update()
    {
        if (writtingEffectTimer == null)
        {
            if (endPlayTimer != null && isInTextArea == true && Input.GetKeyDown(KeyCode.Space))
            {
                step++;

                //��ת����һ��

                if (step >= currentDialog.conversations.Length)
                    EndPlayDialog();
                else
                    PlayOneDiolag();

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ֱ����ʾ����������
                isEndWritting = true;
            }
        }


    }

    //��ʼ�Ի�
    public void StartPlayDialog(DialogSo dialog)
    {
        isPlaying = true;
        //��ʼ���ŶԻ�
        step = 0;
        DialogCanvas.SetActive(true);
        currentDialog = dialog;
        PlayOneDiolag();
    }
    //����һ������
    private void PlayOneDiolag()
    {
        currentConversation = currentDialog.conversations[step];
        Conversation t = currentConversation;
        SpeakerTextUI.text = t.speaker.ToString();
        HeadImageUI.sprite = SpeakerDic[t.speaker].speakerHead;
        RefreshWrittingEffectTimer(t.text);
    }
    //�����Ի�
    public void EndPlayDialog()
    {
        //����Э��
        StopEndPlayTimer();
        StopWrittingEffectTimer();

        //���öԻ������Ϣ
        isInBranch = false;
        isPlaying = false;
        isEndWritting = true;
        step = 0;
        currentDialog = null;
        currentConversation = null;
        DialogCanvas.SetActive(false);
    }

    #region �����Ի�Э��
    //�����Ի�Э�̣�����6��δ������Ӧ��Ի���ʧ
    public IEnumerator EndPlayTimer()
    {
        yield return new WaitForSeconds(6);
        EndPlayDialog();
        StopEndPlayTimer();
    }
    //ֹͣ�����Ի�Э��
    public void StopEndPlayTimer()
    {
        if (endPlayTimer != null)
        {
            StopCoroutine(endPlayTimer);
            endPlayTimer = null;
        }
    }
    //ˢ�½����Ի�Э��
    private void RefreshEndPlayTimer()
    {
        StopEndPlayTimer();
        endPlayTimer = StartCoroutine(EndPlayTimer());
    }
    #endregion
    #region ���ֻ�Ч��Э��
    public IEnumerator WrittingEffectTimer(string text)
    {
        isEndWritting = false;

        DialogTextUI.text = "";
        yield return new WaitForSeconds(0.5f);
        foreach (char c in text.ToCharArray())
        {
            if (isEndWritting == true)
            {
                DialogTextUI.text = text;
                break;
            }
            DialogTextUI.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        if (currentConversation.options.Length != 0)
        {
            isInBranch = true;
            for (int i = 0; i < currentConversation.options.Length; i++)
            {
                OptionButtons[i].SetActive(true);

                OptionButtons[i].GetComponentInChildren<TMP_Text>().text = currentConversation.options[i].optionText;

                //���ǳ�ȥ���������ȴ�ѡ�������Ż���ʧ
                //������ʧЭ��
                StopEndPlayTimer();
            }
        }
        else
        {
            RefreshEndPlayTimer();
        }

        StopWrittingEffectTimer();

    }

    public void StopWrittingEffectTimer()
    {
        if (writtingEffectTimer != null)
        {
            StopCoroutine(writtingEffectTimer);
            writtingEffectTimer = null;
        }
    }
    private void RefreshWrittingEffectTimer(string text)
    {
        StopWrittingEffectTimer();
        writtingEffectTimer = StartCoroutine(WrittingEffectTimer(text));
    }
    #endregion


    //��֧ѡ��
    public void Choose(int x)
    {
        //TODO:ִ�з�֧���¼�

        //��ʼ�µĶԻ�
        isInBranch = false;
        foreach (var b in OptionButtons)
            b.SetActive(false);
        StartPlayDialog(currentConversation.options[x].optionDialog);

    }


}
