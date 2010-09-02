using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace Starter3DGame
{
    static class Input
    {
        static TouchCollection m_Touches = TouchPanel.GetState();

        public static void Update()
        {
            m_Touches = TouchPanel.GetState();
        }

        public static TouchCollection touches { get { return m_Touches; } }

    }
}
