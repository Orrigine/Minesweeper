using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonMedium : MonoBehaviour
{
    public void MediumMode()
    {
        Debug.Log("Click on Medium");
        SceneManager.LoadScene("Scenes/GameScene");
    }
}