using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 任务管理

    #endregion

    #region 对话管理
    //进入对话区域
    public void EnterTextArea()
    {
        DialogManager.Instance.isInTextArea = true;
    }
    //退出对话区域
    public void ExitTextArea()
    {
        DialogManager.Instance.isInTextArea = false;
        DialogManager.Instance.EndPlayDialog();
    }
    //播放一套对话
    public void PlayDialog(DialogSo t)
    {
        if (DialogManager.Instance.isInTextArea == false || DialogManager.Instance.isPlaying == true) return;
        DialogManager.Instance.StartPlayDialog(t);
    }
    #endregion
}
