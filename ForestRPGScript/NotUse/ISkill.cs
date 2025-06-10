public interface ISkill
{
    public int SetKey { set; }      //스킬 키값 저장
    public int SetMask { set; }      //스킬 키값 저장
    public bool PlayCheck { get; }  //스킬 플레이 시 true 리턴
    public void SetData();    //스킬 플레이 함수
}