using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CombatController : MonoBehaviour
{
    public bool startedByPlayer;
    private List<FighterStats> fighterStats;
    private int energyGained=0;
    
    [SerializeField] public TMP_ColorGradient enemyGradient;
    [SerializeField] public TMP_ColorGradient allyGradient;

    [SerializeField]
    private GameObject battleMenu;
    [SerializeField]
    public TextMeshProUGUI damageText;
    

    void Start()
    {
        battleMenu.SetActive(false);
        Invoke("newOrders",3);


        
    }
    private void newOrders(){
        fighterStats = new List<FighterStats>();
        GameObject hero = GameObject.FindGameObjectWithTag("Ally");
        FighterStats currentFighterStats = hero.GetComponent<FighterStats>();
        GameObject foe = GameObject.FindGameObjectWithTag("Foe");
        FighterStats currentEnemyStats = foe.GetComponent<FighterStats>();

        if (startedByPlayer){
            fighterStats.Add(currentFighterStats);
            fighterStats.Add(currentEnemyStats);
            fighterStats.Add(currentFighterStats);
        }else{
            fighterStats.Add(currentEnemyStats);
            fighterStats.Add(currentFighterStats);
            fighterStats.Add(currentEnemyStats);
        }
        
        

        NextTurn();

    }
    

    public void checkRemainingEnemies(){

        


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyBattle");
        if(enemies.Length<=0){
            GameObject startingPlayer = GameObject.FindGameObjectWithTag("Ally");
            BattleController.inBattle=false;
            SceneManager.UnloadSceneAsync("Combat");
            BattleController.PlayerWon(energyGained);
        }else if(GameObject.FindGameObjectWithTag("Ally")==null){
            BattleController.PlayerLost();
            BattleController.inBattle=false;
            SceneManager.UnloadSceneAsync("TurnBased");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void NextTurn()
    {
       
        damageText.gameObject.SetActive(false);
        
        if(fighterStats.Count<=0){
            Debug.Log("Termina la batalla, nadie pierde");
        }else{
            FighterStats currentFighterStats = fighterStats[0];
            fighterStats.Remove(currentFighterStats);
            if(currentFighterStats.GetDead()){
                GameObject  currentUnit = currentFighterStats.gameObject;
                if(currentUnit.tag=="Ally"){
                    this.battleMenu.SetActive(true);

                }else{
                    this.battleMenu.SetActive(false);
                    string attackType= Random.Range(0,2)==1?"melee":"melee";
                    currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
                }
            }else{
                NextTurn();
            }
        }

        
        

    
    }
}
