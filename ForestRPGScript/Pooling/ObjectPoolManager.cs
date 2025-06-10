using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance
/// https://thepathfinder.co.kr/entry/TIL-Generic-20220706
/// https://coderzero.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-C-%EA%B0%95%EC%A2%8C-15-%EC%A0%9C%EB%84%A4%EB%A6%AD-Generics
/// </summary>
public class ObjectPoolManager<T> where T : PooledObject
{
    private Dictionary<int, ObjectPool<T>> objectPool;      //오브젝트 풀
    private List<T> mActiveObjects;                         //활성화 된 오브젝트 관리
    private Transform pooledParent;                         //오브젝트 풀 부모의 위치

    public T GetObject(int iKey) => objectPool[iKey].Spawn();   //유닛 생성

    public void Initialize()
    {
        //리소스 추가
        pooledParent = GameObject.Find("PooledObject").transform;  //오브젝트 찾기

        if (mActiveObjects == null) 
        {
            mActiveObjects =  new List<T>(); 
        }

        //풀이 비어 있을 경우
        if (objectPool == null)
        {
            string[] data = CSVReader.Instance.LoadCSV("CSVData/ObjResourceData");     //리소스 관련 데이터, pooling
            objectPool = new Dictionary<int, ObjectPool<T>>();
            for (int i = 1, iKey = 0; i < data.Length; ++i)
            {
                string[] sliceData = CSVReader.Instance.GetLineSlice(data[i]);     //데이터 슬라이스
                iKey = int.Parse(sliceData[0]);     //키값

                //풀 비어있으면 추가
                if(!objectPool.ContainsKey(iKey))
                {
                    objectPool[iKey] = new ObjectPool<T>();      //풀 생성
                    objectPool[iKey].Initialize(sliceData[1], pooledParent, this, int.Parse(sliceData[2]));      //초기화
                }
            }
        }
    }


    /// <summary>
    /// iKey : 몬스터 키값
    /// iCount : 소환할 개수
    /// centerPos : 스폰 지점
    /// </summary>
    public void SpawnEnemy(int iKey, int iCount, Vector3 centerPos)
    {
        //키에 포함되지 않는다면 리턴
        if (!objectPool.ContainsKey(iKey))
        {
            return;
        }

        //iCount 만큼 유닛 생성
        for(int i = 0; i < iCount; ++i)
        {
            Enemy enemy = GetObject(iKey) as Enemy;
            enemy.Active(RandPositionInNavMesh(centerPos));
        }
    }   //스폰

    private Vector3 RandPositionInNavMesh(Vector3 centerPos)
    {
        Vector2 randPos = Random.insideUnitCircle * 2.0f;
        randPos.x += 2;
        randPos.y += 2;
        NavMesh.SamplePosition(centerPos + new Vector3(randPos.x, 0.0f, randPos.y), out NavMeshHit navHit, 1.0f, NavMesh.AllAreas);
        return navHit.position;
    }   //중심점을 기준으로 +2 이상 되는곳에 생성

    /// <summary>
    /// obj : PooledObject 타입
    /// </summary>
    public void ActivePool(T obj)
    {
        //포함되어있지 않다면 삽입
        if(!mActiveObjects.Contains(obj))
        {
            mActiveObjects.Add(obj);
        }
    }   //활성화 된 오브젝트 추가

    /// <summary>
    /// obj : PooledObject 타입
    /// </summary>
    public void RemoveActiveOjbect(T obj)
    {
        //포함하고 있으면 제거
        if(mActiveObjects.Contains(obj))
        {
            mActiveObjects.Remove(obj);
        }
    }   //활성화 된 오브젝트 리턴

    public void UnActiveAllUnit()
    {
        while (mActiveObjects.Count > 0)
        {
            if (mActiveObjects[0] == null)
            {
                mActiveObjects.RemoveAt(0);
            }
            else
            {
                mActiveObjects[0].UnActive();
            }
        }
        mActiveObjects.Clear();
    }   //활성화 된 모든 오브젝트 비활성화
}
