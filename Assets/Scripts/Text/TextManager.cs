
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    private AudioSource audioClip;
    private TMP_Text TextBox;

    // Speed between characters being added to text
    public float LetterDelay;

    // Speed between characters being removed from text
    public float RemoveDelay;

    char[] Letters;

    struct DialogueRequest
    {
        public float StartDelay;
        public float ReadTime;
        public string Dialogue;
        public Color color;
    }

    List<DialogueRequest> Stack = new List<DialogueRequest>();

    // Boolean to track whether dialogue is finished
    public bool isDialogueFinished = false;

    void Start()
    {
        TextBox = this.gameObject.GetComponent<TextMeshProUGUI>();
        TextBox.text = "";
        audioClip = GetComponent<AudioSource>();
    }

    public void TextRequest(float StartDelay, string Dialogue, float ReadTime, Color color)
    {
        DialogueRequest NewRequest = new DialogueRequest();
        NewRequest.StartDelay = StartDelay;
        NewRequest.Dialogue = Dialogue;
        NewRequest.ReadTime = ReadTime;
        NewRequest.color = color;
        Stack.Add(NewRequest);

        // Si no hay ningún diálogo en curso, empezar a mostrar el nuevo texto
        if (Stack.Count == 1)
        {
            Letters = Stack[0].Dialogue.ToCharArray();
            StartCoroutine("AddChar", StartDelay);
        }
    }

    int OnLetter = 0;

    // Adding text
    public void NextCharacter()
    {
        TextBox.text += Letters[OnLetter];
        OnLetter++;
        if (OnLetter > Letters.Length - 1)
        {
            StartCoroutine("RemoveChar", Stack[0].ReadTime);
            Stack.Remove(Stack[0]);
            CheckIfDialogueFinished(); // Check if dialogue is finished after removing from stack
            return;
        }
        StartCoroutine("AddChar", LetterDelay);
    }

    private IEnumerator AddChar(float t)
    {
        yield return new WaitForSeconds(t);
        NextCharacter();
    }

    // Removing text
    public void NextCharacterRemove()
    {
        audioClip.Stop();
        TextBox.text = TextBox.text.Remove(OnLetter - 1);
        OnLetter--;
        if (OnLetter == 0)
        {
            if (Stack.Count > 0)
            {
                Letters = Stack[0].Dialogue.ToCharArray();
                StartCoroutine("AddChar", Stack[0].StartDelay);
            }
            else
            {
                CheckIfDialogueFinished(); // Check if dialogue is finished when all characters are removed
            }
            return;
        }
        StartCoroutine("RemoveChar", RemoveDelay);
    }

    private IEnumerator RemoveChar(float t)
    {
        yield return new WaitForSeconds(t);
        NextCharacterRemove();
    }

    // Set the dialogue to be finished
    private void CheckIfDialogueFinished()
    {
        if (Stack.Count == 0 && OnLetter == 0)
        {
            isDialogueFinished = true;
        }
    }

    // Call this method when dialogue ends
    public void FinishDialogue()
    {
        CheckIfDialogueFinished();
    }
}