using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonEasy : MonoBehaviour
{
    public void EasyMode()
    {
        PlayerPrefs.SetInt("Difficulty", (int)Game.Difficulty.Easy);
        SceneManager.LoadScene("Scenes/GameScene");
    }
}