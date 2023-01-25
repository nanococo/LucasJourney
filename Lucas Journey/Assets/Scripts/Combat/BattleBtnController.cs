using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBtnController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool physical;
    GameObject GameController; 
    private GameObject Hero;
    void Start()
    {
        GameController = GameObject.Find("CombatControllerObj");

        string temp = gameObject.name;
        gameObject.GetComponent<Button>().onClick.AddListener(()=> AttachCallback(temp, gameObject));
        Hero = GameObject.FindGameObjectWithTag("Ally");
    }

    void turnOffIndicator(){
        GameController.GetComponent<CombatController>().turnoffMenu();
    }


    // Update is called once per frame
    private void AttachCallback(string btn, GameObject btnPressed){
        turnOffIndicator();
            if(btn.CompareTo("AttackBtn")== 0){
                Hero.GetComponent<FighterAction>().SelectAttack("melee");

            }
            else if(btn.CompareTo("DoubleBtn")== 0){
                Hero.GetComponent<FighterAction>().SelectAttack("double");

            }
            else if(btn.CompareTo("HealBtn")== 0){
                Hero.GetComponent<FighterAction>().SelectAttack("heal");

            }
            else if(btn.CompareTo("GuardBtn")== 0){
                Hero.GetComponent<FighterAction>().SelectAttack("guard");

            }
            else{
                
                Debug.Log("No sirvió");
            }
    }
}
