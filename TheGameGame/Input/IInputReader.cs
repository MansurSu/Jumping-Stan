﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheGameGame.Input
{

    public interface IInputReader
    {
        Vector2 ReadInput();
    }
}