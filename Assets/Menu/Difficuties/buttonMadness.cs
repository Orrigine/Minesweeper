using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonMadness : MonoBehaviour
{
    public void MadnessMode()
    {
        Debug.Log("Click on Madness");
        SceneManager.LoadScene("Scenes/GameScene");
    }
}