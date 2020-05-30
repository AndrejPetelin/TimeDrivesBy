using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public void Test(BaseEventData data)
    {
        Debug.Log("OVER CLOCK");
        Debug.Log(data.currentInputModule.input.mousePosition );
    }
    void HideTargetHand()
    {
        targetHand.SetActive(false);
    }


    void DragHand()
    {

    }
    
}
