public class NPCData
{
    private string mstrName;    //NPC �̸�
    private int miInitKey;      //NPC �ʱ� Ű��, ������ -1��

    public string Name => mstrName;
    public int InitKey => miInitKey;

    public void SetData(string[] data)
    {
        mstrName = data[1];
        miInitKey = int.Parse(data[2]);
    }
}
