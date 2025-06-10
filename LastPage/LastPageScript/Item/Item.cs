using System;
using UnityEngine;

public enum AddValueType
{
    Default,                //���Ұ��� ���� �� ó��
    Health,                 //ü��
    MaxHealth,              //�ִ� ü��
    MoveSpeed,              //�̼�
    AttackSpeed,            //����
    Critical,               //ũ��Ƽ��
    Defend                  //����
}

public struct Item
{
    private ItemType type;              //������ Ÿ��
    private int key;                    //������ Ű
    private string name;                //������ �̸�
    private string description;         //������ ����
    private int price;                  //���� ����
    private int addValue;               //���� ��
    private AddValueType valueType;     //���ؾ� �� �� Ÿ��
    private Sprite icon;
    //������ ����, ������ ȸ�� enum, ȸ���� ��

    public ItemType Type => type;
    public int Key => key;
    public string Name => name;
    public string Description => description;
    public int Price => price;
    public int AddValue => addValue;
    public AddValueType ValueType => valueType;
    public Sprite Icon => icon;


    /// <summary>
    /// data : csv ����
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
    }   //������ ����
}
