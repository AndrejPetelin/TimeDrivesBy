using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PostProcessHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Volume regularVolume;
    public Volume backwardsVolume;
    public Volume dyingVolume;
    //Volume currentVolume;
    public float effectSpeed;
    public float dyingSpeed = 3;
    public bool flipping;
    bool dying;
    public bool dead; 
    DyingPostProcess dyingPostProc;
    List<Coroutine> running;
    void Start()
    {
        dead = false;
        regularVolume.weight = 1;
        backwardsVolume.weight = 0;
        dyingVolume.weight = 0;
        dyingPostProc = dyingVolume.gameObject.GetComponent<DyingPostProcess>();
        running = new List<Coroutine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipToBackwards()
    {
        if (dying) return;
         if (backwardsVolume.weight < 1)
         {

            running.Add( StartCoroutine(Flip(backwardsVolume, regularVolume)));
            flipping = true;
         }
       
        
    }

    public void FlipToRegular()
    {
        if (dying) return;
        if (regularVolume.weight < 1)
        {
            running.Add(StartCoroutine(Flip(regularVolume, backwardsVolume)));
            flipping = true;
        }
       
    }

    public void FlipToDying()
    {
        StartCoroutine(Flip(dyingVolume, regularVolume, backwardsVolume));
    }

    IEnumerator Flip(Volume increase, Volume decrease)
    {
       // Debug.Log("COROUTINE");
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

    IEnumerator Flip(Volume increase, Volume decrease, Volume decrease2)
    {
        // Debug.Log("COROUTINE2");
        dying = true;
        foreach (var cor in running) StopCoroutine(cor);
        float timer = 0;
        float decreaseVal = decrease.weight;
        float decrease2Val = decrease2.weight;

        while (timer < effectSpeed)
        {
            timer += Time.deltaTime;
            increase.weight = Mathf.Lerp(0, 1, timer);
            decrease.weight = Mathf.Lerp(decreaseVal, 0, timer);
            decrease2.weight = Mathf.Lerp(decrease2Val, 0, timer);
            yield return null;
        }
        dyingPostProc.DarkenVignette();
        dying = false;
        // flipping = false;

        yield return new WaitForSeconds(dyingSpeed/2);

        if (! dead)
        {
            float timer2 = 0;

            while (timer2 < dyingSpeed / 2)
            {
                timer2 += Time.deltaTime;
                dyingVolume.weight = Mathf.Lerp(1, 0, timer2);
                regularVolume.weight = Mathf.Lerp(0, 1, timer2);
                backwardsVolume.weight = Mathf.Lerp(1, 0, timer2);
                yield return null;
            }
        }
        
      /*  dyingVolume.weight = 0;
        regularVolume.weight = 1;
        backwardsVolume.weight = 0;*/

    }
}
