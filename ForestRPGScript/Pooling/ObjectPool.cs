using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : PooledObject
{
    private Queue<T> mObjectPool;      //Ǯ queue
    private T mResource;      //���ҽ�

    private Transform mParents;     //SetParent �� ����� transform
    private ObjectPoolManager<T> mObjPoolMgr;    //Ǯ �޴���
    private int miCreateCount;      //���� ����

    /// <summary>
    /// path : Ǯ���� ������Ʈ�� ��� ex. "Prefab/Bullet"
    /// parents : ��Ȱ��ȭ�� ������ �θ�
    /// objPoolMgr : ������ƮǮ �޴���
    /// </summary>
    public void Initialize(string path, Transform parents, ObjectPoolManager<T> objPoolMgr, int iCreateCount)
    {
        //�ߺ� ȣ�� �Ǿ��ٸ� return
        if (mObjectPool != null) 
        { 
            return; 
        }
        mResource = Resources.Load<T>(path);     //��ο� �ִ� ���ҽ� �ε�
        mObjectPool = new Queue<T>();            //Ǯ�� ����
        mParents = parents;
        mObjPoolMgr = objPoolMgr;
        miCreateCount = iCreateCount;
        Create();
    }

    public void Create()
    {
        for (int i = 0; i < miCreateCount; ++i)
        {
            T obj = MonoBehaviour.Instantiate(mResource);      //����
            obj.Initialize();                                  
            obj.transform.SetParent(mParents);                 //�θ� ����
            obj.ReturnPool += () => ReturnPool(obj);           //Ǯ�� ���ư���
            mObjectPool.Enqueue(obj);                          //����
        }
    }   //���� �� Ǯ�� ����

    /// <summary>
    /// T : PooledObject ����� Ŭ����
    /// </summary>
    public void ReturnPool(T obj)
    {
        if(obj == null)
        {
            return;
        }

        mObjPoolMgr.RemoveActiveOjbect(obj);      //Ȱ��ȭ Ǯ���� ����
        obj.gameObject.SetActive(false);          //��Ȱ��ȭ ó��
        obj.transform.SetParent(mParents);        //�θ�� ����

        //ť�� �ߺ����� �ʴ´ٸ� ����
        if(!mObjectPool.Contains(obj))
        {
            mObjectPool.Enqueue(obj);
        }
    }   //Ǯ�� ����

    /// <summary>
    /// T : Ǯ�� ������Ʈ�� ������, return ��
    /// </summary>
    private bool GetObject(out T obj)
    {
        obj = null;
        //Ǯ�� ������ ����
        if (mObjectPool.Count == 0) 
        { 
            Create();
        }

        obj = mObjectPool.Dequeue();
        return true;
    }   //������Ʈ�� ������ true ����

    public T Spawn()
    {
        //������Ʈ �޾Ҵٸ� ������Ʈ ����
        if (GetObject(out T obj))
        {
            mObjPoolMgr.ActivePool(obj);    //Ȱ��ȭ Ǯ�� ����ֱ�
            return obj; 
        }
        return null;
    }   //����
}
