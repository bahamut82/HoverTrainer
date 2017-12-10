using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoverTrainer
{
    class Controller
    {

        private long lastDashPressed = 0;
        private long lastDashReleased = 0;
        private bool wasPressed = false;

        private Joystick joystick;
        private JoystickState joystickState;

        public Controller()
        {
            InitializeDevices();
        }

        private void InitializeDevices() {
            DirectInput di = new DirectInput();

            Guid joystickGuid = Guid.Empty;

            foreach (var dev in di.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly)) {
                if (dev.InstanceGuid != Guid.Empty) {
                    joystickGuid = dev.InstanceGuid;
                    break;
                }
            }

            if (joystickGuid == Guid.Empty) {
                foreach (var dev in di.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly))
                {
                    if (dev.InstanceGuid != Guid.Empty)
                    {
                        joystickGuid = dev.InstanceGuid;
                        break;
                    }
                }
            }

            joystick = new Joystick(di, joystickGuid);
        }

        private bool IsDashPressed() {
            //joystick.Poll();
            return joystick.GetCurrentState().Buttons[2];
        }

        /// <summary>
        /// returns the amount of frames the dash button has been released.
        /// 0 as default value.
        /// </summary>
        public long FramesDashReleased() {
            if (IsDashPressed() == wasPressed) {
                return 0;
            }

            // TODO: think about time less than 1 frame released? 
            long delta = (DateTime.Now.Millisecond - lastDashReleased) / (1000 / 60);

            // key-down-event
            if (!wasPressed) {
                wasPressed = true;
                lastDashPressed = DateTime.Now.Millisecond;
                return delta;
            }

            // key-up-event
            if (wasPressed) {
                wasPressed = false;
                lastDashReleased = DateTime.Now.Millisecond;
            }
            return 0;
        }

        public string GetCurrentInput() {
            if (joystick == null) {
                return "No joystick connected.";
            }

            joystick.Acquire();
            joystick.Poll();
            joystickState = joystick.GetCurrentState();

            string inputString = "";
            int buttonId = 1;
            foreach (bool buttonFlag in joystickState.Buttons) {
                if (buttonFlag) {
                    inputString += GetButtonName(buttonId) + " ";
                }
                buttonId++;
            }

            return inputString;
        }

        private string GetButtonName(int buttonId)
        {
            switch (buttonId) {
                case 1: return "\x25A1"; // viereck
                case 2: return "\x2716"; // X
                case 3: return "\x25CB"; // O
                case 4: return "\x25B3"; // ▲
                case 5: return "L1";
                case 6: return "R1";
                case 7: return "L2";
                case 8: return "R2";
                case 9: return "Select";
                case 10: return "Start";
                case 11: return "L3";
                case 12: return "R3";
                case 13: return "PSButton";
                case 14: return "DS4Touchpad";
                default: return "Unknown_Button_id";
            }
        }
    }
}
