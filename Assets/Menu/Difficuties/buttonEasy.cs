using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonEasy : MonoBehaviour
{
    public void EasyMode()
    {
        Debug.Log("Click on Easy");
        SceneManager.LoadScene("Scenes/GameScene");
    }
}