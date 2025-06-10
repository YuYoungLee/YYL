using System.Collections;
using UnityEngine;

//투사체 공격
public class Projectile : MonoBehaviour
{
    Coroutine fireCoroutine = null;                     //투사체 코루틴
    Vector3 startPos = new Vector3(0, 0.8f, 1f);        //발사 시작할 위치
    Vector3[] curveFire = new Vector3[32];              //커브 저장할 벡터
    bool fireCheck = false;     //발사 준비가 되면 true
    int damage = 0;             //데미지 입힐때
    int iCurveSize = 0;         //코루틴 순회시 사용할 index

    public void Initialize(int damage)
    {
        this.gameObject.SetActive(false);
        this.damage = damage;                 //투사체의 공격력
    }

    public bool ReadyFire(Vector3 targetPos, Vector3 StartPos)
    {
        if (fireCheck) return false;        //발사중일때 false 리턴
        transform.position = startPos;      //투사체 위치 초기화
        curveFire = GameManager.Instance.GetPath(StartPos, targetPos, 3);  //Curve 찾기
        return true;
    }   //발사 준비가 될때 발사 시퀀스 추가 true 리턴

    public void Fire(int damage)
    {
        fireCheck = true;                                      //발사 준비 비활성화
        this.gameObject.SetActive(true);                       //활성화
        if (fireCoroutine != null) return;
        fireCoroutine = StartCoroutine(CurveCorutine());
    }   //공격 초기화 및 코루틴 시작

    public void Stop()
    {
        StopCoroutine(CurveCorutine());         //코루틴 종료
        fireCoroutine = null;                   //코루틴 비우기
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) other.GetComponent<Player>().Damaged(damage);   //other.SendMessage("Damaged", damaged);
        else return;

        this.gameObject.SetActive(false);       //투사체 비활성화
        fireCheck = false;                      //발사종료
        iCurveSize = 0;                         //index 0 초기화
        StopCoroutine("CurveCorutine");         //코루틴 종료
        fireCoroutine = null;                   //코루틴 비우기
    }   //투사체 충돌할 때 처리

    IEnumerator CurveCorutine()
    {
        while(iCurveSize < curveFire.Length)
        {
            this.transform.position = curveFire[iCurveSize];
            ++iCurveSize;
            yield return new WaitForSeconds(0.1f);
        }

        fireCheck = false;                      //발사종료
        this.gameObject.SetActive(false);       //투사체 비활성화
        iCurveSize = 0;                         //index 0 초기화
        Stop();
    }   //투사체 커브 코루틴
}
