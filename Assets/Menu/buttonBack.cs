using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonBack : MonoBehaviour
{
    public void Back()
    {
        Debug.Log("oui");
        SceneManager.LoadScene("Menu/SceneMenu");
    }
}
