using System.Collections.Generic;
using UnityEngine;


public class SpawnerManager : MonoBehaviour
{
    [SerializeField] ArtifactSpawner[] spawnerItem = null;         //������ ������
    [SerializeField] BossSpawner[] bossSpawners = null;            //���� ������
    [SerializeField] GameObject[] StartPos = null;                 //�÷��̾� ������ġ & npc ����Ʈ ������Ʈ

    [SerializeField] QuestObject[] questObjects = null;            //����Ʈ ������Ʈ
    [SerializeField] ShopObject[] shopObjects = null;              //���� ������Ʈ
    private List<int> suffleIndex = new List<int>();               //�ε��� ����

    public void SetSpawner(int count)
    {
        //������ ���� �ڽ� �ʱ�ȭ
        for (int i = 0; i < spawnerItem.Length; ++i)
        {
            spawnerItem[i].Initialize();
        }

        SuffleSpawner(spawnerItem.Length);
        ActiveSpawner(count);

        //���� ������ ����
        for (int i = 0; i < bossSpawners.Length; ++i)
        {
            bossSpawners[i].gameObject.SetActive(false);
        }
        if (bossSpawners.Length > 0) { bossSpawners[Random.Range(0, bossSpawners.Length)].SetActive(true); }

        //������ġ ����
        for (int i = 0; i < StartPos.Length; ++i)
        {
            StartPos[i].gameObject.SetActive(false);
        }

        int randomIdx = Random.Range(0, StartPos.Length);

        StartPos[randomIdx].SetActive(true);
        GameManager.Instance.GetQuestManager().SetQuestObject(questObjects[randomIdx]);     //����Ʈ ������Ʈ ����
        //�÷��̾ �ٶ󺸴� UI ����
        questObjects[randomIdx].StartCoroutine();
        shopObjects[randomIdx].StartCoroutine();
        GameManager.Instance.GetPlayer().gameObject.transform.position = StartPos[randomIdx].transform.position;    //������ ��ġ�� �̵�
        GameManager.Instance.GetCameraController.SetPosition(StartPos[randomIdx].transform.position);   //ī�޶� ��ġ �̵�
        GameManager.Instance.GetPlayer().SetYaw(StartPos[randomIdx].transform.rotation.eulerAngles);
        GameManager.Instance.GetPlayer().SetJumpStatus();   //���� ���·� ����
    }   //������ ����

    private void SuffleSpawner(int length)
    {
        if (length <= 0) return;                // ������ �ڽ��� ���� ��
        suffleIndex.Clear();

        for (int i = 0; i < length; ++i)
        {
            spawnerItem[i].SetActive(false);                //������ ��Ȱ��ȭ
            suffleIndex.Add(i);
        }   //�ε��� �߰�

        for(int i = 0; i < suffleIndex.Count; ++i)
        {
            System.Random ran = new System.Random();
            int randomValue = ran.Next(0, suffleIndex.Count);
            int temp = suffleIndex[i];
            suffleIndex[i] = suffleIndex[randomValue];
            suffleIndex[randomValue] = temp;
        }   //Random��� ����
    }   //����Ʈ�� ���� ���� ����

    private void ActiveSpawner(int count)
    {
        if (suffleIndex.Count <= 0) return;

        for (int i = 0; i < suffleIndex.Count*0.5f; ++i)
        {
            spawnerItem[suffleIndex[i]].SetActive(true);

            //ī��Ʈ ��ŭ ���� ����
            for(int j = 0; j < count; ++j)
            {
                GameManager.Instance.GetEnemyPool.SpawnMonster((Monster)Random.Range(0, 2), spawnerItem[suffleIndex[i]].gameObject.transform.position);
            }
        }
    }    //������ ������ Ȱ��ȭ

    public void ResetSpawner()
    {
        for (int i = 0; i < spawnerItem.Length; ++i)
        {
            spawnerItem[i].SetActive(false);
            spawnerItem[i].ResetObject();
        }

        for (int i = 0; i < bossSpawners.Length; ++i)
        {
            bossSpawners[i].SetActive(false);
        }   //������ �ʱ�ȭ

        for (int i = 0; i < StartPos.Length; ++i)
        {
            questObjects[i].StopCoroutine();
            shopObjects[i].StopCoroutine();
            StartPos[i].SetActive(false);
        }
    }
}
