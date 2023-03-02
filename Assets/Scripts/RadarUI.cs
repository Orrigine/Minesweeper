using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Playables;
using UnityEngine.UI;

public class RadarUI : MonoBehaviour
{
    Game game;
    // Start is called before the first frame update
    void Start()
    {
        if (game == null)
        {
            game = FindObjectOfType<Game>();
            string value = "Radars : " + game.radarInitValue.ToString();
            var radar = GetComponent<TextMeshProUGUI>();
            radar.text = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var radar = GetComponent<TextMeshProUGUI>();
        string value = "Radars : " + game.radarActualValue.ToString();
        radar.text = value;
    }
}