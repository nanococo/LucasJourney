using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialButton : MonoBehaviour
{
    public GameObject Panel;
    public void hidePanel(){
        Panel.SetActive(false);
    }
    public void showPanel(){
        Panel.SetActive(true);
    }
}
