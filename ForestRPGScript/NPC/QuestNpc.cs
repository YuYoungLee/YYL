using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class QuestNpc : NPC, IInteractAble
{
    //private int miQuestKey;     //����Ʈ Ű
    [SerializeField] Transform mTransform;       //����Ʈ npc Ʈ������
    [SerializeField] private TextMeshPro mQuestStateText;       //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] private List<Transform> mTextTransform;    //NPC �ؽ�Ʈ ����Ʈ Ʈ������

    private Transform mTargetTr;
    private StringBuilder mStrBuilder;      //��Ʈ�� ����
    public delegate QuestManager QuestMgr();
    public QuestMgr GetQuestMgr;    //����Ʈ �޴���

    public override void Initialize()
    {
        GameManager gameMgr = GameManager.Instance;

        //��Ʈ�� ����
        if (mStrBuilder == null)
        {
            mStrBuilder = new StringBuilder();
        }

        //����Ʈ �޴���
        if (GetQuestMgr == null)
        {
            GetQuestMgr = gameMgr.GetQuestMgr;
        }

        //Ÿ���� �� ī�޶� Tr
        if(mTargetTr == null)
        {
            mTargetTr = gameMgr.GetCameraContorol().Transform;
        }

        SetName();
        //ResetData();
        //����Ʈ ������ ���´� �ε� o
        //����Ʈ NPC �ε� x

        //NPC ������ �ε�
        //SerializeField ���� ����Ʈ Ű �ʱⰪ�� �־��� ������
        //������ �ε�� �÷��̾ ����Ʈ ������ ������ �� �ʱ� ����Ʈ Ű���� �� �� ���� ����
        //
        //NPC �����Ϳ� �ʱ� ������ Ű���� ���� ��Ű��(������ �޴��� -> NPC Data)
        //�ʱ�ȭ �� ������ �޴��� > �ʱ� Ű���� ��������
    }

    public void Interaction()
    {
        GameManager gameMgr = GameManager.Instance;     //���� �޴���

        //GUI ����
        UIManager uiMgr = UIManager.Instance;
        uiMgr.GetGUI(EGUI.Inventory).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Equipment).ActiveGUI(false);
        uiMgr.GetGUI(EGUI.Skill).ActiveGUI(false);

        gameMgr.GetQuestMgr().AddCount(EQuestTargetType.InteractionNPC, miNPCKey, 1);      //��ȣ�ۿ� ī��Ʈ ����
        gameMgr.GetPlayer.SetLookAt(mTransform.position);     //�÷��̾ NPC �������� �ٶ󺸱�

        GetQuestMgr().Talk(miNPCKey);     //����Ʈ ��ȭ
    }   //��ȣ�ۿ�

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

        NPCState(EQuestState.None);     //�ƹ��͵� �ƴ� ���·� ����
    }

    public void LateUpdate()
    {
        //����Ʈ ó��
        for (int i = 0; i < mTextTransform.Count; ++i)
        {
            Vector3 targetDir = mTextTransform[i].position - mTargetTr.position;            //�ؽ�Ʈ ����
            targetDir.y = 0.0f;
            mTextTransform[i].rotation = Quaternion.LookRotation(targetDir).normalized;     //ȸ��
        }
    }
}
