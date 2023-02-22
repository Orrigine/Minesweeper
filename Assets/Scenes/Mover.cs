using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 0.1f;
    void Start()
    {
        transform.position = new Vector2(0, -2);
        GetComponent<SpriteRenderer>().color = Color.green;
    }



    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x - speed + Time.deltaTime, transform.position.y);
        // GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
        // m = m + m/s + s
    }
}
