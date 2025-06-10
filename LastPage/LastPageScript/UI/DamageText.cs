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

    private float reduceAlpha = 0.2f;       //알파값 변경 속도
    private float moveSpeed = 1.0f;        //텍스트 움직이는 속도

    public delegate void DamageTextPool(DamageText text);
    public DamageTextPool returnPool;                       //풀로 리턴

    public delegate Vector3 GetCameraPos();
    public GetCameraPos cameraPos;                          //카메라 좌표

    public void Initialize()
    {
        trans = GetComponent<Transform>();
        damageText = GetComponent<TextMeshPro>();
        color = damageText.color;
        this.gameObject.SetActive(false);
    }   //초기화

    public void PlayFloatingText(int damage, Vector3 position)
    {
        if (textCoroutine != null) return;
        color.a = 1;                                        //알파값 초기화
        moveSpeed = 6.0f;                                   //움직이는 속도
        trans.position = position + Vector3.up * 2.3f;      //시작위치 위쪽으로

        strBuilder.Clear();
        strBuilder.Append(damage);
        damageText.text = strBuilder.ToString();            //데미지 텍스트

        this.gameObject.SetActive(true);                    //오브젝트 활성화
        textCoroutine = StartCoroutine(TextCoroutine());    //코루틴 시작
    }   //플로팅 텍스트 재생

    private void Stop()
    {
        StopCoroutine(TextCoroutine());
        textCoroutine = null;
        this.gameObject.SetActive(false);
        returnPool(this);
    }   //코루틴 멈추기

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
                color.a -= reduceAlpha;   //텍스트 알파값 감소
                moveSpeed -= 0.5f;        //움직이는 속도 감소
            }
            //텍스트 위쪽으로 올라가기
            trans.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            damageText.color = color;

            //방향 바라보기
            lookPlayer = trans.position - GameManager.Instance.GetCameraController.GetPos();
            lookPlayer.y = 0;
            trans.rotation = Quaternion.LookRotation(lookPlayer.normalized);
            yield return new WaitForSeconds(0.1f);
        }
        Stop();
    }
}
