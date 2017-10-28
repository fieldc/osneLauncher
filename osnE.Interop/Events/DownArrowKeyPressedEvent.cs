﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Interop.Events
{
    public class DownArrowKeyPressedEvent : KeyPressedEvent
    {
        public DownArrowKeyPressedEvent(System.Windows.Input.Key pressed)
            : base(pressed)
        {

        }
    }
}
