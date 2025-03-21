using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public float startDelay;
    public string dialogue;
    public float readTime;
    public Color textColor; // Nueva propiedad para el color
}


public class TextDialogues : MonoBehaviour
{
    public TextManager TextManager;

    [SerializeField] private List<DialogueData> dialogues = new List<DialogueData>();

    private void Start()
    {
        Dialogues();
    }

    void Dialogues()
    {
        foreach (DialogueData data in dialogues)
        {
            // Pasar el color definido en el diccionario
            TextManager.TextRequest(data.startDelay, data.dialogue, data.readTime, data.textColor);
        }
    }

}
