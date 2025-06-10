using System;
using System.Collections.Generic;

public class EnemyData
{
    private int miKey;              //에너미 키값
    private string mstrName;        //에너미 이름
    private int miMaxHp;            //최대 체력
    private int miAttacker;         //공격력
    private int miDefense;          //방어력
    private float mfHitDeleay;      //공격 딜레이
    private List<int> mSkillKey;    //스킬 키값
    private int miGetExe;           //상대받이 얻을 경험치
    private int miDropTableKey;     //드랍 테이블 키
    private string mstrAttackSoundKey;
    private string mstrHitSoundKey;
    private EEnemyType meEnemyType;

    public int Key => miKey;
    public string Name => mstrName;
    public int MaxHp => miMaxHp;
    public int Attacker => miAttacker;
    public int Defense => miDefense;
    public float HitDeleay => mfHitDeleay;
    public List<int> SkillKey => mSkillKey;
    public int GetExe => miGetExe;
    public int DropTableKey => miDropTableKey;
    public EEnemyType GetEnemyType() => meEnemyType;

    public void SetData(string[] strData)
    {
        miKey = int.Parse(strData[0]);         //에너미 키
        mstrName = strData[1];
        miMaxHp = int.Parse(strData[2]);
        miAttacker = int.Parse(strData[3]);    //공격력
        miDefense = int.Parse(strData[4]);     //방어력
        mfHitDeleay = float.Parse(strData[5]);      //공격 딜레이

        //스킬 키
        if(mSkillKey == null)
        {
            mSkillKey = new List<int>();
            //스킬 키값 슬라이스
            string[] strKeySlice = CSVReader.Instance.GetListSlice(strData[6]);
            for (int i = 0; i < strKeySlice.Length; ++i)
            {
                mSkillKey.Add(int.Parse(strKeySlice[i]));
            }
        }

        miGetExe = int.Parse(strData[7]);              //상대방이 얻을 경험치
        miDropTableKey = int.Parse(strData[8]);        //드랍 테이블 키

        mstrAttackSoundKey = strData[9];
        mstrHitSoundKey = strData[10];

        meEnemyType = Enum.Parse<EEnemyType>(strData[11]);
    }
}
