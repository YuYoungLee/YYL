using System.Collections.Generic;
using UnityEngine;


public class SpawnerManager : MonoBehaviour
{
    [SerializeField] ArtifactSpawner[] spawnerItem = null;         //아이템 스포너
    [SerializeField] BossSpawner[] bossSpawners = null;            //보스 스포너
    [SerializeField] GameObject[] StartPos = null;                 //플레이어 시작위치 & npc 퀘스트 오브젝트

    [SerializeField] QuestObject[] questObjects = null;            //퀘스트 오브젝트
    [SerializeField] ShopObject[] shopObjects = null;              //상점 오브젝트
    private List<int> suffleIndex = new List<int>();               //인덱스 섞기

    public void SetSpawner(int count)
    {
        //아이템 생성 박스 초기화
        for (int i = 0; i < spawnerItem.Length; ++i)
        {
            spawnerItem[i].Initialize();
        }

        SuffleSpawner(spawnerItem.Length);
        ActiveSpawner(count);

        //보스 스포너 설정
        for (int i = 0; i < bossSpawners.Length; ++i)
        {
            bossSpawners[i].gameObject.SetActive(false);
        }
        if (bossSpawners.Length > 0) { bossSpawners[Random.Range(0, bossSpawners.Length)].SetActive(true); }

        //시작위치 설정
        for (int i = 0; i < StartPos.Length; ++i)
        {
            StartPos[i].gameObject.SetActive(false);
        }

        int randomIdx = Random.Range(0, StartPos.Length);

        StartPos[randomIdx].SetActive(true);
        GameManager.Instance.GetQuestManager().SetQuestObject(questObjects[randomIdx]);     //퀘스트 오브젝트 설정
        //플레이어를 바라보는 UI 실행
        questObjects[randomIdx].StartCoroutine();
        shopObjects[randomIdx].StartCoroutine();
        GameManager.Instance.GetPlayer().gameObject.transform.position = StartPos[randomIdx].transform.position;    //설정된 위치로 이동
        GameManager.Instance.GetCameraController.SetPosition(StartPos[randomIdx].transform.position);   //카메라 위치 이동
        GameManager.Instance.GetPlayer().SetYaw(StartPos[randomIdx].transform.rotation.eulerAngles);
        GameManager.Instance.GetPlayer().SetJumpStatus();   //점프 상태로 설정
    }   //스포너 설정

    private void SuffleSpawner(int length)
    {
        if (length <= 0) return;                // 아이템 박스가 없을 때
        suffleIndex.Clear();

        for (int i = 0; i < length; ++i)
        {
            spawnerItem[i].SetActive(false);                //스포너 비활성화
            suffleIndex.Add(i);
        }   //인덱스 추가

        for(int i = 0; i < suffleIndex.Count; ++i)
        {
            System.Random ran = new System.Random();
            int randomValue = ran.Next(0, suffleIndex.Count);
            int temp = suffleIndex[i];
            suffleIndex[i] = suffleIndex[randomValue];
            suffleIndex[randomValue] = temp;
        }   //Random사용 섞기
    }   //리스트에 따로 빼서 셔플

    private void ActiveSpawner(int count)
    {
        if (suffleIndex.Count <= 0) return;

        for (int i = 0; i < suffleIndex.Count*0.5f; ++i)
        {
            spawnerItem[suffleIndex[i]].SetActive(true);

            //카운트 만큼 몬스터 생성
            for(int j = 0; j < count; ++j)
            {
                GameManager.Instance.GetEnemyPool.SpawnMonster((Monster)Random.Range(0, 2), spawnerItem[suffleIndex[i]].gameObject.transform.position);
            }
        }
    }    //랜덤한 스포너 활성화

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
        }   //스포너 초기화

        for (int i = 0; i < StartPos.Length; ++i)
        {
            questObjects[i].StopCoroutine();
            shopObjects[i].StopCoroutine();
            StartPos[i].SetActive(false);
        }
    }
}
