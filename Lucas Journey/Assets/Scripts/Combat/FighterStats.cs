using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
using TMPro;


public class FighterStats : MonoBehaviour, IComparable
{
    private bool dead = true;
     [SerializeField]
    private Animator animator;

    [SerializeField]
    public TextMeshProUGUI healthFill;

    public string fighterName;



    [Header("Stats")]

    public float health;
    public float energy;
    public float attack;
    public float defense;
    public float special;
    public float speed;
    public float experience;


    public float startHealth;
    public float startEnergy;
    
    [HideInInspector]
    public int nextActTurn;
    //Resize bars

    

    public Vector2 healthScale;
    public Vector2 energyScale;

    private float xNewHealthScale;
    private float xNewEnergyScale;

    public GameObject GameController;

    void Start() {


        startHealth=health;
        startEnergy=energy;

        GameController = GameObject.Find("CombatControllerObj");

        
    }
    public void aumentar_health(float puntos)
    {
        health += puntos;
       Debug.Log("Player health: " + health);
       

    }
    public void ReceiveDamage(float damage){
        health = health - damage;
        animator.Play("Hurt");
         Debug.Log("Damage received:"+damage);
        if(health<=0){
            dead = true;
            gameObject.tag="Dead";
            Destroy(gameObject);
            GameController.GetComponent<CombatController>().checkRemainingEnemies();

        }else if(damage>0){
            healthFill.text = health.ToString();


        }
        if(gameObject.tag.CompareTo("Ally")==0){
            
            GameController.GetComponent<CombatController>().damageText.colorGradientPreset=GameController.GetComponent<CombatController>().enemyGradient;
        }else{
            GameController.GetComponent<CombatController>().damageText.colorGradientPreset=GameController.GetComponent<CombatController>().allyGradient;
        }
        GameController.GetComponent<CombatController>().damageText.gameObject.SetActive(true);
        
        
        GameController.GetComponent<CombatController>().damageText.text=damage.ToString();
        Invoke("continueGame",2);
    }
    /*
    public void updateEnergyFill(float cost){
        if(cost>1){
            energy=energy-cost;
            xNewEnergyScale=energyScale.x*(energy/startEnergy);
    
        }
    }
    */
    public void updateHealth(){
        float addition = (UnityEngine.Random.Range(0.3f, 0.5f))*startHealth;
        health += addition;
        if(health>startHealth){
            health=startHealth;
        }
    
        healthFill.text = health.ToString();
    }
    void continueGame(){
        GameObject.Find("CombatControllerObj").GetComponent<CombatController>().NextTurn();
    }

    public bool GetDead(){
        return dead;
    }

    public void CalculateNextTurn(int currentTurn){
        nextActTurn = currentTurn + Mathf.CeilToInt(100f/speed);
    }

    public int CompareTo(object otherStat){
        int nex= nextActTurn.CompareTo(((FighterStats)otherStat).nextActTurn);
        return nex;
        
    }

}
