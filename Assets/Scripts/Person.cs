using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State { 
    sick = 0,
    infected = 1,
    recovering = 2,
    healthy = 3
}

public class Person : MonoBehaviour
{
    [SerializeField] int xBound;
    [SerializeField] int yBound;
    [SerializeField] int zBound;
    [SerializeField] private float timeOfTurn;
    [SerializeField] private float chancesOfBeingBorn;
    [SerializeField] private GameObject humanPrefab;
    [SerializeField] private UIManager uiManager;

    private float timeRemaining;
    private Vector2Int position;
    private float speed;
    private Vector2 direction;
    private State healthState;
    private uint age;
    private float immunity;
    private int[] stateTimes;
    private Color[] stateColors;
    private int stateTime;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    static public int turnCounter = 0;
    static public int spawnedCounter = 0;
    

    public static int[] stats = new int[4]; //0-sick 1-infected 2-recovering 3-healthy
    public Vector2Int GetPosition() { return position; }
    public float GetSpeed() { return speed; }
    public Vector2 GetDirection() { return direction; }
    public State GetHealthState() { return healthState; }
    public uint GetAge() { return age; }
    public float GetImmunity() { return immunity; }
    public float GetTimeOfTurn() { return timeOfTurn; }

    public float Immunity
    {
        get { return immunity; }
        set { immunity = value; }
    }
    public int StateTime
    {
        get { return stateTime; }
        set { stateTime = value; }
    }
    public State HealthState
    {
        get { return healthState; }
        set { healthState = value; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        direction = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
    }
    private void Move()
    {
        transform.position += new Vector3(direction.x * speed, direction.y * speed, 0);
        if (transform.position.x > xBound || transform.position.x < -xBound)
            direction.x = -direction.x;
        if (transform.position.y > yBound || transform.position.y < -yBound)
            direction.y = -direction.y;
        
    }
    private void Meeting()
    {
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = Physics2D.AllLayers;
        filter.useTriggers = true;
        Physics2D.OverlapCircle(gameObject.transform.position, circleCollider.radius * 2, filter, hits);
        foreach (var item in hits)
        {
            if (item.gameObject == this.gameObject)
                continue;
            bool reproductionConditionOther = item.gameObject.GetComponent<Person>().GetAge() >= 20 && item.gameObject.GetComponent<Person>().GetAge() <= 40;
            bool reproductionConditionThis = age >= 20 && age <= 40;
            if (Random.Range(0.0f, 1.0f) < chancesOfBeingBorn && reproductionConditionOther && reproductionConditionThis)
            {
                var child = Instantiate(humanPrefab, new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0), Quaternion.identity);
                child.GetComponent<Person>().HealthState = State.healthy;
            }
            switch ((item.gameObject.GetComponent<Person>().GetHealthState(), healthState))
            {
                case (State.infected, State.healthy):
                    if (immunity <= 3)
                        UpdateState(State.infected);
                    break;
                case (State.sick, State.healthy):
                    if (immunity <= 6)
                        UpdateState(State.infected);
                    else if (immunity > 6)
                        immunity -= 3;
                    break;
                case (State.recovering, State.healthy):
                    item.gameObject.GetComponent<Person>().Immunity += 1;
                    break;
                case (State.healthy, State.healthy):
                    if (item.gameObject.GetComponent<Person>().Immunity > immunity)
                        immunity = item.gameObject.GetComponent<Person>().Immunity;
                    else
                        item.gameObject.GetComponent<Person>().Immunity = immunity;
                    break;
                case (State.sick, State.infected):
                    if (immunity <= 6)
                    {
                        UpdateState(State.sick);
                        stateTime = 7;
                    }
                    item.gameObject.GetComponent<Person>().StateTime = 7;
                    break;
                case (State.sick, State.recovering):
                    if (immunity <= 6)
                    {
                        UpdateState(State.infected);
                        stateTime = 2;
                    }
                    break;
                case (State.sick, State.sick):
                    if (item.gameObject.GetComponent<Person>().Immunity < immunity)
                        immunity = item.gameObject.GetComponent<Person>().Immunity;
                    else
                        item.gameObject.GetComponent<Person>().Immunity = immunity;
                    break;
                case (State.infected, State.recovering):
                    immunity -= 1;
                    break;
                case (State.infected, State.infected):
                    immunity -= 1;
                    item.gameObject.GetComponent<Person>().Immunity -= 1;
                    break;
                default:
                    break;
            }
        }
    }

    private void Death()
    {
        stats[(int)healthState]--;
        Destroy(gameObject);
    }

    private void UpdateState(State newState)
    {
        stats[(int)healthState]--;
        healthState = newState;

        spriteRenderer.color = stateColors[(int)healthState];
        stats[(int)healthState]++;
        stateTime = stateTimes[(int)healthState];

        
    }
    void Start()
    {
        chancesOfBeingBorn = chancesOfBeingBorn / 100;
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        timeRemaining = timeOfTurn;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        position = new Vector2Int(Random.Range(0, 100), Random.Range(0, 100));
        speed = Random.Range(0.1f, 1.0f);
        healthState = (State)Random.Range(0, 4);
        direction = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        age = (uint)Random.Range(1, 60);
        immunity = age < 15 || age >= 70 ? Random.Range(1, 3) : age > 40 && age < 70 ? Random.Range(4, 6) : age >= 15 && age < 40 ? Random.Range(7, 10) : 0;
        //0-sick 1-infected 2-recovering 3-healthy
        stateColors = new Color[] { Color.red, Color.yellow, new Color(1, .3f, .1f), Color.green };
        stateTimes = new int[] { 7, 2, 5, 999 };
        spriteRenderer.color = stateColors[(int)healthState];
        stats[(int)healthState]++;
        stateTime = stateTimes[(int)healthState];
        spawnedCounter++;
    }

    private void HandleTurn()
    {
        //0-sick 1-infected 2-recovering 3-healthy
        Meeting();
        age++;
        stateTime--;

        if (healthState == State.infected)
            immunity -= 0.1f;
        else if (healthState == State.sick)
            immunity -= 0.5f;
        else if (healthState == State.recovering)
            immunity += 0.1f;
        else if (healthState == State.healthy)
            immunity += 0.5f;
        if (age < 15 || age >= 70 && immunity > 3)
            immunity = 3;
        if (age > 40 && age < 70 && immunity > 6)
            immunity = 6;
        if (age >= 15 && age < 40 && immunity > 10)
            immunity = 10;

        if(stateTime == 0)
        {
            if (healthState == State.recovering)
                UpdateState(State.healthy);
            else if (healthState == State.infected)
                UpdateState(State.sick);
        }
        if (age > 100 || immunity == 0)
            Death();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = timeOfTurn;
            Move();
            HandleTurn();
            
        }       
    }
}
