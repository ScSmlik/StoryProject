using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : SingletonMono<DialogManager>
{
    #region UI控件
    //Canvas盒子
    [SerializeField]
    private GameObject DialogCanvas;
    //发言人UI
    [SerializeField]
    private TMP_Text SpeakerTextUI;
    //发言内容UI
    [SerializeField]
    private TMP_Text DialogTextUI;
    //头像UI
    [SerializeField]
    private Image HeadImageUI;
    //按钮UI组件
    [SerializeField]
    private GameObject OptionPanel;
    [SerializeField]
    private GameObject[] OptionButtons;
    #endregion

    #region 查询信息
    //所有发言人
    [SerializeField]
    private SpeakerSo[] speakers;
    private Dictionary<SpeakerName, SpeakerSo> SpeakerDic = new Dictionary<SpeakerName, SpeakerSo>();
    #endregion

    #region 状态信息
    //是否正在播放
    public bool isPlaying = false;
    //是否在可触发区域
    public bool isInTextArea = false;
    //当前播放发言
    private Conversation currentConversation;
    //当前播放对话
    private DialogSo currentDialog;
    //当前播放进度
    private int step = 0;
    //是否处于分支选项
    private bool isInBranch = false;
    //打字机效果是否应该结束
    private bool isEndWritting = true;
    #endregion

    #region 协程
    //结束计时器
    public Coroutine endPlayTimer;
    //打字机效果
    public Coroutine writtingEffectTimer;
    #endregion

    //初始化
    private void Start()
    {
        //默认不显示UI组
        DialogCanvas.SetActive(false);
        foreach (var b in OptionButtons)
            b.SetActive(false);
        //加入字典，便于查询
        foreach (var c in speakers)
            SpeakerDic.Add(c.speakerName, c);
        //刚开始不在区域内,没有对话
        isInTextArea = false;
        isInBranch = false;
        isPlaying = false;
        isEndWritting = true;

        currentDialog = null;
        currentConversation = null;
        step = 0;

    }

    //下一条发言
    private void Update()
    {
        if (writtingEffectTimer == null)
        {
            if (endPlayTimer != null && isInTextArea == true && Input.GetKeyDown(KeyCode.Space))
            {
                step++;

                //跳转到下一句

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
                //直接显示完所有文字
                isEndWritting = true;
            }
        }


    }

    //开始对话
    public void StartPlayDialog(DialogSo dialog)
    {
        isPlaying = true;
        //开始播放对话
        step = 0;
        DialogCanvas.SetActive(true);
        currentDialog = dialog;
        PlayOneDiolag();
    }
    //播放一条发言
    private void PlayOneDiolag()
    {
        currentConversation = currentDialog.conversations[step];
        Conversation t = currentConversation;
        SpeakerTextUI.text = t.speaker.ToString();
        HeadImageUI.sprite = SpeakerDic[t.speaker].speakerHead;
        RefreshWrittingEffectTimer(t.text);
    }
    //结束对话
    public void EndPlayDialog()
    {
        //结束协程
        StopEndPlayTimer();
        StopWrittingEffectTimer();

        //重置对话相关信息
        isInBranch = false;
        isPlaying = false;
        isEndWritting = true;
        step = 0;
        currentDialog = null;
        currentConversation = null;
        DialogCanvas.SetActive(false);
    }

    #region 结束对话协程
    //结束对话协程，超过6秒未作出反应则对话消失
    public IEnumerator EndPlayTimer()
    {
        yield return new WaitForSeconds(6);
        EndPlayDialog();
        StopEndPlayTimer();
    }
    //停止结束对话协程
    public void StopEndPlayTimer()
    {
        if (endPlayTimer != null)
        {
            StopCoroutine(endPlayTimer);
            endPlayTimer = null;
        }
    }
    //刷新结束对话协程
    private void RefreshEndPlayTimer()
    {
        StopEndPlayTimer();
        endPlayTimer = StartCoroutine(EndPlayTimer());
    }
    #endregion
    #region 打字机效果协程
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

                //除非出去，否则必须等待选择结束后才会消失
                //结束消失协程
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


    //分支选择
    public void Choose(int x)
    {
        //TODO:执行分支的事件

        //开始新的对话
        isInBranch = false;
        foreach (var b in OptionButtons)
            b.SetActive(false);
        StartPlayDialog(currentConversation.options[x].optionDialog);

    }


}
