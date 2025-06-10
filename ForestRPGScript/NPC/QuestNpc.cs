using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class QuestNpc : NPC, IInteractAble
{
    //private int miQuestKey;     //퀘스트 키
    [SerializeField] Transform mTransform;       //퀘스트 npc 트렌스폼
    [SerializeField] private TextMeshPro mQuestStateText;       //퀘스트 상태 텍스트
    [SerializeField] private List<Transform> mTextTransform;    //NPC 텍스트 리스트 트렌스폼

    private Transform mTargetTr;
    private StringBuilder mStrBuilder;      //스트링 빌더
    public delegate QuestManager QuestMgr();
    public QuestMgr GetQuestMgr;    //퀘스트 메니저

    public override void Initialize()
    {
        GameManager gameMgr = GameManager.Instance;

        //스트링 빌더
        if (mStrBuilder == null)
        {
            mStrBuilder = new StringBuilder();
        }

        //퀘스트 메니저
        if (GetQuestMgr == null)
        {
            GetQuestMgr = gameMgr.GetQuestMgr;
        }

        //타겟이 될 카메라 Tr
        if(mTargetTr == null)
        {
            mTargetTr = gameMgr.GetCameraContorol().Transform;
        }

        SetName();
        //ResetData();
        //퀘스트 데이터 상태는 로드 o
        //퀘스트 NPC 로드 x

        //NPC 데이터 로드
        //SerializeField 에서 퀘스트 키 초기값을 넣었기 때문에
        //데이터 로드시 플레이어가 퀘스트 수락을 안했을 때 초기 퀘스트 키값을 알 수 없는 상태
        //
        //NPC 데이터에 초기 참조할 키값을 포함 시키기(데이터 메니저 -> NPC Data)
        //초기화 시 데이터 메니저 > 초기 키값을 가져오기
    }

    public void Interaction()
    {
        GameManager gameMgr = GameManager.Instance;     //게임 메니저

        //GUI 끄기
        UIManager uiMgr = UIManager.Instance;
        uiMgr.GetGUI(EGUI.Inventory).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Equipment).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Skill).ActiveGUI(false);

        gameMgr.GetQuestMgr().AddCount(EQuestTargetType.InteractionNPC, miNPCKey, 1);      //상호작용 카운트 증가
        gameMgr.GetPlayer.SetLookAt(mTransform.position);     //플레이어가 NPC 방향으로 바라보기

        GetQuestMgr().Talk(miNPCKey);     //퀘스트 대화
    }   //상호작용

    public void NPCState(EQuestState eState)
    {
        mStrBuilder.Clear();
        switch (eState)
        {
            case EQuestState.Start:
                mStrBuilder.Append("?");
                mQuestStateText.text = mStrBuilder.ToString();
                mQuestStateText.color = Color.red;
                break;
            case EQuestState.Progress:
                mStrBuilder.Append("!");
                mQuestStateText.text = mStrBuilder.ToString();
                mQuestStateText.color = Color.white;
                break;
            case EQuestState.Clear:
                mStrBuilder.Append("!");
                mQuestStateText.text = mStrBuilder.ToString();
                mQuestStateText.color = Color.green;
                break;
            case EQuestState.None:
                mQuestStateText.text = mStrBuilder.ToString();
                break;
        }
    }

    public void NextQuest()
    {

        NPCState(EQuestState.None);     //아무것도 아닌 상태로 변경
    }

    public void LateUpdate()
    {
        //빌보트 처리
        for (int i = 0; i < mTextTransform.Count; ++i)
        {
            Vector3 targetDir = mTextTransform[i].position - mTargetTr.position;            //텍스트 방향
            targetDir.y = 0.0f;
            mTextTransform[i].rotation = Quaternion.LookRotation(targetDir).normalized;     //회전
        }
    }
}
