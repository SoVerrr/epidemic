using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject humanPrefab;
    [SerializeField] private int numberOfSpawns;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Instantiate(humanPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
