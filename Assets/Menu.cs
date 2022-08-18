using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private void OnGUI()
    {
        int wide = 100;
        int high = 40;
        Rect rect = new Rect(Screen.width / 2 - wide / 2, 50, wide, high);
        if (GUI.Button(rect, "Level 1"))
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        rect = new Rect(Screen.width / 2 - wide / 2, 150, wide, high);
        if (GUI.Button(rect, "Level 2"))
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);

        rect = new Rect(Screen.width / 2 - wide / 2, 250, wide, high);
        if (GUI.Button(rect, "Level 3"))
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}
