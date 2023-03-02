using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonMedium : MonoBehaviour
{    


    public void MediumMode()
    {
        PlayerPrefs.SetInt("Difficulty", (int)Game.Difficulty.Medium);
        SceneManager.LoadScene("Scenes/GameScene");
    }
}