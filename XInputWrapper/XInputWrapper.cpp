#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "XInputWrapper.h"
#include <XInput.h>
#pragma comment(lib,"xinput9_1_0.lib")

XINPUT_WRAPPER int GetState(int controllerIndex, void* state)
{
	int result = XInputGetState(controllerIndex, (XINPUT_STATE*)state);
	return result;
}
