using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour {

    private Grid grid;
    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .01f;
    public GameObject character;

    private void Start() {
        grid = new Grid(19, 11, 1f, new Vector3(0, 0));

        //HeatMapVisual heatMapVisual = new HeatMapVisual(grid, GetComponent<MeshFilter>());
    }

    private void Update() {
        HandleClickToModifyGrid();
        //HandleHeatMapMouseMove();

        // if (Input.GetMouseButtonDown(1)) {
        //     Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        // }
    }

    private void HandleClickToModifyGrid() {
        if (Input.GetMouseButtonDown(0)) {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(), 1);
            var pos = UtilsClass.GetMouseWorldPosition();
            int x, y;
            grid.GetXY(pos, out x, out y);
            Debug.Log(x);
            Debug.Log(y);
            character.transform.position = grid.GetWorldPosition(x, y) + new Vector3(grid.cellSize, grid.cellSize) * .5f;
        }
    }

    private void HandleHeatMapMouseMove() {
        mouseMoveTimer -= Time.deltaTime;
        if (mouseMoveTimer < 0f) {
            mouseMoveTimer += mouseMoveTimerMax;
            int gridValue = grid.GetValue(UtilsClass.GetMouseWorldPosition());
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), gridValue + 1);
        }
    }
}
