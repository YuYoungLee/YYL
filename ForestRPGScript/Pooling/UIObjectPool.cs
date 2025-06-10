using System.Collections.Generic;
using UnityEngine;

public class UIObjectPool : MonoBehaviour
{
    [SerializeField] private Canvas mTargetCanvas;
    [SerializeField] private RectTransform mRect;
    private Queue<HPBar> mEnemyHPSlider;     //에너미 HP슬라이더
    [SerializeField] TrackingBarSlider mResource;

    [SerializeField] private GameObject mGameObject;    //OBJPool 오브젝트
    public void Initialize()
    {
        if(mEnemyHPSlider == null)
        {
            mEnemyHPSlider = new Queue<HPBar>();
            CreateHP(5);
        }
    }

    public void SetActive(bool bActive)
    {
        mGameObject.SetActive(bActive);
    }   //오브젝트 비활성화

    private void CreateHP(int iCount)
    {
        for(int i = 0; i < iCount; ++i)
        {
            TrackingBarSlider hpSlider = Instantiate(mResource);
            hpSlider.Initialize(mTargetCanvas, mRect);
            mEnemyHPSlider.Enqueue(hpSlider);
        }
    }

    public HPBar GetHp()
    {
        //없다면 생성
        if(mEnemyHPSlider.Count < 1)
        {
            CreateHP(5);
        }

        return mEnemyHPSlider.Dequeue();
    }

    public void ReturnPool(HPBar hpBar)
    {
        //포함되어 있지 않다면
        if(!mEnemyHPSlider.Contains(hpBar))
        {
            hpBar.gameObject.SetActive(false);
            mEnemyHPSlider.Enqueue(hpBar);
        }
    }
}
