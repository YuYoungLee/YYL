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
    private Dictionary<int, ObjectPool<T>> objectPool;      //������Ʈ Ǯ
    private List<T> mActiveObjects;                         //Ȱ��ȭ �� ������Ʈ ����
    private Transform pooledParent;                         //������Ʈ Ǯ �θ��� ��ġ

    public T GetObject(int iKey) => objectPool[iKey].Spawn();   //���� ����

    public void Initialize()
    {
        //���ҽ� �߰�
        pooledParent = GameObject.Find("PooledObject").transform;  //������Ʈ ã��

        if (mActiveObjects == null) 
        {
            mActiveObjects =  new List<T>(); 
        }

        //Ǯ�� ��� ���� ���
        if (objectPool == null)
        {
            string[] data = CSVReader.Instance.LoadCSV("CSVData/ObjResourceData");     //���ҽ� ���� ������, pooling
            objectPool = new Dictionary<int, ObjectPool<T>>();
            for (int i = 1, iKey = 0; i < data.Length; ++i)
            {
                string[] sliceData = CSVReader.Instance.GetLineSlice(data[i]);     //������ �����̽�
                iKey = int.Parse(sliceData[0]);     //Ű��

                //Ǯ ��������� �߰�
                if(!objectPool.ContainsKey(iKey))
                {
                    objectPool[iKey] = new ObjectPool<T>();      //Ǯ ����
                    objectPool[iKey].Initialize(sliceData[1], pooledParent, this, int.Parse(sliceData[2]));      //�ʱ�ȭ
                }
            }
        }
    }


    /// <summary>
    /// iKey : ���� Ű��
    /// iCount : ��ȯ�� ����
    /// centerPos : ���� ����
    /// </summary>
    public void SpawnEnemy(int iKey, int iCount, Vector3 centerPos)
    {
        //Ű�� ���Ե��� �ʴ´ٸ� ����
        if (!objectPool.ContainsKey(iKey))
        {
            return;
        }

        //iCount ��ŭ ���� ����
        for(int i = 0; i < iCount; ++i)
        {
            Enemy enemy = GetObject(iKey) as Enemy;
            enemy.Active(RandPositionInNavMesh(centerPos));
        }
    }   //����

    private Vector3 RandPositionInNavMesh(Vector3 centerPos)
    {
        Vector2 randPos = Random.insideUnitCircle * 2.0f;
        randPos.x += 2;
        randPos.y += 2;
        NavMesh.SamplePosition(centerPos + new Vector3(randPos.x, 0.0f, randPos.y), out NavMeshHit navHit, 1.0f, NavMesh.AllAreas);
        return navHit.position;
    }   //�߽����� �������� +2 �̻� �Ǵ°��� ����

    /// <summary>
    /// obj : PooledObject Ÿ��
    /// </summary>
    public void ActivePool(T obj)
    {
        //���ԵǾ����� �ʴٸ� ����
        if(!mActiveObjects.Contains(obj))
        {
            mActiveObjects.Add(obj);
        }
    }   //Ȱ��ȭ �� ������Ʈ �߰�

    /// <summary>
    /// obj : PooledObject Ÿ��
    /// </summary>
    public void RemoveActiveOjbect(T obj)
    {
        //�����ϰ� ������ ����
        if(mActiveObjects.Contains(obj))
        {
            mActiveObjects.Remove(obj);
        }
    }   //Ȱ��ȭ �� ������Ʈ ����

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
    }   //Ȱ��ȭ �� ��� ������Ʈ ��Ȱ��ȭ
}
