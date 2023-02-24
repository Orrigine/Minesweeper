using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonPlay : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("oui");
        SceneManager.LoadScene("Menu/Difficulté");
    }   
}