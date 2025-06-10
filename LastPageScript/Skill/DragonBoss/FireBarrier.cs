using System;
using System.Collections;
using UnityEngine;

public class FireBarrier : Skill
{
    private Action damagedEvent = null;

    int playerInSecond = 0;       //�÷��̾ ���ʵ��� �ȿ� �־����� ���
    int skillSec = 0;             //��ų ���ʵ��� ����Ұ����� ���
    protected override void Damage()
    {
    }
    protected override IEnumerator SkillCoroutine()
    {
        //�ʱ�ȭ
        skillSec = 0;
        PlayParticle(true, 0, particle.Length);

        while (skillSec < 50)
        {
            skillSec += 1;
            playerInSecond += 1;

            //ó�� ������ 0.4�ʵ��� �ȿ� �־��� ��, 0.5�ʸ��� ������ ������
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
    }   //������ ����, ������Ʈ ��Ȱ��ȭ, ����Ʈ ����
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            damagedEvent = () => other.GetComponent<Player>().Damaged(damage); //�÷��̾��� ������ �̺�Ʈ
        }
    }   //�÷��̾ �ش� ������Ʈ�� ����� �� ����

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerInSecond = 0;
            damagedEvent = null;
        }
    }   //�÷��̾ ������ �� �̺�Ʈ ����

}
