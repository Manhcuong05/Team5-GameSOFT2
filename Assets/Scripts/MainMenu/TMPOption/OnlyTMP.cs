using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TMP_ToggleColorGroup : MonoBehaviour
{
    [System.Serializable]
    public class ToggleItem
    {
        public Toggle toggle;
        public TMP_Text label;
    }

    [Header("Danh sách option")]
    public List<ToggleItem> items = new List<ToggleItem>();

    [Header("Màu")]
    public Color onColor = new Color(0.35f, 0.65f, 0.30f, 1f);
    public Color offColor = new Color(0.55f, 0.55f, 0.55f, 1f);

    private bool isUpdating = false;

    void Awake()
    {
        for (int i = 0; i < items.Count; i++)
        {
            int index = i;

            if (items[index].toggle == null)
                continue;

            items[index].toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(index, isOn));
        }

        RefreshColors();
    }

    void OnToggleChanged(int changedIndex, bool isOn)
    {
        if (isUpdating) return;

        isUpdating = true;

        if (isOn)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i != changedIndex && items[i].toggle != null)
                    items[i].toggle.isOn = false;
            }
        }

        RefreshColors();
        isUpdating = false;
    }

    void RefreshColors()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].label == null || items[i].toggle == null)
                continue;

            items[i].label.color = items[i].toggle.isOn ? onColor : offColor;
        }
    }
}