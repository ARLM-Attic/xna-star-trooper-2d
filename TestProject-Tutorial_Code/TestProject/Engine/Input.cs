#region Using directives

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace TestProject
{

    public sealed class Input
    {

        static InputMappings m_InputMappings = new InputMappings();
        //Keyboard States
        static KeyboardState m_KeyStates = Keyboard.GetState();
        static KeyboardState m_OldKeyState;

        //Touch States - Added for reference as Touch is GS 4 only at the moment, to be reenabled once upgraded to 4.
        //static TouchCollection m_Touches = TouchPanel.GetState();

        //Mouse States
        static MouseState m_MouseState = Mouse.GetState();
        static MouseState m_OldMouseState;

        //Gamepad states
        static GamePadState m_GamepadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
        static GamePadState m_OldGamepadState;



        private Input() { }

        public static void Update()
        {
            //Keyboard Update
            m_OldKeyState = m_KeyStates;
            m_KeyStates = Keyboard.GetState();

            //Touch Update - GS 4 only
            //m_Touches = TouchPanel.GetState();

            //Mouse Update
            m_OldMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();

            //Gamepad Update
            m_OldGamepadState = m_GamepadState;
            m_GamepadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);

        }

        public static void Load_Defaults()
        {
            //Single Player settings
            m_InputMappings.MoveUp = Keys.Up;
            m_InputMappings.MoveDown = Keys.Down;
            m_InputMappings.MoveLeft = Keys.Left;
            m_InputMappings.MoveRight = Keys.Right;
            m_InputMappings.Fire = Keys.Space;

            m_InputMappings.AltMoveMethod = MovementMethod.Analogue;

            m_InputMappings.AltMoveControl = Buttons.LeftStick;
            m_InputMappings.AltMoveUp = Buttons.LeftThumbstickUp;
            m_InputMappings.AltMoveDown = Buttons.LeftThumbstickDown;
            m_InputMappings.AltMoveLeft = Buttons.LeftThumbstickLeft;
            m_InputMappings.AltMoveRight = Buttons.LeftThumbstickRight;
            m_InputMappings.AltFire = Buttons.A;

            m_InputMappings.ChangeTrooperFireButton = Keys.F;
            m_InputMappings.SaveSettings = Keys.S;
            
            m_InputMappings.AltChangeTrooperFireButton = Buttons.DPadDown;
            m_InputMappings.AltSaveSettings = Buttons.LeftShoulder;

            m_InputMappings.InvertYLeftStick = true;
            m_InputMappings.InvertYRightStick = false;


        }


        public void SaveSettings()
        {
        }

        public void LoadSettings()
        {
        }


        #region Private Functions
        private static bool IsKeyPressed(Keys key)
        {
            return m_KeyStates.IsKeyDown(key) && m_OldKeyState.IsKeyDown(key);
        }

        private static bool IsKeyTriggered(Keys key)
        {
            return m_KeyStates.IsKeyDown(key) && m_OldKeyState.IsKeyUp(key);
        }

        private static bool IsButtonPressed(Buttons button)
        {
            return m_GamepadState.IsButtonDown(button) && m_OldGamepadState.IsButtonDown(button);
        }

        private static bool IsButtonTriggered(Buttons button)
        {
            return m_GamepadState.IsButtonDown(button) && m_OldGamepadState.IsButtonUp(button);
        }

        private static Vector2 invertY(Vector2 move)
        {
            move.Y = -move.Y;
            return move;
        }

        public static void ChangeSetting(MovementMethod value)
        {
            m_InputMappings.AltMoveMethod = value;
        }

        private static Vector2 IsStickorTriggerMoved(Buttons button)
        {
            switch (button)
            {
                case Buttons.LeftStick:
                    if (m_InputMappings.InvertYLeftStick) return invertY(m_GamepadState.ThumbSticks.Left); else return m_GamepadState.ThumbSticks.Left;
                case Buttons.RightStick:
                    if (m_InputMappings.InvertYRightStick) return invertY(m_GamepadState.ThumbSticks.Right); else return m_GamepadState.ThumbSticks.Right;
                case Buttons.LeftTrigger:
                    return new Vector2(m_GamepadState.Triggers.Left, 0);
                case Buttons.RightTrigger:
                    return new Vector2(m_GamepadState.Triggers.Right, 0);
                default:
                    return Vector2.Zero;
            }
        }

        #endregion


        #region Public Controls

        public static bool MoveUp()
        {
            return IsKeyPressed(m_InputMappings.MoveUp) || IsButtonPressed(m_InputMappings.AltMoveUp);
        }

        public static bool MoveDown()
        {
            return IsKeyPressed(m_InputMappings.MoveDown) || IsButtonPressed(m_InputMappings.AltMoveDown);
        }

        public static bool MoveLeft()
        {
            return IsKeyPressed(m_InputMappings.MoveLeft) || IsButtonPressed(m_InputMappings.AltMoveLeft);
        }

        public static bool MoveRight()
        {
            return IsKeyPressed(m_InputMappings.MoveRight) || IsButtonPressed(m_InputMappings.AltMoveRight);
        }

        public static bool TrooperFired()
        {
            return IsKeyTriggered(m_InputMappings.Fire) || IsButtonTriggered(m_InputMappings.AltFire);
        }

        public static Vector2 TrooperMoveStick()
        {
            return IsStickorTriggerMoved(m_InputMappings.AltMoveControl);
        }

        public static bool ChangeTrooperFireButton()
        {
            return IsKeyTriggered(m_InputMappings.ChangeTrooperFireButton) || IsButtonTriggered(m_InputMappings.AltChangeTrooperFireButton);
        }

        public static bool SaveSettingsKey()
        {
            return IsKeyTriggered(m_InputMappings.SaveSettings) || IsButtonTriggered(m_InputMappings.AltSaveSettings);
        }

        #endregion


        #region Properties
        //GS 4 only
        //public static TouchCollection touches { get { return m_Touches; } }
        public static InputMappings InputMappings { get { return m_InputMappings; } set { m_InputMappings = value; } }
        public static bool SettingsSaved { get { return m_InputMappings.SettingsSaved; } set { m_InputMappings.SettingsSaved = value; } }
        public static MovementMethod MoveMethod {set { m_InputMappings.AltMoveMethod = value; } }
        public static Keys FireButton { set { m_InputMappings.Fire = value; } }
        public static Buttons AltFireButton { set { m_InputMappings.AltFire = value; } }
        #endregion
    }
}
