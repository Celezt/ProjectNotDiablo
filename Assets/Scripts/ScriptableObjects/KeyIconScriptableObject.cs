using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using MyBox;

[CreateAssetMenu(fileName = "KeyIconAssets", menuName = "Custom/KeyIconAssets")]
public class KeyIconScriptableObject : ScriptableObject
{
    public KeyIcons Xbox;
    public KeyIcons Ps4;
    public KeyIcons Pc;

    [Serializable]
    public struct KeyIcons
    {
        public Sprite ButtonSouth;
        public Sprite ButtonNorth;
        public Sprite ButtonEast;
        public Sprite ButtonWest;
        public Sprite StartButton;
        public Sprite SelectButton;
        public Sprite LeftTrigger;
        public Sprite RightTrigger;
        public Sprite LeftShoulder;
        public Sprite RightShoulder;
        public Sprite Dpad;
        public Sprite DpadUp;
        public Sprite DpadDown;
        public Sprite DpadLeft;
        public Sprite DpadRight;
        public Sprite LeftStick;
        public Sprite RightStick;
        public Sprite LeftStickPress;
        public Sprite RightStickPress;
        public Sprite Empty;

        public Sprite GetSprite(string controlPath)
        {
            // From the input system, we get the path of the control on device. So we can just
            // map from that to the sprites we have for gamepads.
            switch (controlPath)
            {
                case "buttonSouth": return ButtonSouth;
                case "buttonNorth": return ButtonNorth;
                case "buttonEast": return ButtonEast;
                case "buttonWest": return ButtonWest;
                case "start": return StartButton;
                case "select": return SelectButton;
                case "leftTrigger": return LeftTrigger;
                case "rightTrigger": return RightTrigger;
                case "leftShoulder": return LeftShoulder;
                case "rightShoulder": return RightShoulder;
                case "dpad": return Dpad;
                case "dpad/up": return DpadUp;
                case "dpad/down": return DpadDown;
                case "dpad/left": return DpadLeft;
                case "dpad/right": return DpadRight;
                case "leftStick": return LeftStick;
                case "rightStick": return RightStick;
                case "leftStickPress": return LeftStickPress;
                case "rightStickPress": return RightStickPress;
                default: return Empty;
            }
        }

        public Sprite GetSprite(Keys key)
        {
            // From the input system, we get the path of the control on device. So we can just
            // map from that to the sprites we have for gamepads.
            switch (key)
            {
                case Keys.ButtonSouth: return ButtonSouth;
                case Keys.ButtonNorth: return ButtonNorth;
                case Keys.ButtonEast: return ButtonEast;
                case Keys.ButtonWest: return ButtonWest;
                case Keys.StartButton: return StartButton;
                case Keys.SelectButton: return SelectButton;
                case Keys.LeftTrigger: return LeftTrigger;
                case Keys.RightTrigger: return RightTrigger;
                case Keys.LeftShoulder: return LeftShoulder;
                case Keys.RightShoulder: return RightShoulder;
                case Keys.Dpad: return Dpad;
                case Keys.DpadUp: return DpadUp;
                case Keys.DpadDown: return DpadDown;
                case Keys.DpadLeft: return DpadLeft;
                case Keys.DpadRight: return DpadRight;
                case Keys.LeftStick: return LeftStick;
                case Keys.RightStick: return RightStick;
                case Keys.LeftStickPress: return LeftStickPress;
                case Keys.RightStickPress: return RightStickPress;
                default: return Empty;
            }
        }

        public enum Keys
        {
            ButtonSouth,
            ButtonNorth,
            ButtonEast,
            ButtonWest,
            StartButton,
            SelectButton,
            LeftTrigger,
            RightTrigger,
            LeftShoulder,
            RightShoulder,
            Dpad,
            DpadUp,
            DpadDown,
            DpadLeft,
            DpadRight,
            LeftStick,
            RightStick,
            LeftStickPress,
            RightStickPress,
        }
    }
}
