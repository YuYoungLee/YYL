using System;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Item,       //일반 아이템
    Armor,      //장비 아이템
    Potion,     //소비 아이템

    Count,      //for문 조건문
}

public enum EItemGrade
{
    Empty,          //비어있을 때
    Normal,         //평범 등급
    Rare,           //레어 등급
    Unique,         //유니크 등급
    Legend,         //레전드 등급
}

public struct ItemData
{
    private EItemType meItemType;       //타입
    private int miKey;                  //키값
    private EItemGrade meItemGrade;     //등급(ui 외곽선)
    private int miMaxCount;             //최대 개수
    private string mstrName;            //이름
    private string mstrExplanation;     //설명
    private List<int> mStatValue;       //스텟 값
    private List<EStatDataType> mStatDataType;    //스텟 타입
    private Sprite mSprite;             //아이템 이미지
    private EEquipType meEquipType;     //장착 타입
    private int miPay;                  //구매 가격
    private string mstrEquipItemKey;        //장착 아이템 키값
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
    /// csvData : csv 데이터
    /// </summary>
    public void SetData(string[] csvData)
    {
        meItemType = Enum.Parse<EItemType>(csvData[0]);
        miKey = int.Parse(csvData[1]);
        meItemGrade = Enum.Parse<EItemGrade>(csvData[2]);
        miMaxCount = int.Parse(csvData[3]);
        mstrName = csvData[4];
        mstrExplanation = csvData[5];
        
        //스텟 데이터 값
        if(mStatValue == null) 
        {
            mStatValue = new List<int>();
            string[] addValueSlice = CSVReader.Instance.GetListSlice(csvData[6]);
            for (int i = 0; i < addValueSlice.Length; ++i)
            { 
                mStatValue.Add(int.Parse(addValueSlice[i])); 
            }
        }

        //스텟 데이터 타입
        if(mStatDataType == null) 
        {
            mStatDataType = new List<EStatDataType>();
            string[] typeSlice = CSVReader.Instance.GetListSlice(csvData[7]);
            for (int i = 0; i < typeSlice.Length; ++i)
            { 
                mStatDataType.Add(Enum.Parse<EStatDataType>(typeSlice[i])); 
            }
        }

        mSprite = Resources.Load<Sprite>(csvData[8]);       //이미지
        meEquipType = Enum.Parse<EEquipType>(csvData[9]);   //장착 아이템 타입
        miPay = int.Parse(csvData[10]);     //구매가격
        mstrEquipItemKey = csvData[11];     //장착 아이템 키값
    }
}
