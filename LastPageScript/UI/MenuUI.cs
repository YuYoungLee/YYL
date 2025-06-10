using UnityEngine;

public class MenuUI : GUI
{
    [SerializeField] SaveLoadPanel saveLoadPanel;

    public SaveLoadPanel GetSaveLoadPanel => saveLoadPanel; 

    public void SelectButton ()
    {
        saveLoadPanel.SetActive(true);
    }

    public void OptionPanel()
    {
        GameManager.Instance.GetUIManager().GetOptionPanel.SetActive(true);
    }

    public void InformationPanel()
    {
        GameManager.Instance.GetUIManager().GetInformation.SetActive(true);
    }

    public void QuitGame()
    {
        GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.MenuSelect);
        Application.Quit();
    }
}
