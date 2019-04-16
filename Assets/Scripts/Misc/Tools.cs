﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    static class Tools
    {
        public static Vector3 SetZAxisToZero(Vector3 v)
        {
            return v + new Vector3(0, 0, -v.z);
        }
    }
}


