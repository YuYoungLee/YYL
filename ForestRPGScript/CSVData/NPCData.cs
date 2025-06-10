public class NPCData
{
    private string mstrName;    //NPC 이름
    private int miInitKey;      //NPC 초기 키값, 없을시 -1값

    public string Name => mstrName;
    public int InitKey => miInitKey;

    public void SetData(string[] data)
    {
        mstrName = data[1];
        miInitKey = int.Parse(data[2]);
    }
}
