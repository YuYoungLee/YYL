using System.Collections.Generic;
using UnityEngine;

public class QuestTapPanel : GUI
{
    [SerializeField] private List<QuestTracker> mQuestTap;

    public override void Initialize()
    {
        base.Initialize();

        for(int i = 0; i < mQuestTap.Count; ++i)
        {
            mQuestTap[i].Initialize();    //Äù½ºÆ® ÅÇ ÃÊ±âÈ­
        }
    }

    public bool GetQuestTap(out QuestTracker questTap)
    {
        questTap = null;
        for (int i = 0; i < mQuestTap.Count; ++i)
        {
            //ºñÈ°¼ºÈ­ ÅÇÀÎ °æ¿ì
            if (!mQuestTap[i].IsActive)
            {
                questTap = mQuestTap[i];
                return true;
            }
        }

        return false;
    }
}
