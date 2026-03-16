using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Toggle))]
public class TMP_ToggleColor : MonoBehaviour
{
    public TMP_Text label;
    public Color onColor = new Color(0.2f, 0.8f, 0.2f);
    public Color offColor = new Color(0.55f, 0.55f, 0.55f);

    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(UpdateColor);
        UpdateColor(toggle.isOn);
    }

    void UpdateColor(bool isOn)
    {
        label.color = isOn ? onColor : offColor;
    }
}