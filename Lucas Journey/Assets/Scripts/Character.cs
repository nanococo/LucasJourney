using System;

public class Character : GridElement {

    public int Health { get; set; }
    public int Speed { get; set; }
    public int Defense { get; set; }
    public int Attack { get; set; }
    public int SpecialAttack { get; set; }
    public bool UltimateAvailable { get; set; }

    private void Awake() {
        var random = new Random();
        Health = random.Next(5, 10);
        Speed = random.Next(5, 10);
        Defense = random.Next(5, 10);
        Attack = random.Next(5, 10);
        SpecialAttack = random.Next(5, 10);
        UltimateAvailable = false;    
    }

}