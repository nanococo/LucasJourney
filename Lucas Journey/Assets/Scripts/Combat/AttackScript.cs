using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
        // Start is called before the first frame update
    public GameObject owner;

    [SerializeField]
    private string animationName;



    [SerializeField]
    private float minAttackMultiplier;
    [SerializeField]
    private float maxAttackMultiplier;
    [SerializeField]
    private float minDefenseMultiplier;
    [SerializeField]
    private float maxDefenseMultiplier;
    private FighterStats attackerStats;
    private FighterStats targetStats;
    
    private float damage=0.0f;


    // Start is called before the first frame update
    public void Attack(GameObject victim)
    {
        attackerStats=owner.GetComponent<FighterStats>();
        targetStats=victim.GetComponent<FighterStats>();
        
        
        float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        
        damage=multiplier*attackerStats.attack;
        float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
        damage = Mathf.Max(0,damage-(defenseMultiplier*targetStats.defense));
        owner.GetComponent<Animator>().Play(animationName);
        targetStats.ReceiveDamage(Mathf.CeilToInt(damage));

        
    }

    public void doubleAttack(GameObject victim)
    {
        attackerStats=owner.GetComponent<FighterStats>();
        targetStats=victim.GetComponent<FighterStats>();
        
        attackerStats.specialCounter =0;
        attackerStats.updateSpecialIndicator();
        
        float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        
        damage=(multiplier*attackerStats.attack); // double damage
        float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
        damage = Mathf.Max(0,damage-(defenseMultiplier*targetStats.defense));
        owner.GetComponent<Animator>().Play(animationName);
        targetStats.ReceiveDamage(Mathf.CeilToInt(2*damage));

        
    }
    public void guardAttack(GameObject victim)
    {
        attackerStats=owner.GetComponent<FighterStats>();
        targetStats=victim.GetComponent<FighterStats>();
        
        attackerStats.specialCounter =0;
        attackerStats.updateSpecialIndicator();
        float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        
        damage=multiplier*attackerStats.attack; // guardbreak damage
        Debug.Log("True damage   "+ damage);
        damage = Mathf.Max(0,damage);
        owner.GetComponent<Animator>().Play(animationName);
        targetStats.ReceiveDamage(Mathf.CeilToInt(damage));

        
    }

    public void heal()
    {
        attackerStats=owner.GetComponent<FighterStats>();
        attackerStats.specialCounter =0;
        attackerStats.updateSpecialIndicator();
        attackerStats=owner.GetComponent<FighterStats>();
        owner.GetComponent<FighterStats>().updateHealth();
        owner.GetComponent<Animator>().Play("Heal");
        targetStats.ReceiveDamage(0);

        
    }
}
