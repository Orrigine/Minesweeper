using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonHard : MonoBehaviour
{
    public void HardMode()
    {
        PlayerPrefs.SetInt("Difficulty", (int)Game.Difficulty.Hard);
        SceneManager.LoadScene("Scenes/GameScene");
    }
}