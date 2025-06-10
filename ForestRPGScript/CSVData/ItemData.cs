using System;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Item,       //�Ϲ� ������
    Armor,      //��� ������
    Potion,     //�Һ� ������

    Count,      //for�� ���ǹ�
}

public enum EItemGrade
{
    Empty,          //������� ��
    Normal,         //��� ���
    Rare,           //���� ���
    Unique,         //����ũ ���
    Legend,         //������ ���
}

public struct ItemData
{
    private EItemType meItemType;       //Ÿ��
    private int miKey;                  //Ű��
    private EItemGrade meItemGrade;     //���(ui �ܰ���)
    private int miMaxCount;             //�ִ� ����
    private string mstrName;            //�̸�
    private string mstrExplanation;     //����
    private List<int> mStatValue;       //���� ��
    private List<EStatDataType> mStatDataType;    //���� Ÿ��
    private Sprite mSprite;             //������ �̹���
    private EEquipType meEquipType;     //���� Ÿ��
    private int miPay;                  //���� ����
    private string mstrEquipItemKey;        //���� ������ Ű��
    public EItemType ItemType => meItemType;
    public int Key => miKey;
    public EItemGrade Grade => meItemGrade;
    public int MaxCount => miMaxCount;
    public string Name => mstrName;
    public string Explanation => mstrExplanation;
    public int StatValueCount => mStatValue.Count;
    public int StatValue(int idx) => mStatValue[idx];
    public EStatDataType StatType(int idx) => mStatDataType[idx];
    public Sprite Sprite => mSprite;
    public EEquipType EquipType => meEquipType;
    public int Pay => miPay;
    public string EquipItemKey => mstrEquipItemKey;

    /// <summary>
    /// csvData : csv ������
    /// </summary>
    public void SetData(string[] csvData)
    {
        meItemType = Enum.Parse<EItemType>(csvData[0]);
        miKey = int.Parse(csvData[1]);
        meItemGrade = Enum.Parse<EItemGrade>(csvData[2]);
        miMaxCount = int.Parse(csvData[3]);
        mstrName = csvData[4];
        mstrExplanation = csvData[5];
        
        //���� ������ ��
        if(mStatValue == null) 
        {
            mStatValue = new List<int>();
            string[] addValueSlice = CSVReader.Instance.GetListSlice(csvData[6]);
            for (int i = 0; i < addValueSlice.Length; ++i)
            { 
                mStatValue.Add(int.Parse(addValueSlice[i])); 
            }
        }

        //���� ������ Ÿ��
        if(mStatDataType == null) 
        {
            mStatDataType = new List<EStatDataType>();
            string[] typeSlice = CSVReader.Instance.GetListSlice(csvData[7]);
            for (int i = 0; i < typeSlice.Length; ++i)
            { 
                mStatDataType.Add(Enum.Parse<EStatDataType>(typeSlice[i])); 
            }
        }

        mSprite = Resources.Load<Sprite>(csvData[8]);       //�̹���
        meEquipType = Enum.Parse<EEquipType>(csvData[9]);   //���� ������ Ÿ��
        miPay = int.Parse(csvData[10]);     //���Ű���
        mstrEquipItemKey = csvData[11];     //���� ������ Ű��
    }
}
