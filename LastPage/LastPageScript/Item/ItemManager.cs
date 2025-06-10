using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public enum ItemType
{
    Artifact,           //아티팩트
    Consumption,        //소비형 아이템
    Default,
}
//아이템 데이터 베이스
public class ItemManager : MonoBehaviour
{
    private Dictionary<int, Item> itemDB = new Dictionary<int, Item>();               //아이템 DB
    private Queue<DropItem> dropItemObjectPool = new Queue<DropItem>();               //떨어진 아이템 오브젝트 풀
    private Dictionary<int, List<int>> shopDB = new Dictionary<int, List<int>>();     //상점 아이템 DB
    
    public void Initialize()
    {
        CreateDropItemPool();
        ReadItemData();
        ReadShopData();
    }
    private void CreateDropItemPool()
    {
        DropItem createItem = Resources.Load<DropItem>("Prefabs/Item/DropItem");
        DropItem dropItem;
        for (int i = 0; i < 10; ++i)
        {
            dropItem = GameObject.Instantiate(createItem);
            dropItem.gameObject.SetActive(false);
            dropItem.transform.SetParent(this.gameObject.transform, false);
            dropItem.returnPool += () => { ReturnDropItemPool(dropItem); };
            dropItemObjectPool.Enqueue(dropItem);
        }
    }   //풀에 아이템 생성
    public void ReturnDropItemPool(DropItem item)
    {
        item.SetActive(false);
        dropItemObjectPool.Enqueue(item);
    }   //드랍 아이템 오브젝트 풀에 집어넣기
    public int GetRandomItemKey()
    {
        return 200000 + UnityEngine.Random.Range(1, 6);
    }   //랜덤 키 리턴

    /// <summary>
    /// pos : 아이템 생성 위치
    /// key : 아이템DB 키값
    /// </summary>
    public void CreateDropItem(Vector3 pos, int key)
    {
        //키가 존제 할 때
        if (CheckItem(key))
        {
            //풀에 아이템 없을 때 생성
            if (dropItemObjectPool.Count <= 0) { CreateDropItemPool(); }

            DropItem item;
            dropItemObjectPool.TryDequeue(out item);
            item.SetItem(itemDB[key], pos);     //아이템 데이터 삽입   
        }
    }   //드랍 아이템 해당 좌표에 생성

    private void ReadItemData()
    {
        string[] lines = CSVReader.Read("Script/Item/ItemData");
        string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            Item item = new Item();
            item.SetData(values);
            itemDB[item.Key] = item;    //아이템 key 아이템 삽입
        }
    }   //CSV 파일 읽기

    private void ReadShopData()
    {
        string[] lines = CSVReader.Read("Script/Item/ShopData");
        string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            
            int shopKey = Int32.Parse(values[0]);   //상인의 키값
            if(!shopDB.ContainsKey(shopKey))        //키가 존제하지 않을 경우 리스트 생성
            { shopDB[shopKey] = new List<int>(); }
            for(int j = 1; j < values.Length; ++j)
            {
                shopDB[shopKey].Add(Int32.Parse(values[j]));
            }
        }
    }   //CSV 파일 읽기

    public bool CheckItem(int key)
    {
        return itemDB.ContainsKey(key) ? true : false;
    }   //아이템DB 키체크

    public Item GetItem(int key)
    {
        return itemDB[key];
    }

    public bool CheckShop(int key)
    {
        return shopDB.ContainsKey(key) ? true : false;
    }   //상점DB 키체크

    public List<int> GetItemList(int key)
    {
        return shopDB[key];
    }   //상점 아이템 리스트 반환
}
