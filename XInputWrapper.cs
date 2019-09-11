using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Gamepad
{
	static class XInput
	{
		public const int MAX_CONTROLLERS				= 4;
		public const int THUMB_MAX						= 32767;
		public const int THUMB_DEAD_ZONE_L				= 7849; // Copied form the Xinput.h
		public const int THUMB_DEAD_ZONE_R				= 8689; // Copied form the Xinput.h
		public const int TRIGGER_THRESHOLD				= 30;   // Copied form the Xinput.h
		public const int XINPUT_GAMEPAD_DPAD_UP			= 0x0001;
		public const int XINPUT_GAMEPAD_DPAD_DOWN		= 0x0002;
		public const int XINPUT_GAMEPAD_DPAD_LEFT		= 0x0004;
		public const int XINPUT_GAMEPAD_DPAD_RIGHT		= 0x0008;
		public const int XINPUT_GAMEPAD_START			= 0x0010;
		public const int XINPUT_GAMEPAD_BACK			= 0x0020;
		public const int XINPUT_GAMEPAD_LEFT_THUMB		= 0x0040;
		public const int XINPUT_GAMEPAD_RIGHT_THUMB		= 0x0080;
		public const int XINPUT_GAMEPAD_LEFT_SHOULDER	= 0x0100;
		public const int XINPUT_GAMEPAD_RIGHT_SHOULDER	= 0x0200;
		public const int XINPUT_GAMEPAD_A				= 0x1000;
		public const int XINPUT_GAMEPAD_B				= 0x2000;
		public const int XINPUT_GAMEPAD_X				= 0x4000;
		public const int XINPUT_GAMEPAD_Y				= 0x8000;
		public const int ERROR_DEVICE_NOT_CONNECTED		= 1167; // Copied form the winerror.h
	}

	struct ThumbVector
	{
		public double X;
		public double Y;
	}

	struct XINPUT_GAMEPAD
	{
		public ushort wButtons;
		public byte bLeftTrigger;
		public byte bRightTrigger;
		public short sThumbLX;
		public short sThumbLY;
		public short sThumbRX;
		public short sThumbRY;

		#region Private methods
		private double GetRadian(double x, double y)
		{
			double radian = 0;

			if (x == 0)
			{
				if (y > 0) radian = 0;
				else if (y < 0) radian = Math.PI;
			}
			else if (y == 0)
			{
				if (x > 0) radian = Math.PI / 2;
				else if (x < 0) radian = Math.PI * 3 / 2;
			}
			else if (x > 0)
			{
				radian = -Math.Atan(y / x) + Math.PI / 2;
			}
			else if (x < 0)
			{
				radian = -Math.Atan(y / x) + Math.PI * 3 / 2;
			}

			return radian;
		}

		private ThumbVector NormalizeThumb(double x, double y, double deadzone)
		{
			ThumbVector vec = new ThumbVector();
			double magnitude = Math.Sqrt(x * x + y * y);
			if (magnitude > deadzone)
			{
				vec.X = Math.Round(x / (XInput.THUMB_MAX - deadzone), 2);
				vec.Y = Math.Round(y / (XInput.THUMB_MAX - deadzone), 2);
			}
			else
			{
				vec.X = 0;
				vec.Y = 0;
			}
			return vec;
		}
		#endregion

		#region Properties
		public ThumbVector ThumbL { get { return NormalizeThumb(sThumbLX, sThumbLY, XInput.THUMB_DEAD_ZONE_L); } }
		public ThumbVector ThumbR { get { return NormalizeThumb(sThumbRX, sThumbRY, XInput.THUMB_DEAD_ZONE_R); } }

		public bool Up { get { return (wButtons & XInput.XINPUT_GAMEPAD_DPAD_UP) > 0; } }
		public bool Right { get { return (wButtons & XInput.XINPUT_GAMEPAD_DPAD_RIGHT) > 0; } }
		public bool Down { get { return (wButtons & XInput.XINPUT_GAMEPAD_DPAD_DOWN) > 0; } }
		public bool Left { get { return (wButtons & XInput.XINPUT_GAMEPAD_DPAD_LEFT) > 0; } }
		public bool Start { get { return (wButtons & XInput.XINPUT_GAMEPAD_START) > 0; } }
		public bool Back { get { return (wButtons & XInput.XINPUT_GAMEPAD_BACK) > 0; } }
		public bool A { get { return (wButtons & XInput.XINPUT_GAMEPAD_A) > 0; } }
		public bool B { get { return (wButtons & XInput.XINPUT_GAMEPAD_B) > 0; } }
		public bool X { get { return (wButtons & XInput.XINPUT_GAMEPAD_X) > 0; } }
		public bool Y { get { return (wButtons & XInput.XINPUT_GAMEPAD_Y) > 0; } }
		public byte TriggerL { get {
			return bLeftTrigger > XInput.TRIGGER_THRESHOLD ? bLeftTrigger : (byte)0;
		}}
		public byte TriggerR { get {
			return bRightTrigger> XInput.TRIGGER_THRESHOLD ? bRightTrigger : (byte)0;
		}}

		public bool ShoulderL { get { return (wButtons & XInput.XINPUT_GAMEPAD_LEFT_SHOULDER) > 0; } }
		public bool ShoulderR { get { return (wButtons & XInput.XINPUT_GAMEPAD_RIGHT_SHOULDER) > 0; } }

		public bool ThumbLLeans { get {
			return ThumbL.X != 0 && ThumbL.Y != 0;
		}}
		public bool ThumbRLeans { get {
			return ThumbR.X != 0 && ThumbR.Y != 0;
		}}
		#endregion

		public bool Equals(XINPUT_GAMEPAD other)
		{
			return wButtons == other.wButtons
					&& bLeftTrigger == other.bLeftTrigger
					&& bRightTrigger == other.bRightTrigger
					&& sThumbLX == other.sThumbLX
					&& sThumbLY == other.sThumbLY
					&& sThumbRX == other.sThumbRX
					&& sThumbRY == other.sThumbRY;
		}
	}

	struct XINPUT_STATE
	{
		public uint dwPacketNumber;
		public XINPUT_GAMEPAD Gamepad;
		public bool Equals(XINPUT_STATE other)
		{
			return Gamepad.Equals(other.Gamepad);
		}
	}

	class XInputWrapper
	{
		private const string DLL_NAME = "XInputWrapper.dll";

		[DllImport(DLL_NAME, EntryPoint = "GetState", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetState(int index, ref XINPUT_STATE state);
	}
}
