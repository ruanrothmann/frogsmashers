using UnityEngine;
using System.Collections;
using UnityEngine;

namespace FreeLives
{
    public static class InputReader
    {
        public enum Device
        {
            Keyboard1, Keyboard2, Gamepad1, Gamepad2, Gamepad3, Gamepad4
        }

        static KeyCode kb1Left = KeyCode.LeftArrow;
        static KeyCode kb1Right = KeyCode.RightArrow;
        static KeyCode kb1Up = KeyCode.UpArrow;
        static KeyCode kb1Down = KeyCode.DownArrow;
        static KeyCode kb1A = KeyCode.M;
        static KeyCode kb1B = KeyCode.Comma;
        static KeyCode kb1X = KeyCode.Period;
        static KeyCode kb1Y = KeyCode.Slash;
        static KeyCode kb1Start = KeyCode.Return;

        static KeyCode kb2Left = KeyCode.A;
        static KeyCode kb2Right = KeyCode.D;
        static KeyCode kb2Up = KeyCode.W;
        static KeyCode kb2Down = KeyCode.S;
        static KeyCode kb2A = KeyCode.T;
        static KeyCode kb2B = KeyCode.Y;
        static KeyCode kb2X = KeyCode.U;
        static KeyCode kb2Y = KeyCode.I;
        static KeyCode kb2Start = KeyCode.Space;


        static float deadZone = 0.3f;

//        static InputDevice[] inControlDevices = new InputDevice[4];

        static bool haveInitialized;

        public static void GetInput(InputState inputState)
        {
            if (UnityEngine.InputSystem.Gamepad.current != null)
            {
                GetGamepadInput(UnityEngine.InputSystem.Gamepad.current, inputState);
            }
            else
            {
                GetKeyboard1Input(inputState);
                
            }
        }


        public static void GetInput(Device device, InputState inputState)
        {
            Initialize();

            CacheLastInput(inputState);

            if (device == Device.Keyboard1)
            {
                GetKeyboard1Input(inputState);
            }
            else if (device == Device.Keyboard2)
            {
                GetKeyboard2Input(inputState);
            }
            else if (device == Device.Gamepad1)
            {
                if (GamepadHasBeenAssigned(Device.Gamepad1))
                {
                    
                    GetGamepadInput(UnityEngine.InputSystem.Gamepad.all[0], inputState);
                }
                else
                {
                    ClearInputState(inputState);
                }
            }
            else if (device == Device.Gamepad2)
            {
                if (GamepadHasBeenAssigned(Device.Gamepad2))
                {
                    GetGamepadInput(UnityEngine.InputSystem.Gamepad.all[1], inputState);
                }
                else
                {
                    ClearInputState(inputState);
                }
            }
            else if (device == Device.Gamepad3)
            {
                if (GamepadHasBeenAssigned(Device.Gamepad3))
                {
                    GetGamepadInput(UnityEngine.InputSystem.Gamepad.all[2], inputState);
                }
                else
                {
                    ClearInputState(inputState);
                }
            }
            else if (device == Device.Gamepad4)
            {
                if (GamepadHasBeenAssigned(Device.Gamepad4))
                {
                    GetGamepadInput(UnityEngine.InputSystem.Gamepad.all[3], inputState);
                }
                else
                {
                    ClearInputState(inputState);
                }
            }
        }

        private static void CacheLastInput(InputState inputState)
        {
            inputState.wasAButton = inputState.aButton;
            inputState.wasBButton = inputState.bButton;
            inputState.wasXButton = inputState.xButton;
            inputState.wasYButton = inputState.yButton;

            inputState.wasLeft = inputState.left;
            inputState.wasRight = inputState.right;
            inputState.wasUp = inputState.up;
            inputState.wasDown = inputState.down;
            inputState.wasStart = inputState.start;
        }

        private static void Initialize()
        {
            if (haveInitialized)
                return;
            haveInitialized = true;

            int i = 0;
//            foreach (var device in InputManager.Devices)
//            {
//                
//                if (device != null)
//                {
//                    inControlDevices[i] = device;
//                    i++;
//                }
//            }

        }

        

        static void GetKeyboard1Input(InputState inputState)
        {
            inputState.xAxis = inputState.yAxis = inputState.leftTrigger = inputState.rightTrigger = 0f;
            if (Input.GetKey(kb1Left))
            {
                inputState.xAxis -= 1f;
            }
            if (Input.GetKey(kb1Right))
            {
                inputState.xAxis += 1f;
            }
            if (Input.GetKey(kb1Up))
            {
                inputState.yAxis += 1f;
            }
            if (Input.GetKey(kb1Down))
            {
                inputState.yAxis -= 1f;
            }

            inputState.up = Input.GetKey(kb1Up);
            inputState.down = Input.GetKey(kb1Down);
            inputState.left = Input.GetKey(kb1Left);
            inputState.right = Input.GetKey(kb1Right);

            inputState.yButton = Input.GetKey(kb1Y);
            inputState.xButton = Input.GetKey(kb1X);
            inputState.aButton = Input.GetKey(kb1A);
            inputState.bButton = Input.GetKey(kb1B);
            inputState.start = Input.GetKey(kb1Start);
        }

        static void GetKeyboard2Input(InputState inputState)
        {
            inputState.xAxis = inputState.yAxis = inputState.leftTrigger = inputState.rightTrigger = 0f;
            if (Input.GetKey(kb2Left))
            {
                inputState.xAxis -= 1f;
            }
            if (Input.GetKey(kb2Right))
            {
                inputState.xAxis += 1f;
            }
            if (Input.GetKey(kb2Up))
            {
                inputState.yAxis += 1f;
            }
            if (Input.GetKey(kb2Down))
            {
                inputState.yAxis -= 1f;
            }

            inputState.up = Input.GetKey(kb2Up);
            inputState.down = Input.GetKey(kb2Down);
            inputState.left = Input.GetKey(kb2Left);
            inputState.right = Input.GetKey(kb2Right);

            inputState.yButton = Input.GetKey(kb2Y);
            inputState.xButton = Input.GetKey(kb2X);
            inputState.aButton = Input.GetKey(kb2A);
            inputState.bButton = Input.GetKey(kb2B);
            inputState.start = Input.GetKey(kb2Start);
        }



        static void GetGamepadInput(UnityEngine.InputSystem.Gamepad device, InputState inputState)
        {
            if (device == null)
                return;
//            inputState.xAxis = device.LeftStickX;
//            inputState.yAxis = device.LeftStickY;


            inputState.right = (device.leftStick.ReadValue().x > deadZone) || device.dpad.right.isPressed;
            inputState.left = (device.leftStick.ReadValue().x < -deadZone) || device.dpad.left.isPressed;
            inputState.up = (device.leftStick.ReadValue().y > deadZone) || device.dpad.up.isPressed;
            inputState.down = (device.leftStick.ReadValue().y < -deadZone) || device.dpad.down.isPressed;

            inputState.aButton = device.aButton.isPressed;
            inputState.bButton = device.bButton.isPressed;
            inputState.xButton = device.xButton.isPressed;
            inputState.yButton = device.yButton.isPressed;

            inputState.leftTrigger = device.leftTrigger.ReadValue();
            inputState.rightTrigger = device.rightTrigger.ReadValue();
            inputState.start = device.startButton.isPressed;

        }

        

        public static void ClearInputState(InputState inputState)
        {
            inputState.rightTrigger = inputState.leftTrigger = inputState.xAxis = inputState.yAxis = 0f;
            inputState.left = inputState.right = inputState.up = inputState.down = inputState.aButton = inputState.bButton = inputState.xButton = inputState.yButton = false;
        }


       
        static bool GamepadHasBeenAssigned(Device device)
        {
            int index = 0;

            switch (device)
            {
                case Device.Gamepad1:
                    index = 0;
                    break;
                case Device.Gamepad2:
                    index = 1;
                    break;
                case Device.Gamepad3:
                    index = 2;
                    break;
                case Device.Gamepad4:
                    index = 3;
                    break;
                default:
                    break;
            }

            return UnityEngine.InputSystem.Gamepad.all.Count > index;
//            return inControlDevices[index] != null;
//            return false;

        }

       


    }
}
