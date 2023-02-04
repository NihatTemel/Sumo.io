using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    private void Update()
    {
        if(transform.position != Input.mousePosition) 
        
        {

            if (Input.GetMouseButtonDown(0))
            {
                transform.position = Input.mousePosition;
            }

        }


    }
}