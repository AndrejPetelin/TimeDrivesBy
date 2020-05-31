using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUI : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
       startSequence = StartCoroutine( ShowTutorial());
    }

    public void OnDisable()
    {
        gameObject.SetActive(false);
    
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
            yield return new WaitForSeconds(duration);
       
        }

        gameObject.SetActive(false);
    }
}
