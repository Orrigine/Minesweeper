using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Playables;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int x = 4;
        string test = x.ToString();
        var a = GetComponent<TextMeshProUGUI>();
        a.text = test;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
