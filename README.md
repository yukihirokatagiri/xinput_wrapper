# Overview
This is a sample application which gets and shows XBox controller inputs.  
<img src="screenshot.png" alt="screenshot">

# How it works
The XInput.h can be used for getting the XBox controller's state by XInputGetState. The state is an instance of XINPUT_STATE structure. This sample code wraps the XInputGetState function and provides DLL interface. And the WPF sample application polls the function to show the state.

You can fin the XInputGetState function document here.  
https://docs.microsoft.com/en-us/windows/win32/api/xinput/nf-xinput-xinputgetstate
