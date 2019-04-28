using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    /// <summary>
    /// Simple tools that are somewhat universal
    /// </summary>
    static class Tools
    {
        /// <summary>
        /// Takes a vector, creates a new vector which has the same x and y value but z is set to zero
        /// </summary>
        /// <param name="v">Vector, which z value is to be set to zero</param>
        /// <returns>A new vector which z is set to zero </returns>
        public static Vector3 SetZAxisToZero(Vector3 v)
        {
            return v + new Vector3(0, 0, -v.z);
        }   
        
        /// <summary>
        /// Returns the normal of a vector
        /// </summary>
        /// <param name="original">Original vector</param>
        /// <returns></returns>
        public static Vector3 GetVectorNormal(Vector3 original) {
            return new Vector3(-original.y, original.x, 0);
        }

        /// <summary>
        /// Converts seconds to milliSeconds
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <returns>Returns the time in milliSeconds</returns>
        public static float SecondsToMilliSeconds(float seconds)
        {
            return seconds * 1000;
        }
    }
}


