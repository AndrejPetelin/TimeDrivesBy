using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    public Slider slider;
    public Image image;
    public Gradient colorGradient;
    public bool allowRegeneration;
    [Tooltip("Rate at which we recover stamina. 0.1 == 10% per second")]
    public float recoverRate;
  //  public float loseHealthRate;
    [Tooltip("Total amount we can rewind, in seconds")]
    public float totalRewindTime;
    
    Coroutine HealthDecrease;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if (allowRegeneration)
        {
            StartCoroutine(RegenerateHealth());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSliderValue(bool decrease)
    {
        if (decrease)
        {
            HealthDecrease = StartCoroutine(DecreaseSliderValue());
        }
        else
        {
            Debug.Log("STOPPING");
            StopCoroutine(HealthDecrease);
        }
    }

    public void ValueChanged()
    {
        image.color =  colorGradient.Evaluate(1 - slider.value);
        if (Mathf.Approximately(slider.value, 0))
        {
            Debug.Log("WE REACHED 0");
        }
    }

    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            slider.value += recoverRate;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator DecreaseSliderValue()
    {
        Debug.Log("DEDREASING");
        while(true)
        {
           // Debug.Log("HERE????");
            slider.value -= 1/totalRewindTime;
            yield return new WaitForSeconds(1);
        }
        
    }


}
