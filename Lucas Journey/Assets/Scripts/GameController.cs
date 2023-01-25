using System.Security.Cryptography;
using CodeMonkey.Utils;
using Unity.VisualScripting;
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

    private GridElement[,] squares;
    private bool firstClick = true;
    private GridElement selectedChar;
    
    //Square Prefab
    public GridElement square;
    public Color redSquareColor = new Color(0.6603774f, 0.3395337f, 0.3395337f);
    public Color blueSquareColor = new Color(0.3411765f, 0.408549f, 0.6588235f);

    private void Start() {
        grid = new Grid(gridWidth, gridHeight, cellSize, new Vector3(0, 0));
        squares = new GridElement[gridWidth, gridHeight];
        
        //Add instantiate to loop to add more players 
        players[0] = Instantiate(prefabs[0]);

        //Move player around grid like this
        grid.MoveGridElementToXY(players[0], 2, 3);

        //Example: How to access to player stats
        players[0].GetComponent<Character>().Health = 10;
    }

    private void Update() {
        //HandleClickToModifyGrid();
        //ClickTest();
        RadiusTest();
    }

    private void RadiusTest() {
        if (!Input.GetMouseButtonDown(0)) return;
        
        var pos = UtilsClass.GetMouseWorldPosition();
        int xx, yy;
        grid.GetXY(pos, out xx, out yy);

        
        
        
        if (firstClick) {
            if (grid.Characters[xx, yy] == null) return;
            selectedChar = grid.Characters[xx, yy];
            firstClick = false;

            //var mousePos = grid.GetWorldPosition(xx, yy) + new Vector3(cellSize, cellSize) * .5f;
            
            var worldPosCenterMouse = grid.GetWorldPosition(xx, yy) + new Vector3(cellSize, cellSize) * .5f;
            
            
            for (int y = 0; y < gridHeight; y++) {
                for (int x = 0; x < gridWidth; x++) {
                    var pointWorldPos = grid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
                    if (Inside_circle(worldPosCenterMouse, pointWorldPos, 2)) {
                        if (squares[x,y]==null) {
                            var newSquare = Instantiate(square);
                            var color = grid.Characters[x,y]==null ? blueSquareColor : redSquareColor;
                            newSquare.GetComponent<SpriteRenderer>().material.color = color;
                            newSquare.transform.position = pointWorldPos;
                            squares[x, y] = newSquare;
                        }

                        
                        //grid.MoveGridElementToXY(newSquare, x, y);
                        // draw tile (x, y)
                    }
                }
            }
        }
        else {
            firstClick = true;

            bool canMove = squares[xx, yy] != null;
            if(grid.Characters[xx, yy] != null) return;

            if (canMove) {
                grid.MoveGridElementToXY(selectedChar, xx, yy);
            }
            
            selectedChar = null;
            for (int y = 0; y < gridHeight; y++) {
                for (int x = 0; x < gridWidth; x++) {
                    if (squares[x,y]!=null) {
                        Destroy(squares[x,y].gameObject);
                        squares[x, y] = null;
                    }
                }
            }
        }
    }

    private void ClickTest() {
        if (Input.GetMouseButtonDown(0)) {
            var pos = UtilsClass.GetMouseWorldPosition();
            
            int x, y;
            grid.GetXY(pos, out x, out y);
            GridElement gridElement = null;

            foreach (var t in players) {
                if (t.X == x && t.Y == y) {
                    gridElement = t;
                }
            }

            if (gridElement==null) {
                Debug.Log("Nothing");
            }
            else {
                Debug.Log(gridElement.X);
                Debug.Log(gridElement.Y);
            }
        }
    }

    //Use this function to capture x and y of mouse click 
    private void HandleClickToModifyGrid() {
        if (Input.GetMouseButtonDown(0)) {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(), 1);
            var pos = UtilsClass.GetMouseWorldPosition();
            
            int x, y;
            grid.GetXY(pos, out x, out y);
            grid.MoveGridElementToXY(players[0], x, y);
        }
    }
    
    bool Inside_circle(Vector3 center, Vector3 tile, float radius) {
        float   dx = center.x - tile.x, 
                dy = center.y - tile.y;
        float distance_squared = dx*dx + dy*dy;
        return distance_squared <= radius*radius;
    }
}