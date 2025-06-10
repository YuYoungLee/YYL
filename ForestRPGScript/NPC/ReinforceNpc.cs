using UnityEngine;

public class ReinforceNpc : NPC, IInteractAble
{
    [SerializeField] private Transform mNPCNameTr;      //NPC�̸� tr
    private Transform mTargetTr;    //�ؽ�Ʈ Ÿ���� tr

    public override void Initialize()
    {
        //Ÿ�� tr
        if(mTargetTr == null)
        {
            mTargetTr = GameManager.Instance.GetCameraContorol().Transform;     //ī�޶� tr
        }

        SetName();
    }

    public void Interaction()
    {
        GameManager.Instance.GetQuestMgr().AddCount(EQuestTargetType.InteractionNPC, miNPCKey, 1);    //��ȣ�ۿ�
        UIManager uiMgr = UIManager.Instance;
        uiMgr.GetGUI(EGUI.Equipment).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Skill).ActiveGUI(false);

        UIManager.Instance.GetGUI(EGUI.ItemReinforce).ActiveGUI(true);    //��� �ѱ�
    }   //��ȣ�ۿ�

    public void LateUpdate()
    {
        //����Ʈ ó��
        Vector3 targetDir = mNPCNameTr.position - mTargetTr.position;      //�ؽ�Ʈ ����
        targetDir.y = 0.0f;
        mNPCNameTr.rotation = Quaternion.LookRotation(targetDir).normalized;//ȸ��
    }
}
