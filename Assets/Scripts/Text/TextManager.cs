using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    private AudioSource audioClip;
    private TMP_Text TextBox;

    // Public variable to assign the AudioClip from the Inspector
    public AudioClip typingSound;  // New variable to assign the clip in the Inspector

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

        // Verificar si el audioClip está presente
        if (audioClip == null)
        {
            Debug.LogError("No se ha encontrado un AudioSource en el objeto.");
        }

        // Verificar si el clip ha sido asignado correctamente
        if (typingSound == null)
        {
            Debug.LogWarning("El AudioClip no ha sido asignado en el Inspector.");
        }
        else
        {
            // Si todo está bien, asignar el clip al AudioSource
            audioClip.clip = typingSound;
        }
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
            OnLetter = 0; // Reiniciar posición de letra
            Letters = Stack[0].Dialogue.ToCharArray();

            // Iniciar sonido de escritura si el clip está asignado
            if (audioClip != null && audioClip.clip != null)
            {
                audioClip.loop = true;   // Para que suene mientras escribe
                audioClip.Play();
            }

            StartCoroutine("AddChar", StartDelay);
        }
    }

    int OnLetter = 0;

    // Adding text
    public void NextCharacter()
    {
        if (Letters == null || Stack.Count == 0)
        {
            Debug.LogWarning("No hay diálogo disponible para mostrar.");
            return;
        }

        if (OnLetter >= Letters.Length)
        {
            float readTime = Stack[0].ReadTime;
            audioClip.Stop(); // Detener el sonido al finalizar el texto
            StartCoroutine("RemoveChar", readTime);
            return;
        }

        TextBox.text += Letters[OnLetter];

        OnLetter++;
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
        if (OnLetter > 0)
        {
            TextBox.text = TextBox.text.Remove(OnLetter - 1);
            OnLetter--;
        }

        // Detener el sonido solo cuando el texto esté completamente borrado
        if (OnLetter == 0)
        {
            if (Stack.Count > 0)
            {
                Stack.RemoveAt(0);

                if (Stack.Count > 0)
                {
                    OnLetter = 0;
                    Letters = Stack[0].Dialogue.ToCharArray();
                    StartCoroutine("AddChar", Stack[0].StartDelay);
                }
                else
                {
                    CheckIfDialogueFinished();
                }
            }

            // Detener el sonido después de que el texto se ha borrado por completo
            if (audioClip.isPlaying)
            {
                audioClip.Stop();
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
