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
    private EPooledObject mePooledObject;                //오브젝트 타입
    protected Transform mTransform;                      //트렌스폼
    public abstract event System.Action ReturnPool;      //풀링event

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

    public abstract void Initialize();   //초기화

    public abstract void UnActive();    //비활성화
}
