using System;
using UnityEngine;

public enum AddValueType
{
    Default,                //더할것이 없을 때 처리
    Health,                 //체력
    MaxHealth,              //최대 체력
    MoveSpeed,              //이속
    AttackSpeed,            //공속
    Critical,               //크리티컬
    Defend                  //방어력
}

public struct Item
{
    private ItemType type;              //아이템 타입
    private int key;                    //아이템 키
    private string name;                //아이템 이름
    private string description;         //아이템 설명
    private int price;                  //상점 가격
    private int addValue;               //더할 값
    private AddValueType valueType;     //더해야 할 값 타입
    private Sprite icon;
    //아이템 가격, 아이템 회복 enum, 회복할 값

    public ItemType Type => type;
    public int Key => key;
    public string Name => name;
    public string Description => description;
    public int Price => price;
    public int AddValue => addValue;
    public AddValueType ValueType => valueType;
    public Sprite Icon => icon;


    /// <summary>
    /// data : csv 문자
    /// </summary>
    public void SetData(string[] data)
    {
        type = (ItemType)Enum.Parse(typeof(ItemType), data[0]);
        key = Int32.Parse(data[1]);
        name = data[2];
        description = data[3];
        price = Int32.Parse(data[4]);
        addValue = Int32.Parse(data[5]);
        valueType = (AddValueType)Enum.Parse(typeof(AddValueType), data[6]);
        icon = Resources.Load<Sprite>(data[7]);
    }   //데이터 설정
}
