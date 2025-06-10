using UnityEngine;

public class GameOptionPanel : GUI
{
    public void OptionButtonDown()
    {
        this.SetActive(false);
        GameManager.Instance.GetUIManager().GetPlayerUI.GetgameOptionPanel.SetActive(true);
    }
    public void MainButtonDown()
    {
        this.SetActive(false);
        GameManager.Instance.ChangeScene(SceneLoad.MAIN);
    }
    public void InformationButtonDown()
    {
        this.SetActive(false);
        GameManager.Instance.GetUIManager().GetInformation.SetActive(true);
    }
    public void ExitButtonDown()
    {
        Application.Quit();
    }
}
