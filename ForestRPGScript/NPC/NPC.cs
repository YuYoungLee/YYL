using TMPro;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    [SerializeField] protected int miNPCKey;      //npc 키값
    [SerializeField] private GameObject mNPCObject;     //npc Object
    [SerializeField] protected TextMeshPro mNameText;   //이름 text

    public int GetNPCKey => miNPCKey;
    public void SetActive(bool bStatu) => mNPCObject.SetActive(bStatu);
    public abstract void Initialize();

    protected void SetName()
    {
        mNameText.text = GameManager.Instance.GetDataMgr().GetNPCData(miNPCKey).Name;
    }   //NPC 이름 설정
}
