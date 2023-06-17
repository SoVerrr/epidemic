using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider numberOfSpawns;
    [SerializeField] private Slider numberOfTurns;
    [SerializeField] private TextMeshProUGUI numberOfSpawnsVar;
    [SerializeField] private TextMeshProUGUI numberOfTurnsVar;
    [SerializeField] private TextMeshProUGUI numberOfSick;
    [SerializeField] private TextMeshProUGUI numberOfHealthy;
    [SerializeField] private TextMeshProUGUI numberOfInfected;
    [SerializeField] private TextMeshProUGUI numberOfRecovering;
    [SerializeField] private TextMeshProUGUI currentTurn;
    [SerializeField] private TextMeshProUGUI currentSpawned;
    [SerializeField] private GameObject humanSpawner;
    public static int NumberOfTurns;

    public int GetNumberOfSpawns() { return (int)(numberOfSpawns.value * 1000); }
    public int GetNumberOfTurns() { return (int)(numberOfTurns.value * 1000); }
    // Start is called before the first frame update

    private void Awake()
    {
        NumberOfTurns = 1;
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        humanSpawner.SetActive(false);
        Person.turnCounter = 0;
        Person.spawnedCounter = 0;
        Time.timeScale = 1;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //0-sick 1-infected 2-recovering 3-healthy
        NumberOfTurns = GetNumberOfTurns();
        numberOfSpawnsVar.SetText(GetNumberOfSpawns().ToString());
        numberOfTurnsVar.SetText(GetNumberOfTurns().ToString());
        numberOfHealthy.SetText("Number of healthy: " + Person.stats[3].ToString());
        numberOfSick.SetText("Number of sick: " + Person.stats[0].ToString());
        numberOfRecovering.SetText("Number of recovering: " + Person.stats[2].ToString());
        numberOfInfected.SetText("Number of infected: " + Person.stats[1].ToString());
        currentTurn.SetText("Current turn: " + Person.turnCounter);
        currentSpawned.SetText("Total spawned: " + Person.spawnedCounter);
        
    }
}
