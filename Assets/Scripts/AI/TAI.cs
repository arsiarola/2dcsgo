﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TAI : SideAI
    {
        public override Side Side { get; protected set; } = Side.Terrorist;
    }
}

