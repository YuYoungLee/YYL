using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    public DamageText textObject;
    private Queue<DamageText> damagePool = new Queue<DamageText>();

    public void Initialize()
    {
        CreateTextPool(10);
    }

    void CreateTextPool(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            DamageText text = Instantiate(textObject);
            text.transform.SetParent(transform);
            text.Initialize();
            text.returnPool += AddDamageText;
            text.cameraPos += GameManager.Instance.GetCameraController.GetPos;
            damagePool.Enqueue(text);
        }
    }

    public void PlayDamageText(int damage, Vector3 StartPos)
    {
        if (damagePool.Count < 0) CreateTextPool(5);
        
        DamageText text = damagePool.Dequeue();
        text.PlayFloatingText(damage, StartPos);
    }

    public void AddDamageText(DamageText text)
    {
        damagePool.Enqueue(text);
    }
}
