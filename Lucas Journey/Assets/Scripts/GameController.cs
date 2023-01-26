using System.Linq;
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

    public BattleController battleControllerObj;
    
    //Character Prefabs
    public GridElement[] prefabs = new GridElement[2]; //Only 1 prefab for now

    private GridElement[] players = new GridElement[1]; //Actual players, only 1 for now

    private GridElement[] enemies = new GridElement[1];

    private GridElement[,] squares;
    private bool firstClick = true;
    private GridElement selectedChar;
    
    //Square Prefab
    public GridElement square;
    public Color redSquareColor = new Color(0.6603774f, 0.3395337f, 0.3395337f);
    public Color blueSquareColor = new Color(0.3411765f, 0.408549f, 0.6588235f);

    private void Start() {

        battleControllerObj = GameObject.Find("BattleControllerObj").GetComponent<BattleController>();

        grid = new Grid(gridWidth, gridHeight, cellSize, new Vector3(0, 0));
        squares = new GridElement[gridWidth, gridHeight];
        
        //Add instantiate to loop to add more players 
        players[0] = Instantiate(prefabs[0]);

        //Move player around grid like this
        grid.MoveGridElementToXY(players[0], 2, 3);

        enemies[0] = Instantiate(prefabs[1]);
        grid.MoveGridElementToXY(enemies[0], 5, 5);

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
            if (IsEnemyThere(xx,yy)) return;
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
                            newSquare.X = x;
                            newSquare.Y = y;
                            squares[x, y] = newSquare;
                        }

                        
                        //grid.MoveGridElementToXY(newSquare, x, y);
                        // draw tile (x, y)
                    }
                }
            }
        }
        else {
            var canMove = squares[xx, yy] != null;

            if (canMove) {
                var clickedEnemy = GetEnemy(xx, yy);
                if (clickedEnemy!=null) {
                    
                    if (!CheckIfCharacterIsInSurroundingSquares(clickedEnemy)) {
                        var squareToMove = GetValidSquare(clickedEnemy);
                        if (squareToMove!=null) {
                            grid.MoveGridElementToXY(selectedChar, squareToMove.X, squareToMove.Y);    
                        }
                    }
                    else {
                        //Testing Trigger
                        
                        Debug.Log("Adjacent");
                    }
                    //Start attack
                    battleControllerObj.StartBattle(selectedChar.gameObject, clickedEnemy.gameObject, grid);

                }
                else {
                    grid.MoveGridElementToXY(selectedChar, xx, yy);
                }
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
            firstClick = true;
        }
    }

    private bool CheckIfCharacterIsInSurroundingSquares(GridElement clickedEnemy) {

        


        //Down
        if (clickedEnemy.Y - 1 > 0) {
            if (selectedChar.X == clickedEnemy.X && selectedChar.Y == clickedEnemy.Y-1) {
                return true;
            }
        }

        //Left
        if (clickedEnemy.X -1 > 0) {
            if (selectedChar.X == clickedEnemy.X-1 && selectedChar.Y == clickedEnemy.Y) {
                return true;
            }
        }
        
        //Up
        if (clickedEnemy.Y + 1 < gridHeight) {
            if (selectedChar.X == clickedEnemy.X && selectedChar.Y == clickedEnemy.Y+1) {
                return true;
            }
        }
        
        //Right
        if (clickedEnemy.X + 1 < gridWidth) {
            if (selectedChar.X == clickedEnemy.X+1 && selectedChar.Y == clickedEnemy.Y) {
                return true;
            }
        }

        return false;
    }

    private GridElement GetValidSquare(GridElement enemy) {
        //Check 4 directions of enemy 

        //Down first
        if (enemy.Y-1 > 0) {
            var squareDown = squares[enemy.X, enemy.Y-1];
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null) {
                return squareDown;
            }
        }
        
        //Left first
        if (enemy.X-1 > 0) {
            var squareDown = squares[enemy.X-1, enemy.Y];
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null) {
                return squareDown;
            }
        }
        
        //Up first
        if (enemy.Y+1 < gridHeight) {
            var squareDown = squares[enemy.X, enemy.Y+1];
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null) {
                Debug.Log("Up");
                return squareDown;
            }
        }
        
        //Up first
        if (enemy.X+1 < gridWidth) {
            var squareDown = squares[enemy.X+1, enemy.Y];
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null) {
                return squareDown;
            }
        }

        return null;
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

    private bool IsEnemyThere(int x, int y) {
        return enemies.Any(t => t.X == x && t.Y == y);
    }

    private GridElement GetEnemy(int x, int y) {
        return enemies.FirstOrDefault(gridElement => gridElement.X == x && gridElement.Y == y);
    }

    private GridElement GetSquare(int x, int y) {
        for (var i = 0; i < gridHeight; i++) {
            for (var j = 0; j < gridWidth; j++) {
                if (squares[j,i]!=null && squares[j,i].X==x && squares[j,i].Y==y ) {
                    return squares[i, j];
                }
            }
        }
        return null;
    }
}