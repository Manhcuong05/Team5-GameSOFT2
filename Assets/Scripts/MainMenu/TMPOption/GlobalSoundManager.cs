using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSoundApplier : MonoBehaviour
{
    public static GlobalSoundApplier Instance;

    private const string SOUND_KEY = "sound_enabled";

    public bool SoundEnabled { get; private set; } = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate()
    {
        if (Instance != null) return;

        GameObject obj = new GameObject("GlobalSoundApplier");
        Instance = obj.AddComponent<GlobalSoundApplier>();
        DontDestroyOnLoad(obj);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadFromPrefs();
        SceneManager.sceneLoaded += OnSceneLoaded;
        ApplySoundState();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadFromPrefs();
        ApplySoundState();
    }

    private void LoadFromPrefs()
    {
        SoundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
    }

    public void SetSound(bool enabled)
    {
        SoundEnabled = enabled;

        PlayerPrefs.SetInt(SOUND_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplySoundState();
    }

    public void ApplySoundFromPrefs()
    {
        LoadFromPrefs();
        ApplySoundState();
    }

    private void ApplySoundState()
    {
        AudioListener.volume = SoundEnabled ? 1f : 0f;
        AudioListener.pause = !SoundEnabled;

        AudioSource[] allSources = FindObjectsOfType<AudioSource>(true);
        foreach (AudioSource source in allSources)
        {
            if (source != null)
                source.mute = !SoundEnabled;
        }
    }
}