using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CaseMine : MonoBehaviour
{
    UnityEvent m_event;
    [SerializeField] GameObject Original;
    [SerializeField] SpriteRenderer sprite;

    bool discover = false;
    bool flag = false;
    // Start is called before the first frame update
    void Start()
    {
        if(m_event == null)
        {
            m_event = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_event!=null && flag == false)
        {
            discover= true;
            sprite.color= Color.blue;

        }
        else if (Input.GetMouseButtonDown(1) && m_event != null && discover == false)
        {
            if (flag == false)
            {
                flag = true;
                sprite.color= Color.red;
            }
            else
            {
                flag = false;
                sprite.color= Color.white;
            }
        }
    }
}
