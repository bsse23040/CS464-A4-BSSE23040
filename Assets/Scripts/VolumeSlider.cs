using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Connects one slider to one mixer group. It goes on the Canvas, not on the pause panel: the pause panel is switched
/// off, and the scripts on a switched-off object do not start until it opens.
/// </summary>
public class VolumeSlider : MonoBehaviour
{
    public enum Channel { Music, Sfx }

    [SerializeField] Channel channel = Channel.Music;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;                  // may sit on a hidden panel, this script does not

    string Parameter
    {
        get { return channel == Channel.Music ? MixerVolumes.MusicParameter : MixerVolumes.SfxParameter; }
    }

    float Saved
    {
        get { return channel == Channel.Music ? SaveData.MusicVolume : SaveData.SfxVolume; }
    }

    void Start()
    {
        slider.minValue = MixerVolumes.MinimumValue;   // 0 would be minus infinity decibels
        slider.maxValue = 1f;
        slider.SetValueWithoutNotify(Saved);
        mixer.SetFloat(Parameter, MixerVolumes.ToDecibels(slider.value));
        slider.onValueChanged.AddListener(OnChanged);
    }

    void OnDestroy()
    {
        if (slider != null) slider.onValueChanged.RemoveListener(OnChanged);
    }

    void OnChanged(float value)
    {
        mixer.SetFloat(Parameter, MixerVolumes.ToDecibels(value));
        if (channel == Channel.Music) SaveData.MusicVolume = value;
        else SaveData.SfxVolume = value;
    }
}
