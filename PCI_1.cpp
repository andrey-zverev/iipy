#include <stdio.h>
#include <windows.h>
#include <setupapi.h>
#include <devguid.h>
#include <regstr.h>
#include <locale.h>

#pragma comment(lib, "setupapi.lib")
int main(int argc, char *argv[], char *envp[])
{
	setlocale(LC_ALL, "RUSSIAN");

	HDEVINFO hDevInfo;
	SP_DEVINFO_DATA DeviceInfoData;
	DWORD i;
	int number = 0;
	hDevInfo = SetupDiGetClassDevs(NULL, "PCI", 0, DIGCF_PRESENT | DIGCF_ALLCLASSES);

	if (hDevInfo == INVALID_HANDLE_VALUE)
	{
		return 1;
	}

	DeviceInfoData.cbSize = sizeof(SP_DEVINFO_DATA);
	
	for (i = 0; SetupDiEnumDeviceInfo(hDevInfo, i, &DeviceInfoData); i++)
	{
		DWORD DataT;
		LPTSTR buffer = NULL;
		DWORD buffersize = 0;

		while (!SetupDiGetDeviceRegistryProperty(hDevInfo, &DeviceInfoData, SPDRP_DEVICEDESC, &DataT, (PBYTE)buffer, buffersize, &buffersize))
		{
			if (GetLastError() == ERROR_INSUFFICIENT_BUFFER)
			{
				if (buffer) LocalFree(buffer);
				buffer = (LPSTR)LocalAlloc(LPTR, buffersize * 2);
			}
			else
			{
				break;
			}
		}
		number++;
		printf("%d.%s\n",number, buffer);
		if (buffer) 
		{
			LocalFree(buffer);
		}
	}


	if (GetLastError() != NO_ERROR && GetLastError() != ERROR_NO_MORE_ITEMS)
	{
		return 1;
	}

	SetupDiDestroyDeviceInfoList(hDevInfo);

	system("pause");
	return 0;
}
