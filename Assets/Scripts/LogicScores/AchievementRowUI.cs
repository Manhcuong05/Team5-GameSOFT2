using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementRowUI : MonoBehaviour
{
    public TMP_Text noText;
    public TMP_Text titleText;
    public Image backgroundImage;

    [Range(0f, 1f)] public float unlockedAlpha = 1f;
    [Range(0f, 1f)] public float lockedAlpha = 0.3f;

    public void Setup(int index, string title, bool unlocked)
    {
        if (noText != null) noText.text = index.ToString();
        if (titleText != null) titleText.text = title;

        float alpha = unlocked ? unlockedAlpha : lockedAlpha;

        SetAlpha(noText, alpha);
        SetAlpha(titleText, alpha);

        if (backgroundImage != null)
        {
            Color c = backgroundImage.color;
            c.a = unlocked ? 0.15f : 0.05f;
            backgroundImage.color = c;
        }
    }

    void SetAlpha(TMP_Text text, float alpha)
    {
        if (text == null) return;
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }
}