using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine;

public class EnemyUI : GUI
{
    [SerializeField] BossHealthBar healthBar;

    public BossHealthBar HealthBar => healthBar;
}
