using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// The Audio Mixer works in decibels and the sliders work from 0.0001 to 1, so every volume goes through here.
/// 20 times the log of the slider is the decibel change: 1 is 0 dB, 0.5 is about -6 dB, 0.0001 is -80 dB (silent).
/// </summary>
public static class MixerVolumes
{
    public const string MusicParameter = "MusicVolume";   // the exposed parameter names in the mixer
    public const string SfxParameter = "SfxVolume";
    public const float MinimumValue = 0.0001f;

    public static float ToDecibels(float value)
    {
        return Mathf.Log10(Mathf.Max(value, MinimumValue)) * 20f;
    }

    // puts the saved volumes on the mixer. A mixer starts every session at the levels it was saved with.
    public static void ApplySaved(AudioMixer mixer)
    {
        mixer.SetFloat(MusicParameter, ToDecibels(SaveData.MusicVolume));
        mixer.SetFloat(SfxParameter, ToDecibels(SaveData.SfxVolume));
    }
}
