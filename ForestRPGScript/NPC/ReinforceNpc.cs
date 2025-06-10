using UnityEngine;

public class ReinforceNpc : NPC, IInteractAble
{
    [SerializeField] private Transform mNPCNameTr;      //NPC이름 tr
    private Transform mTargetTr;    //텍스트 타겟의 tr

    public override void Initialize()
    {
        //타겟 tr
        if(mTargetTr == null)
        {
            mTargetTr = GameManager.Instance.GetCameraContorol().Transform;     //카메라 tr
        }

        SetName();
    }

    public void Interaction()
    {
        GameManager.Instance.GetQuestMgr().AddCount(EQuestTargetType.InteractionNPC, miNPCKey, 1);    //상호작용
        UIManager uiMgr = UIManager.Instance;
        uiMgr.GetGUI(EGUI.Equipment).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Skill).ActiveGUI(false);

        UIManager.Instance.GetGUI(EGUI.ItemReinforce).ActiveGUI(true);    //장비 켜기
    }   //상호작용

    public void LateUpdate()
    {
        //빌보트 처리
        Vector3 targetDir = mNPCNameTr.position - mTargetTr.position;      //텍스트 방향
        targetDir.y = 0.0f;
        mNPCNameTr.rotation = Quaternion.LookRotation(targetDir).normalized;//회전
    }
}
