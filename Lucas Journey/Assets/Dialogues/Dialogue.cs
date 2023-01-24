using System.Collections;
using UnityEngine;
using TMPro;
public class Dialogue : MonoBehaviour
{
    float typingTime = 0.05f;
    private bool isPlayerInRange;  //Usuario cerca de NPC 
    private bool didDialogueStart; //Si dialogo comenzo
    private int lineIndex;


    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text exclamation;
    [SerializeField, TextArea(4,6)] private string[] dialogueLines;
    


    void Update()
    {
        if(isPlayerInRange && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Entra al if");
            if(!didDialogueStart)
            {
                Debug.Log("Entra al segundo if");
                StartDialogue();
                exclamation.text = "";

            }
            else if(dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else{
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        } 
        
    }
    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if(lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else{
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            exclamation.text = "";
            Time.timeScale = 1f;
        }
    }
    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach(char ch in dialogueLines[lineIndex])
        {
            dialogueText.text +=ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.CompareTag("Player")){
            isPlayerInRange = true;
            Debug.Log("Se puede iniciar un dialogo");
            exclamation.text = "!";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if( collision.gameObject.CompareTag("Player")){
           isPlayerInRange = false;
           Debug.Log("No puede iniciar un dialogo");
            exclamation.text = "";

        }    
    }
}
