using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BattleController : MonoBehaviour
{
    public GameObject currEnemy;
    public GameObject currAlly;
    public static Grid gridInstance;
    public static GameObject charEnemy;
    public static GameObject charAlly;
    static bool startedByAlly;
    public GameObject battle_bg;
    public GameObject infoPrefab;
    public GameObject enemyPrefab1;
    public string LosingScene;
    public static bool inBattle;

    float sss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void turnOffinBattle(){
        inBattle=false;
        Debug.Log("PAUSA APAGADA");

    }

    
    //public void StartBattle(GameObject startingEnemy){
    public void StartBattle(GameObject starting, GameObject victim, Grid gridParameter){
        //currEnemy = startingEnemy;
        inBattle=true;
        gridInstance = gridParameter; 
        if(starting.GetComponent<Character>().battlePrefab.tag == "Ally"){
            currAlly = starting.GetComponent<Character>().battlePrefab;
            currEnemy = victim.GetComponent<Character>().battlePrefab;
            charAlly = starting;
            charEnemy = victim;
            startedByAlly = true;
        }else{
            
            currAlly = victim.GetComponent<Character>().battlePrefab;
            currEnemy = starting.GetComponent<Character>().battlePrefab;
            charAlly = victim;
            charEnemy =  starting;
            startedByAlly = false;
        }

        Debug.Log("Battle should start....");
        StartCoroutine(TriggerBattle());
    }
    public static void PlayerWon(){
        
        //PlayerController.energy+=energyWon;
        gridInstance.Characters[charEnemy.GetComponent<GridElement>().X,charEnemy.GetComponent<GridElement>().Y]=null;
        Destroy(charEnemy);


    }

    public static void PlayerLost(){
 /*       SceneManager.LoadScene( LosingScene);
 
*/
        gridInstance.Characters[charAlly.GetComponent<GridElement>().X,charAlly.GetComponent<GridElement>().Y]=null;
        Destroy(charAlly);
    }


    public void setFighter(bool isAlly, GameObject newFighter){
        GameObject Position;
        GameObject Info; 
        GameObject Char; 
        if(isAlly){
            Position = GameObject.Find("Ally_Slot");
            Char = charAlly;
            Info = GameObject.Find("Info/Ally");
        }else{
            Position = GameObject.Find("Enemy_Slot");
            Info = GameObject.Find("Info/Foe");
            Char = charEnemy;
        }
        
      
        GameObject Fighter = Instantiate (newFighter, Position.transform) as GameObject;
        Info.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Fighter.GetComponent<FighterStats>().fighterName;
        Info.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Fighter.GetComponent<FighterStats>().speed.ToString();
        Info.transform.Find("ATK").GetComponent<TextMeshProUGUI>().text = Fighter.GetComponent<FighterStats>().attack.ToString();
        Fighter.GetComponent<FighterStats>().specialIndicator = Info.transform.Find("Special").GetComponent<Image>();
        Fighter.GetComponent<FighterStats>().healthFill = Info.transform.Find("Health").GetComponent<TextMeshProUGUI>();
        Info.transform.Find("Health").GetComponent<TextMeshProUGUI>().text = Char.GetComponent<Character>().Health.ToString();
        
        Fighter.GetComponent<FighterStats>().attack = Char.GetComponent<Character>().Attack;
        Fighter.GetComponent<FighterStats>().startHealth = Char.GetComponent<Character>().StartHealth;
        Fighter.GetComponent<FighterStats>().health = Char.GetComponent<Character>().Health;
        Fighter.GetComponent<FighterStats>().defense = Char.GetComponent<Character>().Defense;
        Fighter.GetComponent<FighterStats>().speed = Char.GetComponent<Character>().Speed;
        Fighter.GetComponent<FighterStats>().specialCounter = Char.GetComponent<Character>().SpecialCounter;



        

    }

    private void setScene(){
        GameObject BG = Instantiate (battle_bg) as GameObject;
        BG.transform.SetParent(GameObject.Find("Background_Battles").transform);
        BG.transform.localPosition = Vector3.zero;   
        BG.transform.localScale = new Vector3(1f,1f,1f); 

    }


    public IEnumerator TriggerBattle(){
        
        Scene originalScene = SceneManager.GetActiveScene();
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("Combat", LoadSceneMode.Additive);
        while (!sceneLoad.isDone)
        {
            yield return null;

        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Combat"));
        GameObject.Find("CombatControllerObj").GetComponent<CombatController>().startedByPlayer=startedByAlly;
        //
        setFighter(true, currAlly);
        setFighter(false, currEnemy);

        SceneManager.SetActiveScene(originalScene);
     
        
    }
}
