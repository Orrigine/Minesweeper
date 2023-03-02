using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonBack : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Menu/SceneMenu");
    }
}
