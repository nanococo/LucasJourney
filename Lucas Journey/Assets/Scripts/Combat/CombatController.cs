using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CombatController : MonoBehaviour
{

    private List<FighterStats> fighterStats;
    private int energyGained=0;
    
    [SerializeField]
    private GameObject battleMenu;
    [SerializeField]
    public TextMeshProUGUI damageText;
    

    void Start()
    {
        
        Invoke("newOrders",3);


        
    }
    private void newOrders(){
        fighterStats = new List<FighterStats>();
        GameObject hero = GameObject.FindGameObjectWithTag("Ally");
        FighterStats currentFighterStats = hero.GetComponent<FighterStats>();
        currentFighterStats.CalculateNextTurn(0);
        fighterStats.Add(currentFighterStats);


        GameObject foe = GameObject.FindGameObjectWithTag("Foe");
        FighterStats currentEnemyStats = foe.GetComponent<FighterStats>();
        currentEnemyStats.CalculateNextTurn(0);
        fighterStats.Add(currentEnemyStats);


        //fighterStats.Sort();
        battleMenu.SetActive(false);
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
        //  Debug.Log(fighterStats.ToArray());
        damageText.gameObject.SetActive(false);
        FighterStats currentFighterStats = fighterStats[0];
        fighterStats.Remove(currentFighterStats);

        if(currentFighterStats.GetDead()){
            GameObject  currentUnit = currentFighterStats.gameObject;
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn);
            fighterStats.Add(currentFighterStats);
            fighterStats.Sort();
            if(currentUnit.tag=="Ally"){
                this.battleMenu.SetActive(true);

            }else{
                Debug.Log("Enemy Attack!!");
                this.battleMenu.SetActive(false);
                string attackType= Random.Range(0,2)==1?"melee":"melee";
                currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
            }
        }else{
            NextTurn();
        }
        //if(!GameObject.FindGameObjectWithTag)
        

    
    }
}
