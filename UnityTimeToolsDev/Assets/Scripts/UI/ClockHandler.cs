using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockHandler : MonoBehaviour
{

   // public GameObject targetHandPrefab;

    public GameObject targetHand;


    // Start is called before the first frame update
    void Start()
    {
       /* targetHand = Instantiate(targetHandPrefab);
        targetHand.transform.position = transform.position;
        targetHand.transform.rotation = transform.rotation;
        
        targetHand.transform.SetParent(transform.parent);*/
        targetHand.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTargetHand()
    {
        targetHand.SetActive(true);
      //  Debug.Log("OVER CLOCK");
    }

    public void Test()
    {
        Debug.Log("OVER CLOCK");
    }
    void HideTargetHand()
    {
        targetHand.SetActive(false);
    }


    void DragHand()
    {

    }
    
}
