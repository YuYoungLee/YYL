public interface ISkill
{
    public int SetKey { set; }      //��ų Ű�� ����
    public int SetMask { set; }      //��ų Ű�� ����
    public bool PlayCheck { get; }  //��ų �÷��� �� true ����
    public void SetData();    //��ų �÷��� �Լ�
}