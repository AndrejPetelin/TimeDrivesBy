using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Tooltip("Time we're setting in game, should not be changed directly!")]
    float _gameTime = 0;

    [SerializeField, Tooltip("Actual time in game, should not be changed directly!")]
    float _targetTime = 0;

    public float gameTime
    {
        get { return _gameTime; }
        set { _targetTime = value; }
    }
    public StaminaBar staminaBar;
    public float coolDown;
    bool backwards;
    bool forward = true;


    [SerializeField,
     Tooltip("How many seconds progress in 1 second of real time. Gets multiplied with FixedDeltaTime " +
             "in FixedUpdate(). Make sure that the fixed timestep is small enough to accomodate correct " +
             "collision detection, we don't want cars passing through each other!)")]
    float timeRate = 1;
    float sign;
    
    public PostProcessHandler postProcHandler; 

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /**
     * Using FixedUpdate to have a linear passing of time, we can change this if needed
     */
    void FixedUpdate()
    {
        if (WithinRange(_gameTime, _targetTime, timeRate * Time.fixedDeltaTime))
        {
            _gameTime = _targetTime;
        }
        else
        {
            // figure out whether we have to add or subtract the timestep to _gameTime
            sign = Mathf.Sign(_targetTime - _gameTime);
            _gameTime += sign * timeRate * Time.fixedDeltaTime;
            
        }

        if (postProcHandler.flipping) return;
        
        if (gameTime > _targetTime + coolDown )
        {
            if (! backwards)
            {
                postProcHandler.FlipToBackwards();
                // postProcHandler.flipping = true;
                staminaBar.SetSliderValue(true);
                backwards = true;
                forward = false;
            }
           
        }
        else
        {
            if (! forward)
            {
                postProcHandler.FlipToRegular();
                staminaBar.SetSliderValue(false);
                // postProcHandler.flipping = true;
                backwards = false;
                forward = true;
            }

        }
        
    }


    bool WithinRange(float a, float b, float r)
    {
        return Mathf.Abs(b - a) <= r;
    }
}
