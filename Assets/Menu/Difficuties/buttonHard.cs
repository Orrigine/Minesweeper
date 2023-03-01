using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonHard : MonoBehaviour
{
    public void HardMode()
    {
        Debug.Log("Click on Easy");
        SceneManager.LoadScene("Scenes/GameScene");
    }
}