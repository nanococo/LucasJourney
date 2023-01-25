using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BattleController : MonoBehaviour
{
    static GameObject currEnemy;
    public GameObject battle_bg;
    public Sprite floor_sprite;
    public GameObject infoPrefab;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public string LosingScene;
    public static bool inBattle;
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

    
    public void StartBattle(GameObject startingEnemy){
        currEnemy = startingEnemy;
        StartCoroutine(TriggerBattle());
    }
    public static void PlayerWon(int energyWon){

        //PlayerController.energy+=energyWon;
        Destroy(currEnemy);


    }

    public static void PlayerLost(){
 /*       SceneManager.LoadScene( LosingScene);
*/
    }

    public void setEnemy(int slot, GameObject newEnemy){
        GameObject enemyPosition;
        GameObject enemyInfo; 
        switch (slot) {
            case 2:
                enemyPosition = GameObject.Find("Position2");
                enemyInfo = GameObject.Find("EnemyInfo2");
            break;
            case 3:
                enemyPosition = GameObject.Find("Position3");
                enemyInfo = GameObject.Find("EnemyInfo3");
            break;
            default:
                enemyPosition = GameObject.Find("Position1");
                enemyInfo = GameObject.Find("EnemyInfo1");
            break;

        }
        GameObject Enemy = Instantiate (newEnemy) as GameObject;
        GameObject Info = Instantiate (infoPrefab) as GameObject;
        Enemy.transform.SetParent(enemyPosition.transform);
        Enemy.transform.localPosition = Vector3.zero;   
        /*Enemy.GetComponent<FighterStats>().healthFill = Info.transform.Find("HealthBar/HealthFill").gameObject;
        Enemy.GetComponent<FighterStats>().energyFill = Info.transform.Find("EnergyBar/EnergyFill").gameObject;
        */
        

        Info.transform.SetParent(enemyInfo.transform);
        Info.transform.localPosition = Vector3.zero;   
        Info.transform.localScale = new Vector3(1f,1f,1f);   
        Info.transform.Find("Portrait/EnemyPortrait").GetComponent<Image>().sprite = Enemy.GetComponent<FighterAction>().Icon;
        ///Info.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Enemy.GetComponent<FighterStats>().fighterName;
        

    }
    private void setPlayerStats(){
        GameObject startingPlayer = GameObject.FindGameObjectWithTag("Ally");
        /*
        startingPlayer.GetComponent<FighterStats>().attack = PlayerController.Instance.attack_player;
        startingPlayer.GetComponent<FighterStats>().defense = PlayerController.Instance.defense_player;
        startingPlayer.GetComponent<FighterStats>().speed = PlayerController.Instance.speed_player;
        startingPlayer.GetComponent<FighterStats>().special = PlayerController.Instance.special_player;
        startingPlayer.GetComponent<FighterStats>().defense = PlayerController.Instance.defense_player;
        startingPlayer.GetComponent<FighterStats>().startHealth = PlayerController.Instance.health_player;
        startingPlayer.GetComponent<FighterStats>().startEnergy = PlayerController.Instance.energy_player;
        startingPlayer.GetComponent<FighterStats>().health = PlayerController.Instance.current_health_player;
        startingPlayer.GetComponent<FighterStats>().energy = PlayerController.Instance.current_energy_player;
        

        Transform healthTransform=startingPlayer.GetComponent<FighterStats>().healthFill.GetComponent<RectTransform>();
        Vector2 healthScale = healthTransform.localScale;

        Transform energyTransform = startingPlayer.GetComponent<FighterStats>().energyFill.GetComponent<RectTransform>();
        Vector2 energyScale = energyTransform.localScale;

        ///float xNewHealthScale= healthScale.x * (PlayerController.Instance.current_health_player/PlayerController.Instance.health_player);
           
        healthTransform.localScale=new Vector2(xNewHealthScale, healthScale.y);


        float xNewEnergyScale=energyScale.x*(PlayerController.Instance.current_energy_player/PlayerController.Instance.energy_player);
        energyTransform.localScale=new Vector2(xNewEnergyScale,energyScale.y);
        */
    }
    private void setScene(){
        GameObject.Find("Tile1").GetComponent<SpriteRenderer>().sprite = floor_sprite;
        GameObject.Find("Tile2").GetComponent<SpriteRenderer>().sprite = floor_sprite;
        GameObject BG = Instantiate (battle_bg) as GameObject;
        BG.transform.SetParent(GameObject.Find("Background_Battles").transform);
        BG.transform.localPosition = Vector3.zero;   
        BG.transform.localScale = new Vector3(1f,1f,1f); 

    }


    public IEnumerator TriggerBattle(){
        
        Scene originalScene = SceneManager.GetActiveScene();
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("TurnBased", LoadSceneMode.Additive);
        while (!sceneLoad.isDone)
        {
            yield return null;

        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TurnBased"));
        ///setEnemy(1, currEnemy.GetComponent<BattleDescriptor>().Enemy1);

        
        setPlayerStats();
        setScene();
        
        SceneManager.SetActiveScene(originalScene);
        
    }
}
