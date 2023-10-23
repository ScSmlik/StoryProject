using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region �������

    #endregion

    #region �Ի�����
    //����Ի�����
    public void EnterTextArea()
    {
        DialogManager.Instance.isInTextArea = true;
    }
    //�˳��Ի�����
    public void ExitTextArea()
    {
        DialogManager.Instance.isInTextArea = false;
        DialogManager.Instance.EndPlayDialog();
    }
    //����һ�׶Ի�
    public void PlayDialog(DialogSo t)
    {
        if (DialogManager.Instance.isInTextArea == false || DialogManager.Instance.isPlaying == true) return;
        DialogManager.Instance.StartPlayDialog(t);
    }
    #endregion
}
