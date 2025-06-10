using UnityEngine;

public class ShopNpc : NPC, IInteractAble
{
    [SerializeField] private Transform mNPCNameTr;      //NPC�̸� tr
    private Transform mTargetTr;    //�ؽ�Ʈ Ÿ���� tr

    public override void Initialize()
    {
        if(mTargetTr == null)
        {
            mTargetTr = GameManager.Instance.GetCameraContorol().Transform;
        }

        SetName();
    }

    public void Interaction()
    {
        GameManager gameMgr = GameManager.Instance;
        UIManager uiMgr = UIManager.Instance;

        //UI â ����
        uiMgr.GetGUI(EGUI.Inventory).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Equipment).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Skill).ActiveGUI(false);

        gameMgr.GetQuestMgr().AddCount(EQuestTargetType.InteractionNPC, miNPCKey, 1);    //��ȣ�ۿ�
        int iShopTableKey = gameMgr.GetDataMgr().GetNPCData(miNPCKey).InitKey;    //����Ű ��������
        (UIManager.Instance.GetGUI(EGUI.Shop) as ShopPanel).ShopOpen(iShopTableKey);    //���� ����
    }   //��ȣ�ۿ�

    public void LateUpdate()
    {
        //����Ʈ ó��
        Vector3 targetDir = mNPCNameTr.position - mTargetTr.position;      //�ؽ�Ʈ ����
        targetDir.y = 0.0f;
        mNPCNameTr.rotation = Quaternion.LookRotation(targetDir).normalized;//ȸ��
    }
}