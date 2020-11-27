using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public bool MagniturdeGreaterOrEqual(float a, float b)
   {
        if(a >= b)
        {
            return true;
        }
        if(a <= -b)
        {
            return true;
        }
        return false;
   }

   public float FloatMagnitude(float val)
   {
        if( val > 0)
        {
            return val;
        }
        if( val < 0)
        {
            return -val;
        }
        return 0;
   }
}
