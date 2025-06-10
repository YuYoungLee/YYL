using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : PooledObject
{
    private Queue<T> mObjectPool;      //풀 queue
    private T mResource;      //리소스

    private Transform mParents;     //SetParent 시 사용할 transform
    private ObjectPoolManager<T> mObjPoolMgr;    //풀 메니저
    private int miCreateCount;      //생성 갯수

    /// <summary>
    /// path : 풀링할 오브젝트의 경로 ex. "Prefab/Bullet"
    /// parents : 비활성화시 설정할 부모
    /// objPoolMgr : 오브젝트풀 메니저
    /// </summary>
    public void Initialize(string path, Transform parents, ObjectPoolManager<T> objPoolMgr, int iCreateCount)
    {
        //중복 호출 되었다면 return
        if (mObjectPool != null) 
        { 
            return; 
        }
        mResource = Resources.Load<T>(path);     //경로에 있는 리소스 로드
        mObjectPool = new Queue<T>();            //풀링 생성
        mParents = parents;
        mObjPoolMgr = objPoolMgr;
        miCreateCount = iCreateCount;
        Create();
    }

    public void Create()
    {
        for (int i = 0; i < miCreateCount; ++i)
        {
            T obj = MonoBehaviour.Instantiate(mResource);      //생성
            obj.Initialize();                                  
            obj.transform.SetParent(mParents);                 //부모 설정
            obj.ReturnPool += () => ReturnPool(obj);           //풀로 돌아가기
            mObjectPool.Enqueue(obj);                          //삽입
        }
    }   //생성 후 풀에 삽입

    /// <summary>
    /// T : PooledObject 상속한 클래스
    /// </summary>
    public void ReturnPool(T obj)
    {
        if(obj == null)
        {
            return;
        }

        mObjPoolMgr.RemoveActiveOjbect(obj);      //활성화 풀에서 제거
        obj.gameObject.SetActive(false);          //비활성화 처리
        obj.transform.SetParent(mParents);        //부모로 설정

        //큐에 중복되지 않는다면 삽입
        if(!mObjectPool.Contains(obj))
        {
            mObjectPool.Enqueue(obj);
        }
    }   //풀로 리턴

    /// <summary>
    /// T : 풀링 오브젝트를 담을곳, return 용
    /// </summary>
    private bool GetObject(out T obj)
    {
        obj = null;
        //풀에 없을때 생성
        if (mObjectPool.Count == 0) 
        { 
            Create();
        }

        obj = mObjectPool.Dequeue();
        return true;
    }   //오브젝트를 꺼내면 true 리턴

    public T Spawn()
    {
        //오브젝트 받았다면 오브젝트 리턴
        if (GetObject(out T obj))
        {
            mObjPoolMgr.ActivePool(obj);    //활성화 풀에 집어넣기
            return obj; 
        }
        return null;
    }   //스폰
}
