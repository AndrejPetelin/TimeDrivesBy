using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PostProcessHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Volume regularVolume;
    public Volume backwardsVolume;
    public float effectSpeed;
    public bool flipping;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipToBackwards()
    {
         if (backwardsVolume.weight < 1)
         {

             StartCoroutine(Flip(backwardsVolume, regularVolume));
            flipping = true;
         }
       
        
    }

    public void FlipToRegular()
    {
        if (regularVolume.weight < 1)
        {
            StartCoroutine(Flip(regularVolume, backwardsVolume));
            flipping = true;
        }
       
    }

    IEnumerator Flip(Volume increase, Volume decrease)
    {
        Debug.Log("COROUTINE");
        float timer = 0;

        while (timer < effectSpeed)
        {
            timer += Time.deltaTime;
            increase.weight = Mathf.Lerp(0, 1, timer);
            decrease.weight = Mathf.Lerp(1, 0, timer);
            yield return null;
        }
        flipping = false;
        
    }
}
