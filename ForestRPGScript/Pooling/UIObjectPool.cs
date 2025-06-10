using System.Collections.Generic;
using UnityEngine;

public class UIObjectPool : MonoBehaviour
{
    [SerializeField] private Canvas mTargetCanvas;
    [SerializeField] private RectTransform mRect;
    private Queue<HPBar> mEnemyHPSlider;     //���ʹ� HP�����̴�
    [SerializeField] TrackingBarSlider mResource;

    [SerializeField] private GameObject mGameObject;    //OBJPool ������Ʈ
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
    }   //������Ʈ ��Ȱ��ȭ

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
        //���ٸ� ����
        if(mEnemyHPSlider.Count < 1)
        {
            CreateHP(5);
        }

        return mEnemyHPSlider.Dequeue();
    }

    public void ReturnPool(HPBar hpBar)
    {
        //���ԵǾ� ���� �ʴٸ�
        if(!mEnemyHPSlider.Contains(hpBar))
        {
            hpBar.gameObject.SetActive(false);
            mEnemyHPSlider.Enqueue(hpBar);
        }
    }
}
