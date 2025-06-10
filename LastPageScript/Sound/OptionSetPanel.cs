public class OptionSetPanel : GUI
{
    public void SetBGMVolumeButton(float volume)
    {
        GameManager.Instance.GetSoundManager.SetBGMVolume(volume);
    }

    public void SetEffectVolumeButton(float volume)
    {
        GameManager.Instance.GetSoundManager.SetEffectVolume(volume);
    }
}
