using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject humanPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int numberOfSpawns;

    private float timeRemaining;
    private float timeOfTurn;
    private void Awake()
    {
        numberOfSpawns = uiManager.GetNumberOfSpawns();
    }
    // Start is called before the first frame update
    void Start()

    {
        timeOfTurn = humanPrefab.GetComponent<Person>().GetTimeOfTurn();
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Instantiate(humanPrefab, new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0), Quaternion.identity);
        }
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
            Person.turnCounter++;
        }
        if (Person.turnCounter > UIManager.NumberOfTurns)
            Time.timeScale = 0;
        if (Person.stats[0] + Person.stats[1] + Person.stats[2] + Person.stats[3] == 0 && Person.turnCounter != 0)
            Time.timeScale = 0;
    }
}
