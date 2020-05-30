using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class DyingPostProcess : MonoBehaviour
{
    Volume volume;
    VolumeComponent comp;
    Vignette vignette;
    public Color endColor;
    Color startColor;
    public float effectDuration = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        // volume.profile.TryGetSettings(out vignette);
        // volume.profile.TryGetSubclassOf<VolumeComponent>(typeof(Vignette), out comp) ;
        // = (Vignette)comp;
        volume.sharedProfile.TryGet<Vignette>(out vignette);
        startColor = vignette.color.value;
      //  Debug.Log("STARTC: " + startColor + "VIGNETTE: " + vignette.name);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DarkenVignette()
    {
        StartCoroutine(Darken());
    }

    IEnumerator Darken()
    {
        float timer = 0;

        while(timer < effectDuration)
        {
            timer += Time.deltaTime;
            vignette.color.value = Color.Lerp(startColor, endColor, timer);
        }
        yield return null;

    }

    private void OnApplicationQuit()
    {
        vignette.color.value = startColor;
    }
}
