using UnityEngine;

/// <summary>
/// The only script that spells a PlayerPrefs key. Everything the game remembers goes through here, and every
/// read has a default, so the very first run (nothing saved yet) works without any special case.
/// </summary>
public static class SaveData
{
    public const int LevelCount = 5;                 // the levels are build indexes 1 to 5

    const string UnlockedKey = "unlocked";
    const string MusicKey = "MusicVolume";
    const string SfxKey = "SfxVolume";

    // best1 ... best5. The level number is the build index of the level's scene.
    static string BestKey(int level) { return "best" + level; }

    // ---- levels ----
    // how many levels are open. Level 1 is always open, so the default is 1
    public static int Unlocked { get { return PlayerPrefs.GetInt(UnlockedKey, 1); } }

    public static bool IsUnlocked(int level) { return level <= Unlocked; }

    public static void Unlock(int level)
    {
        level = Mathf.Min(level, LevelCount);        // the scene after Level 5 is the credits, not a level
        if (level <= Unlocked) return;
        PlayerPrefs.SetInt(UnlockedKey, level);
        PlayerPrefs.Save();
    }

    // ---- best times ----
    // HasKey first: a missing float would read as 0, and 0 seconds looks like the best time anyone could have
    public static bool HasBest(int level) { return PlayerPrefs.HasKey(BestKey(level)); }

    public static float BestTime(int level)
    {
        return HasBest(level) ? PlayerPrefs.GetFloat(BestKey(level)) : 0f;
    }

    // saves the time if it is the first one or beats the old one. Returns true when it did.
    public static bool Record(int level, float seconds)
    {
        if (HasBest(level) && seconds >= BestTime(level)) return false;
        PlayerPrefs.SetFloat(BestKey(level), seconds);
        PlayerPrefs.Save();
        return true;
    }

    // ---- volume (slider values from 0.0001 to 1, not decibels) ----
    public static float MusicVolume
    {
        get { return PlayerPrefs.GetFloat(MusicKey, 1f); }
        set { PlayerPrefs.SetFloat(MusicKey, value); }
    }

    public static float SfxVolume
    {
        get { return PlayerPrefs.GetFloat(SfxKey, 1f); }
        set { PlayerPrefs.SetFloat(SfxKey, value); }
    }
}
