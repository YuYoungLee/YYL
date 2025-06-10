using UnityEngine;

public enum EPooledObject
{
    MovingMagicProjectile,
    DropItem,
    FloatingText,
    Slime,
    Bear,
    Particle,
}

public abstract class PooledObject : MonoBehaviour
{
    private EPooledObject mePooledObject;                //������Ʈ Ÿ��
    protected Transform mTransform;                      //Ʈ������
    public abstract event System.Action ReturnPool;      //Ǯ��event

    public EPooledObject EPooledObject
    {
        get
        {
            return mePooledObject;
        }

        protected set
        {
            mePooledObject = value;
        }
    }

    public abstract void Initialize();   //�ʱ�ȭ

    public abstract void UnActive();    //��Ȱ��ȭ
}
