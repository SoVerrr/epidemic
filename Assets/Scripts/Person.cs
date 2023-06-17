using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { 
    sick,
    infected,
    recovering,
    healthy
}

public class Person : MonoBehaviour
{
    [SerializeField] int xBound;
    [SerializeField] int yBound;
    private Vector2Int position;
    private float speed;
    private Vector2 direction;
    private State healthState;
    private uint age;
    private float immunity;

    public Vector2Int GetPosition() { return position; }
    public float GetSpeed() { return speed; }
    public Vector2 GetDirection() { return direction; }
    public State GetHealthState() { return healthState; }
    public uint GetAge() { return age; }
    public float GetImmunity() { return immunity; }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("sfjshg");
    }
    private void Move()
    {
        transform.position += new Vector3(direction.x * speed, direction.y * speed, 0);
        if (transform.position.x > xBound || transform.position.x < -xBound)
            direction.x = -direction.x;
        if (transform.position.y > yBound || transform.position.y < -yBound)
            direction.y = -direction.y;
        
    }

    void Start()
    {
        position = new Vector2Int(Random.Range(0, 100), Random.Range(0, 100));
        speed = Random.Range(0.0f, 0.1f);
        healthState = (State)Random.Range(0, 3);
        direction = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        age = (uint)Random.Range(1, 60);
        immunity = age < 15 || age >= 70 ? Random.Range(1, 3) : age > 40 && age < 70 ? Random.Range(4, 6) : age >= 15 && age < 40 ? Random.Range(7, 10) : 0;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
