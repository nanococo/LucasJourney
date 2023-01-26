using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    float typingTime = 0.05f;
    private bool isPlayerInRange=true;  //Usuario cerca de NPC 
    private bool inicio_escena = true;  //Usuario cerca de NPC 
    private bool didDialogueStart; //Si dialogo comenzo
    private int lineIndex;

    public string NextScene;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4,6)] private string[] dialogueLines;
    [SerializeField] private Image imagen_Personaje;


    [SerializeField] private Sprite[] personajes;


    void Start()
    {
        Invoke("Update",6f);
    }

    void Update()
    {
        if (inicio_escena)
        {
            dialoguePanel.SetActive(true);
            string[] Bienvenida = new string[] { "Haz click..." };
            dialogueText.text = string.Empty;
            foreach (char ch in Bienvenida[0])
            {
                dialogueText.text += ch;
            }
            inicio_escena = false;
        }
        if(isPlayerInRange && Input.GetButtonDown("Fire1"))
        {
            if(!didDialogueStart)
            {
                StartDialogue();

            }
            else if(dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
                
            }
            else{
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
                imagen_Personaje.sprite = personajes[lineIndex];
            }
        } 
        
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        lineIndex = 0;
        //Time.timeScale = 0f;
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
            Time.timeScale = 1f;
            SceneManager.LoadScene(NextScene);
        }
    }
    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach(char ch in dialogueLines[lineIndex])
        {
            imagen_Personaje.sprite = personajes[lineIndex];
            dialogueText.text +=ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.CompareTag("Player")){
            isPlayerInRange = true;
            Debug.Log("Se puede iniciar un dialogo");
          
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if( collision.gameObject.CompareTag("Player")){
           isPlayerInRange = false;
           Debug.Log("No puede iniciar un dialogo");

        }    
    }
}
