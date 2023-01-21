/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour {

    private Grid grid;
    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .01f;

    private void Start() {
        grid = new Grid(4, 4, 1f, new Vector3(0, 0));

        //HeatMapVisual heatMapVisual = new HeatMapVisual(grid, GetComponent<MeshFilter>());
    }

    private void Update() {
        //HandleClickToModifyGrid();
        //HandleHeatMapMouseMove();

        // if (Input.GetMouseButtonDown(1)) {
        //     Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        // }
    }

    private void HandleClickToModifyGrid() {
        if (Input.GetMouseButtonDown(0)) {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 1);
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
