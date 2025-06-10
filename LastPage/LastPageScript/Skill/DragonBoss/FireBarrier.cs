using System;
using System.Collections;
using UnityEngine;

public class FireBarrier : Skill
{
    private Action damagedEvent = null;

    int playerInSecond = 0;       //플레이어가 몇초동안 안에 있었는지 계산
    int skillSec = 0;             //스킬 몇초동안 재생할것인지 계산
    protected override void Damage()
    {
    }
    protected override IEnumerator SkillCoroutine()
    {
        //초기화
        skillSec = 0;
        PlayParticle(true, 0, particle.Length);

        while (skillSec < 50)
        {
            skillSec += 1;
            playerInSecond += 1;

            //처음 데미지 0.4초동안 안에 있었을 때, 0.5초마다 데미지 입히기
            if (playerInSecond == 4 || (playerInSecond - 4) % 5 == 0)
            {
                damagedEvent?.Invoke();
            }

            yield return new WaitForSeconds(0.1f);
        }
        StopParticle(0, particle.Length);
        StopSkill();
    }
    public void Initialize(int damage)
    {
        this.damage = damage;
        this.gameObject.SetActive(false);
    }   //데이터 삽입, 오브젝트 비활성화, 이펙트 멈춤
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            damagedEvent = () => other.GetComponent<Player>().Damaged(damage); //플레이어의 데미지 이벤트
        }
    }   //플레이어가 해당 오브젝트에 닿았을 때 실행

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerInSecond = 0;
            damagedEvent = null;
        }
    }   //플레이어가 나갔을 때 이벤트 비우기

}
