using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public enum ItemType
{
    Artifact,           //��Ƽ��Ʈ
    Consumption,        //�Һ��� ������
    Default,
}
//������ ������ ���̽�
public class ItemManager : MonoBehaviour
{
    private Dictionary<int, Item> itemDB = new Dictionary<int, Item>();               //������ DB
    private Queue<DropItem> dropItemObjectPool = new Queue<DropItem>();               //������ ������ ������Ʈ Ǯ
    private Dictionary<int, List<int>> shopDB = new Dictionary<int, List<int>>();     //���� ������ DB
    
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
    }   //Ǯ�� ������ ����
    public void ReturnDropItemPool(DropItem item)
    {
        item.SetActive(false);
        dropItemObjectPool.Enqueue(item);
    }   //��� ������ ������Ʈ Ǯ�� ����ֱ�
    public int GetRandomItemKey()
    {
        return 200000 + UnityEngine.Random.Range(1, 6);
    }   //���� Ű ����

    /// <summary>
    /// pos : ������ ���� ��ġ
    /// key : ������DB Ű��
    /// </summary>
    public void CreateDropItem(Vector3 pos, int key)
    {
        //Ű�� ���� �� ��
        if (CheckItem(key))
        {
            //Ǯ�� ������ ���� �� ����
            if (dropItemObjectPool.Count <= 0) { CreateDropItemPool(); }

            DropItem item;
            dropItemObjectPool.TryDequeue(out item);
            item.SetItem(itemDB[key], pos);     //������ ������ ����   
        }
    }   //��� ������ �ش� ��ǥ�� ����

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
            itemDB[item.Key] = item;    //������ key ������ ����
        }
    }   //CSV ���� �б�

    private void ReadShopData()
    {
        string[] lines = CSVReader.Read("Script/Item/ShopData");
        string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            
            int shopKey = Int32.Parse(values[0]);   //������ Ű��
            if(!shopDB.ContainsKey(shopKey))        //Ű�� �������� ���� ��� ����Ʈ ����
            { shopDB[shopKey] = new List<int>(); }
            for(int j = 1; j < values.Length; ++j)
            {
                shopDB[shopKey].Add(Int32.Parse(values[j]));
            }
        }
    }   //CSV ���� �б�

    public bool CheckItem(int key)
    {
        return itemDB.ContainsKey(key) ? true : false;
    }   //������DB Űüũ

    public Item GetItem(int key)
    {
        return itemDB[key];
    }

    public bool CheckShop(int key)
    {
        return shopDB.ContainsKey(key) ? true : false;
    }   //����DB Űüũ

    public List<int> GetItemList(int key)
    {
        return shopDB[key];
    }   //���� ������ ����Ʈ ��ȯ
}
