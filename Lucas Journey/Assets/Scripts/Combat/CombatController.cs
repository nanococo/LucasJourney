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
    private GameObject specialMenu;
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
            if(currentFighterStats.speed>currentEnemyStats.speed){
                fighterStats.Add(currentFighterStats);
            }
            
        }else{
            fighterStats.Add(currentEnemyStats);
            fighterStats.Add(currentFighterStats);
            if(currentFighterStats.speed<currentEnemyStats.speed){
                fighterStats.Add(currentEnemyStats);
            }
        }
        
        

        NextTurn();

    }
    public void turnoffMenu(){
        this.battleMenu.SetActive(false);
    }

    public void turnSpecials(bool state){
        foreach (Transform child in specialMenu.transform){
            child.gameObject.GetComponent<Button>().interactable =state;
        }
        
    }

    public void increaseSpecialCounter(FighterStats currstats){
        
        currstats.specialCounter+=1;
        if(currstats.specialCounter>4){
            currstats.specialCounter=4;
        }
        currstats.updateSpecialIndicator();
    }

    public void checkRemainingEnemies(){

        


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Foe");
        if(enemies.Length<=0){
             GameObject startingPlayer = GameObject.FindGameObjectWithTag("Ally");
            BattleController.charAlly.GetComponent<Character>().SpecialCounter = startingPlayer.GetComponent<FighterStats>().specialCounter;
            BattleController.charAlly.GetComponent<Character>().Health = startingPlayer.GetComponent<FighterStats>().health;
            BattleController.PlayerWon();
            BattleController.inBattle=false;
            BattleController.battlefinished=true;
           
            
            SceneManager.UnloadSceneAsync("Combat");
            
        }else if(GameObject.FindGameObjectWithTag("Ally")==null){
            
            BattleController.charEnemy.GetComponent<Character>().SpecialCounter = enemies[0].GetComponent<FighterStats>().specialCounter;
            BattleController.charEnemy.GetComponent<Character>().Health = enemies[0].GetComponent<FighterStats>().health;
            BattleController.PlayerLost();
            BattleController.inBattle=false;
            BattleController.battlefinished=true;
            SceneManager.UnloadSceneAsync("Combat");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void finishBattle(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Foe");
        GameObject startingPlayer = GameObject.FindGameObjectWithTag("Ally");
        BattleController.charAlly.GetComponent<Character>().SpecialCounter = startingPlayer.GetComponent<FighterStats>().specialCounter;
        BattleController.charAlly.GetComponent<Character>().Health = startingPlayer.GetComponent<FighterStats>().health;
        BattleController.charEnemy.GetComponent<Character>().SpecialCounter = enemies[0].GetComponent<FighterStats>().specialCounter;
        BattleController.charEnemy.GetComponent<Character>().Health = enemies[0].GetComponent<FighterStats>().health;
        BattleController.inBattle=false;
        SceneManager.UnloadSceneAsync("Combat");
       
    }

    public void NextTurn()
    {
       
        damageText.gameObject.SetActive(false);
        
        if(fighterStats.Count<=0){
            finishBattle();
            Debug.Log("Termina la batalla, nadie pierde");
        }else{
            FighterStats currentFighterStats = fighterStats[0];
            fighterStats.Remove(currentFighterStats);
            if(currentFighterStats.GetDead()){
                increaseSpecialCounter(currentFighterStats);
                GameObject  currentUnit = currentFighterStats.gameObject;
                if(currentUnit.tag=="Ally"){
                    

                    this.battleMenu.SetActive(true);
                    if(currentFighterStats.specialCounter>=4){
                        turnSpecials(true);
                    }else{
                        turnSpecials(false);
                    }

                }else{
                    this.battleMenu.SetActive(false);
                    string attackType="melee";
                    if(currentFighterStats.specialCounter>=4){
                        switch(Random.Range(0,4)){
                            case 0:
                                attackType="melee";
                                break;
                            case 1:
                                attackType="double";
                                break;
                            
                            case 2:
                                attackType="heal";
                                break;
                            case 3:
                                attackType="guard";
                                break;
                        }
                    }
                    
                    currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
                }
            }else{
                NextTurn();
            }
        }

        
        

    
    }
}
