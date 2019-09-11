#pragma once

#define XINPUT_WRAPPER __declspec(dllexport)   

extern "C"
{
	XINPUT_WRAPPER int GetState(int controllerIndex, void* state);
}
