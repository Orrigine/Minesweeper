using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonMadness : MonoBehaviour
{
    public void MadnessMode()
    {
        PlayerPrefs.SetInt("Difficulty", (int)Game.Difficulty.Madness);
        SceneManager.LoadScene("Scenes/GameScene");
    }
}