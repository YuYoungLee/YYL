using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerUI playerUI;
    [SerializeField] EnemyUI enemyUI;
    [SerializeField] OptionSetPanel optionSetPanel;
    [SerializeField] GameInformation informationPanel;

    [Header("SaveText")]
    [SerializeField] GameObject saveObject;
    private Coroutine saveCoroutine = null;

    public PlayerUI GetPlayerUI => playerUI;
    public EnemyUI GetEnemyUI => enemyUI;
    public OptionSetPanel GetOptionPanel => optionSetPanel;
    public GameInformation GetInformation => informationPanel;

    public void StartSaveCoroutine()
    {
        if (saveCoroutine == null) saveCoroutine = StartCoroutine(SaveCoroutine());
    }

    public void StopSaveCoroutine()
    {
        if (saveCoroutine == null) return;
        StopCoroutine(saveCoroutine);
        saveCoroutine = null;
    }

    private IEnumerator SaveCoroutine()
    {
        saveObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        saveObject.SetActive(false);
        StopSaveCoroutine();
    }
}
