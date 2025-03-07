using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardExample : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.onSelect.AddListener(ShowKeyboard);
    }

    public void ShowKeyboard(string text)
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
    }
}
