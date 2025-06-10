using UnityEngine;

public class ShopNpc : NPC, IInteractAble
{
    [SerializeField] private Transform mNPCNameTr;      //NPC이름 tr
    private Transform mTargetTr;    //텍스트 타겟의 tr

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

        //UI 창 끄기
        uiMgr.GetGUI(EGUI.Inventory).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Equipment).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Skill).ActiveGUI(false);

        gameMgr.GetQuestMgr().AddCount(EQuestTargetType.InteractionNPC, miNPCKey, 1);    //상호작용
        int iShopTableKey = gameMgr.GetDataMgr().GetNPCData(miNPCKey).InitKey;    //상점키 가져오기
        (UIManager.Instance.GetGUI(EGUI.Shop) as ShopPanel).ShopOpen(iShopTableKey);    //상점 열기
    }   //상호작용

    public void LateUpdate()
    {
        //빌보트 처리
        Vector3 targetDir = mNPCNameTr.position - mTargetTr.position;      //텍스트 방향
        targetDir.y = 0.0f;
        mNPCNameTr.rotation = Quaternion.LookRotation(targetDir).normalized;//회전
    }
}