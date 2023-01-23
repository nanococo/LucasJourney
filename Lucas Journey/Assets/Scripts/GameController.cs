using System;
using CodeMonkey.Utils;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    //Grid parameters
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellSize;
    
    //Grid Object
    private Grid grid;
    
    //Character Prefabs
    public GridElement[] prefabs = new GridElement[1]; //Only 1 prefab for now

    private GridElement[] players = new GridElement[1]; //Actual players, only 1 for now
    
    
    //Square Prefab
    public GridElement square;

    private void Start() {
        grid = new Grid(gridWidth, gridHeight, cellSize, new Vector3(0, 0));
        
        //Add instantiate to loop to add more players 
        players[0] = Instantiate(prefabs[0]);

        //Move player around grid like this
        grid.MoveGridElementToXY(players[0], 2, 3);

        //Instantiate a square for any purpose
        Instantiate(square);
        grid.MoveGridElementToXY(square, 2, 1);

        //Example: How to access to player stats
        players[0].GetComponent<Character>().Health = 10;
    }

    private void Update() {
        HandleClickToModifyGrid();
    }
    
    //Use this function to capture x and y of mouse click 
    private void HandleClickToModifyGrid() {
        if (Input.GetMouseButtonDown(0)) {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(), 1);
            var pos = UtilsClass.GetMouseWorldPosition();
            
            int x, y;
            grid.GetXY(pos, out x, out y);
            Debug.Log(x);
            Debug.Log(y);
            grid.MoveGridElementToXY(players[0], x, y);
        }
    }
}