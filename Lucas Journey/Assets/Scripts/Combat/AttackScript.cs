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
}
