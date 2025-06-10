using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Vector3 offset;
    private Transform trans;
    private Slider slider;
    private Coroutine hitCorutine = null;
    private int hitSecond = 0;
    public event System.Action returnPool = null;  //Ǯ�� ���ư���

    public void Initialize()
    {
        slider = GetComponent<Slider>();
    }
    public void SetData(Vector3 offset, Transform target, int maxHealth)
    {
        this.offset = offset;
        trans = target;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    public void ReturnPool()
    {
        returnPool?.Invoke();
    }

    public void UpdateUI(int hp)
    {
        slider.value = hp;
    }   //ü�¹� ũ�� ������Ʈ

    public void StartHitCoroutine()
    {
        hitSecond = 3;
        this.gameObject.SetActive(true);
        if (hitCorutine == null) hitCorutine = StartCoroutine(HitCoroutine());
    }

    private IEnumerator HitCoroutine()
    {
        while(hitSecond >= 0)
        {
            --hitSecond;    //�ð�����
            yield return new WaitForSeconds(1.0f);
        }
        StopHitCoroutine(); //���߱�
    }

    public void StopHitCoroutine()
    {
        StopCoroutine(HitCoroutine());
        hitCorutine = null;
        this.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(trans.position + Vector3.up * 2);

        //��ũ�� ��ġ�� ī�޶� ������ �����ϼ���
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        this.transform.position = screenPos;
    }
}
