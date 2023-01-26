using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour {
    
    //Grid parameters
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellSize;
    
    //Grid Object
    private Grid grid;

    public BattleController battleControllerObj;
    
    //Character Prefabs
    private GridElement[] players; //Actual players, only 1 for now

    private GridElement[] enemies;

    public GridElement[] playersPrefabs = new GridElement[3];
    public Vector2[] playersLocations = new Vector2[3];
    
    public GridElement[] enemiesPrefabs = new GridElement[1]; //Only 1 prefab for now
    public Vector2[] enemiesLocations;

    private GridElement[,] squares;
    private bool firstClick = true;
    private GridElement selectedChar;

    bool AllyTurn;
    bool EnemyIsMoving;
    
    //Square Prefab
    public GridElement square;
    public Color redSquareColor = new Color(0.6603774f, 0.3395337f, 0.3395337f);
    public Color blueSquareColor = new Color(0.3411765f, 0.408549f, 0.6588235f);
    
    //PathFinding
    private Pathfinding pathfinding;
    
    //Un-Walkable Spaces
    public Vector2[] unWalkablePositions;

    private void Start() {
        AllyTurn=true;

        battleControllerObj = GameObject.Find("BattleControllerObj").GetComponent<BattleController>();

        grid = new Grid(gridWidth, gridHeight, cellSize, new Vector3(0, 0));
        squares = new GridElement[gridWidth, gridHeight];
        players = new GridElement[playersPrefabs.Length];
        enemies = new GridElement[enemiesLocations.Length];
        
        
        //Start Players
        for (var i = 0; i < playersPrefabs.Length; i++) {
            players[i] = Instantiate(playersPrefabs[i]);
            grid.MoveGridElementToXY(players[i], (int) playersLocations[i].x,(int) playersLocations[i].y);
        }
        
        //Start Enemies
        for (var i = 0; i < enemiesLocations.Length; i++) {
            enemies[i] = Instantiate(enemiesPrefabs[0]);
            grid.MoveGridElementToXY(enemies[i], (int) enemiesLocations[i].x, (int) enemiesLocations[i].y);
        }
        
        //Set UnWalkable Spaces
        foreach (var unWalkablePosition in unWalkablePositions) {
            grid.unWalkableGrid[(int) unWalkablePosition.x, (int) unWalkablePosition.y] = true;
        }

        pathfinding = new Pathfinding(grid);
    }

    private void Update() {
        //HandleClickToModifyGrid();
        //ClickTest();
        if(AllyTurn){

        
            RadiusTest();
        }else if(!EnemyIsMoving){
            EnemyIsMoving=true;

        }
        //PathfindingTest();
    }
    public void enemyTurn(){
        foreach(GridElement currEnemy in enemies){
            GridElement currAlly =  GetClosestTarget(currEnemy);
            EnemyMoveOnTurn(currAlly, currEnemy );
        }
    }

    public void TurnUsed(GridElement selectedChar){
        selectedChar.GetComponent<SpriteRenderer>().color = new Color(0.4f,0.4f,0.4f);
        selectedChar.tag = "PlayerStill";
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        if(enemies.Length<=0){
            //Change turn
            AllyTurn = false;
            Debug.Log("Change Turn");
            EnemyIsMoving=false;
        }
    }
    private void PathfindingTest() {
        if (!Input.GetMouseButtonDown(0)) return;
        
        var pos = UtilsClass.GetMouseWorldPosition();
        int xx, yy;
        grid.GetXY(pos, out xx, out yy);
        if (firstClick) {
            if (grid.Characters[xx, yy] == null) return;
            
            selectedChar = grid.Characters[xx, yy];
            firstClick = false;
        }
        else {
            var mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            grid.GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(selectedChar.X, selectedChar.Y, x, y);
            if (path != null) {
                foreach (var pathNode in path) {
                    Debug.Log(pathNode.x);
                    Debug.Log(pathNode.y);
                }
            }
            else {
                Debug.Log("Path is null");
            }
        }
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
            if (selectedChar.tag.CompareTo("PlayerStill")==0) return;
            firstClick = false;

            //var mousePos = grid.GetWorldPosition(xx, yy) + new Vector3(cellSize, cellSize) * .5f;
            
            var worldPosCenterMouse = grid.GetWorldPosition(xx, yy) + new Vector3(cellSize, cellSize) * .5f;
            
            for (int y = 0; y < gridHeight; y++) {
                for (int x = 0; x < gridWidth; x++) {
                    var pointWorldPos = grid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
                    if (Inside_circle(worldPosCenterMouse, pointWorldPos, 2)) {
                        if (squares[x,y]==null) {
                            var newSquare = Instantiate(square);
                            var color = grid.Characters[x,y]==null && grid.unWalkableGrid[x,y]==false ? blueSquareColor : redSquareColor;

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
                    //Start attack
                    battleControllerObj.StartBattle(selectedChar.gameObject, clickedEnemy.gameObject, grid);

                }
                else {
                    if (grid.Characters[xx, yy] == null && grid.unWalkableGrid[xx,yy]==false) {
                        grid.MoveGridElementToXY(selectedChar, xx, yy);
                        TurnUsed(selectedChar);
                    }

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
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null && grid.unWalkableGrid[enemy.X, enemy.Y-1] == false) {
                return squareDown;
            }
        }
        
        //Left first
        if (enemy.X-1 > 0) {
            var squareDown = squares[enemy.X-1, enemy.Y];
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null && grid.unWalkableGrid[enemy.X-1, enemy.Y] == false) {
                return squareDown;
            }
        }
        
        //Up first
        if (enemy.Y+1 < gridHeight) {
            var squareDown = squares[enemy.X, enemy.Y+1];
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null && grid.unWalkableGrid[enemy.X, enemy.Y+1] == false) {
                return squareDown;
            }
        }
        
        //Up first
        if (enemy.X+1 < gridWidth) {
            var squareDown = squares[enemy.X+1, enemy.Y];
            if (squareDown != null && grid.Characters[squareDown.X, squareDown.Y]==null && grid.unWalkableGrid[enemy.X+1, enemy.Y] == false) {
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

    public GridElement GetClosestTarget(GridElement origin) {
        var minDistance = float.MinValue;
        GridElement closestPlayer = null;
        foreach (var player in players) {
            if (player == null) continue;
            if (!player.GetComponent<Character>().isAlive) continue;

            var distance = GetDistance(player, origin);
            if (!(distance < minDistance)) continue;
            
            minDistance = distance;
            closestPlayer = player;
        }

        return closestPlayer;
    }
    private void EnemyMoveOnTurn(GridElement target, GridElement origin) {
        pathfinding = new Pathfinding(grid);
        var path = pathfinding.FindPath(origin.X, origin.Y, target.X, target.Y);
        for (var i = 0; i < path.Count-1; i++) {
            grid.MoveGridElementToXY(origin, path[i].x, path[i].y);    
        }

        if (GetDistance(target, origin) <= 1.0f) {
            battleControllerObj.StartBattle(origin.gameObject, target.gameObject, grid);
        }
    }

    private float GetDistance(GridElement target, GridElement origin) {
        return Mathf.Pow(Mathf.Pow(target.X - origin.X, 2) + Mathf.Pow(target.Y - origin.Y, 2), 1 / 2f);
    }
}