using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private Transform trans;
    private TextMeshPro damageText;
    private Color color;
    private Coroutine textCoroutine = null;
    private StringBuilder strBuilder = new StringBuilder();

    private float reduceAlpha = 0.2f;       //���İ� ���� �ӵ�
    private float moveSpeed = 1.0f;        //�ؽ�Ʈ �����̴� �ӵ�

    public delegate void DamageTextPool(DamageText text);
    public DamageTextPool returnPool;                       //Ǯ�� ����

    public delegate Vector3 GetCameraPos();
    public GetCameraPos cameraPos;                          //ī�޶� ��ǥ

    public void Initialize()
    {
        trans = GetComponent<Transform>();
        damageText = GetComponent<TextMeshPro>();
        color = damageText.color;
        this.gameObject.SetActive(false);
    }   //�ʱ�ȭ

    public void PlayFloatingText(int damage, Vector3 position)
    {
        if (textCoroutine != null) return;
        color.a = 1;                                        //���İ� �ʱ�ȭ
        moveSpeed = 6.0f;                                   //�����̴� �ӵ�
        trans.position = position + Vector3.up * 2.3f;      //������ġ ��������

        strBuilder.Clear();
        strBuilder.Append(damage);
        damageText.text = strBuilder.ToString();            //������ �ؽ�Ʈ

        this.gameObject.SetActive(true);                    //������Ʈ Ȱ��ȭ
        textCoroutine = StartCoroutine(TextCoroutine());    //�ڷ�ƾ ����
    }   //�÷��� �ؽ�Ʈ ���

    private void Stop()
    {
        StopCoroutine(TextCoroutine());
        textCoroutine = null;
        this.gameObject.SetActive(false);
        returnPool(this);
    }   //�ڷ�ƾ ���߱�

    private IEnumerator TextCoroutine()
    {
        Vector3 lookPlayer = Vector3.zero;
        float timeCheck = 0.0f;
        damageText.color = color;
        while (timeCheck < 2.0f)
        {
            timeCheck += 0.1f;
            if (timeCheck > 1.5f)
            {
                color.a -= reduceAlpha;   //�ؽ�Ʈ ���İ� ����
                moveSpeed -= 0.5f;        //�����̴� �ӵ� ����
            }
            //�ؽ�Ʈ �������� �ö󰡱�
            trans.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            damageText.color = color;

            //���� �ٶ󺸱�
            lookPlayer = trans.position - GameManager.Instance.GetCameraController.GetPos();
            lookPlayer.y = 0;
            trans.rotation = Quaternion.LookRotation(lookPlayer.normalized);
            yield return new WaitForSeconds(0.1f);
        }
        Stop();
    }
}
