using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField]
    NavigationController[] npcs;

    TimeManager timeManager;


    // Start is called before the first frame update
    void Start()
    {
        timeManager = GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentGameTime = timeManager.gameTime;

        foreach (var npc in npcs)
        {
            npc.MoveTo(currentGameTime);    
        }
    }
}
