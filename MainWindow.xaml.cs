using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Gamepad;

namespace XInputSample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private XINPUT_STATE GamePadState { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			Task.Factory.StartNew(() => {
				while (true)
				{
					XINPUT_STATE nextState = new XINPUT_STATE();
					int result = XInputWrapper.GetState(0, ref nextState);
					if (result == XInput.ERROR_DEVICE_NOT_CONNECTED)
					{
						Console.WriteLine("Not Connected");
					}
					else
					{
						if (!GamePadState.Equals(nextState))
						{
							GamePadState = nextState;
							Dispatcher.Invoke(() =>
							{
								Draw();
							});
						}
					}

					Thread.Sleep(5);
				}
			});


		}

		private void Draw()
		{
			label_controller_inputs.Content = "Left stick (x, y): (" + GamePadState.Gamepad.ThumbL.X + "," + GamePadState.Gamepad.ThumbL.Y + ")\n"
											+ "Right stick (x, y): (" + GamePadState.Gamepad.ThumbR.X + "," + GamePadState.Gamepad.ThumbR.Y + ")\n"
											+ "(A, B, X, Y) : (" + GamePadState.Gamepad.A + ", " + GamePadState.Gamepad.B + ", " + GamePadState.Gamepad.X + ", " + GamePadState.Gamepad.Y + ")" + "\n"
											+ "(Up, Down, Left, Right) : (" + GamePadState.Gamepad.Up + ", " + GamePadState.Gamepad.Down + ", " + GamePadState.Gamepad.Left + ", " + GamePadState.Gamepad.Right + ")" + "\n"
											+ "Start, Next : " + GamePadState.Gamepad.Start + ", " + GamePadState.Gamepad.Back + "\n"
											+ "Left Trigger : " + GamePadState.Gamepad.TriggerL + "\n"
											+ "Right Trigger : " + GamePadState.Gamepad.TriggerR + "\n"
											+ "Left shoulder : " + GamePadState.Gamepad.ShoulderL + "\n"
											+ "Right shoulder : " + GamePadState.Gamepad.ShoulderR + "\n";
		}
	}
}
