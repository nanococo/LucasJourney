using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAction : MonoBehaviour
{
    
    private GameObject enemy;
    private GameObject hero;

    [SerializeField]
    private GameObject meleePrefab;



    [SerializeField]
    public Sprite Icon;

    private GameObject currentAttack;
    private GameObject meleeAttack;
    private GameObject specialAttack;
    // Start is called before the first frame update
     void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Ally");
        //enemy = GameObject.FindGameObjectWithTag("Foe");
    }


    public void SelectAttack(string btn)
    {
        

        enemy = GameObject.FindGameObjectWithTag("Foe");

        GameObject victim = tag == "Ally"? enemy: hero;


        
        switch (btn) {
             
        case "melee":
            meleePrefab.GetComponent<AttackScript>().Attack(victim);
            Debug.Log("Melee                  !!!!");
            break;
 
        case "double":
            meleePrefab.GetComponent<AttackScript>().doubleAttack(victim);
            Debug.Log("Double!!");
            break;
        case "heal":
            meleePrefab.GetComponent<AttackScript>().heal();
            Debug.Log("Heal!!");
            break;
        case "guard":
            meleePrefab.GetComponent<AttackScript>().guardAttack(victim);
            Debug.Log("Guard break!!");
            break;

        default:
            Debug.Log("Unknown");
            break;
        }
    }

}
