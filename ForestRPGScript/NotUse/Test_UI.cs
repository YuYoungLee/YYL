using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UI : MonoBehaviour
{
    public List<GameObject> mUI;

    public void InputGUIA(InputAction.CallbackContext context)
    {
        Debug.Log(context.action.actionMap.name);
        ActiveSwitchObjectA(!mUI[0].activeSelf);
    }   //ÀÎÇ² ¾×¼Ç


    public void InputGUIB(InputAction.CallbackContext context)
    {
        Debug.Log(context.action.actionMap.name);
        ActiveSwitchObjectB(!mUI[1].activeSelf);
    }   //ÀÎÇ² ¾×¼Ç


    public void ActiveSwitchObjectA(bool bStatu)
    {
        Debug.Log("ActiveA : " + bStatu);
        mUI[0].SetActive(bStatu);
    }

    public void ActiveSwitchObjectB(bool bStatu)
    {
        Debug.Log("ActiveB : " + bStatu);
        mUI[1].SetActive(bStatu);
    }

    public int GetActiveGUICount()
    {
        int iActiveCount = 0;
        for(int i = 0; i < mUI.Count; ++i)
        {
            if (mUI[i].activeSelf)
            {
                ++iActiveCount;
            }
        }
        Debug.Log("ActiveCount : " + iActiveCount);
        return iActiveCount;
    }
}
