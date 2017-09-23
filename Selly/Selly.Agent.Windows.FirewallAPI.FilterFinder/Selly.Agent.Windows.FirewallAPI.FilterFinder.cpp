// Based loosely on this blog post and sample code
// http://blogs.msdn.com/onoj/archive/2007/05/09/windows-vista-security-series-programming-the-windows-vista-firewall.aspx  
// http://download.microsoft.com/download/f/1/2/f12dbbb5-d164-4e7c-b42d-aaca3efb85dc/FirewallSample.exe  

#include "stdafx.h"

#define CHECK_DWORD(_X) {                                           \
    if (ERROR_SUCCESS != (status = _X)) {                           \
        printf("%s - 0x%x\n", #_X, status);                         \
        __leave;                                                    \
    }																\
}

int main(int argc, char* argv[])
{
	DWORD status = 0;
	HANDLE hEngine = NULL;
	FWPM_SESSION0 session = { 0 };
	FWPM_FILTER0_* filter = NULL;
	//FWPM_LAYER0_* layer = NULL;

	if (argc != 2)
	{
		return -1;
	}

	unsigned long id = strtoul(argv[1], NULL, 10);
	//unsigned short layerId = 44;

	__try
	{
		CHECK_DWORD(FwpmEngineOpen0(NULL, RPC_C_AUTHN_WINNT, NULL, &session, &hEngine));

		CHECK_DWORD(FwpmFilterGetById0(hEngine, id, &filter));

		printf("%S", filter->displayData.name);

		FwpmFreeMemory0((void **) &filter);

		//CHECK_DWORD(FwpmLayerGetById0(hEngine, layerId, &layer));
		//printf("%S", layer->displayData.name);
	}
	__finally
	{
		if (hEngine != NULL)
		{
			FwpmEngineClose0(hEngine);
		}
	}

    return 0;
}

