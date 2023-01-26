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

    [SerializeField]
    public Image specialIndicator;

    public string fighterName;



    [Header("Stats")]

    public float health;
    public float energy;
    public float attack;
    public float defense;
    public float special;
    public float speed;
    public float experience;
    public float specialCounter;


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

    public void KillCharacter(){
        Destroy(gameObject);
        GameController.GetComponent<CombatController>().checkRemainingEnemies();
    }


    public void ReceiveDamage(float damage){
        health = health - damage;
        animator.Play("Hurt");
         Debug.Log("Damage received:"+damage);
        if(health<=0){
            dead = true;
            gameObject.tag="Dead";
            animator.Play("Die");
            Invoke("KillCharacter",1);
            

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

    public void updateSpecialIndicator(){
        switch(specialCounter){
            case 0:
                specialIndicator.color= new Color(0.1037736f, 0.1037736f, 0.1037736f);
                break;
            case 1:
                specialIndicator.color= new Color(0.2960784f, 0.2960784f, 0.2960784f);
                break;
            case 2:
                specialIndicator.color= new Color(0.4679245f, 0.4679245f, 0.4679245f);
                break;
            case 3:
                specialIndicator.color= new Color(0.6849056f, 0.6849056f, 0.6849056f);
                break;
            case 4:
                specialIndicator.color= new Color(1, 1, 1);
                break;
        }
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
