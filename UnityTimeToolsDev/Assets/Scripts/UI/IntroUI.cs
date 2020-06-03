using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntroUI : MonoBehaviour
{
    Func<bool>[] funcArray = new Func<bool>[5];
    /* public GameObject Click;
     public GameObject MoveClock;
     public GameObject rewind;
     public GameObject stamina;
     public GameObject timebombs;
     public GameObject timebombs2;
     public GameObject arrow;
     public GameObject time;*/
    public GameObject[] texts;
   /* public GameObject[] arrows;
    public GameObject staminaArrow;
    public GameObject arrowArrow;
    public GameObject bombsArrow;
    public GameObject clockArrow;*/
    public float duration;
    Coroutine startSequence;
    public PlayerManager playerManager;
    public ClockHandler clock;
    public PlayerController playerController;
    public LevelEnding ending;
    public GameObject particlesStart;
    public GameObject particlesBomb;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
       startSequence = StartCoroutine( ShowTutorial());
        funcArray[0] = ClickedOnGround;
        funcArray[1] = MovedClockHand;
        funcArray[2] = PlayerCrashed;
        funcArray[3] = PlayerRespawned;
        funcArray[4] = TimeBombPlaced;
        particlesStart.SetActive(true);
    }

    private void Update()
    {
        if (ending.DestinationReached())
        {
            OnDisable();
        }
    }

    bool ClickedOnGround()
    {
        Debug.Log("CLICKED");
       // particlesStart.SetActive(false);
        return playerManager.placed;
    }

    bool MovedClockHand()
    {
        Debug.Log("DRAGGED");
        return clock.dragging;
    }

    bool PlayerCrashed()
    {
        Debug.Log("CRASHING");
        return playerController.crashing;
    }

    bool PlayerRespawned()
    {
        Debug.Log("RESPAWNED: " + playerManager.respawned);
        return playerManager.respawned;
    }

    bool TimeBombPlaced()
    {
        Debug.Log("TB");
      //  particlesBomb.SetActive(false);
        return playerManager.timeBombPlaced;
    }

    public void OnDisable()
    {
        // gameObject.SetActive(false);
        for (int i = 0; i < texts.Length; ++i)
        {
            texts[i].SetActive(false);
            button.SetActive(false);
        }
        if (startSequence != null)
            StopCoroutine(startSequence);
    
    }

    IEnumerator ShowTutorial()
    {
        for (int i = 0; i < texts.Length; ++i)
        {
            if (i > 0)
            {
                texts[i-1].SetActive(false);
                
            }
            texts[i].SetActive(true);
            Func<bool> currFunc = GetNewFunction(i);
            if (currFunc != null)
            {
                while (currFunc() == false)
                    yield return new WaitForSeconds(duration);
            }
            else
            {
                yield return new WaitForSeconds(duration *3);
            }

            particlesBomb.SetActive(false);
            particlesStart.SetActive(false);
        }

        for (int i = 0; i < texts.Length; ++i)
        {
            texts[i].SetActive(false);
        }
        button.SetActive(false);

      //  gameObject.SetActive(false);
    }


    Func<bool> GetNewFunction(int i)
    {
        switch (i)
        {
            case 0:
                particlesStart.SetActive(true);
                return ClickedOnGround;
           // case 1:
              //  return MovedClockHand;
            case 1:
                return PlayerCrashed;
            case 2:
                return PlayerRespawned;
            case 4:
                particlesBomb.SetActive(true);
                return TimeBombPlaced;
                
            default:
                return null;
        }
    }
}
