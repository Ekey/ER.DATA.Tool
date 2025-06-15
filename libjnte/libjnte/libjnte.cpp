#include "stdafx.h"
#include <stdlib.h>
#include <stdint.h>
#include <stdio.h>
#include <windows.h>

#ifdef _MSC_VER
#pragma warning( push )
  #pragma warning( disable : 4996 )
#endif

bool isLibraryLoaded = false;

//LZ4
//compressedBuf, decompressedBuf, compressedSize, decompressedSize
typedef int (* JNTE_LZ4Decompress_t)(uint8_t*, uint8_t*, uint32_t, uint32_t);
extern JNTE_LZ4Decompress_t JNTE_LZ4Decompress;
JNTE_LZ4Decompress_t JNTE_LZ4Decompress;

//ZSTD
//decompressedBuf, decompressedSize, compressedBuf, compressedSize
typedef int (* JNTE_ZSTDDecompress_t)(uint8_t*, uint32_t, uint8_t*, uint32_t);
extern JNTE_ZSTDDecompress_t JNTE_ZSTDDecompress;
JNTE_ZSTDDecompress_t JNTE_ZSTDDecompress;

//LZMA (unused at this time)

bool iInitializeLibrary()
{
    HINSTANCE hLib = LoadLibrary(L"UnityPlayer.dll");
    if (hLib == NULL)
    {
        MessageBoxA(0, "Unable to load UnityPlayer.dll library!", "ERROR", MB_ICONERROR | MB_OK);
        return isLibraryLoaded = false;
    }

    HMODULE hModule = GetModuleHandleA("UnityPlayer.dll");

    JNTE_LZ4Decompress = (JNTE_LZ4Decompress_t)((DWORD64)hModule + 0x108D160);
    JNTE_ZSTDDecompress = (JNTE_ZSTDDecompress_t)((DWORD64)hModule + 0x10972F0);

    return isLibraryLoaded = true;
}

extern "C"
{
__declspec(dllexport) int32_t __cdecl iJnteDecompress(uint8_t* compressedBuf, uint32_t compressedSize, uint8_t* decompressedBuf, uint32_t decompressedSize, int32_t CompressionType)
{
	if (!isLibraryLoaded)
	{
		isLibraryLoaded = iInitializeLibrary();
	}

	if (isLibraryLoaded)
	{
		switch(CompressionType)
		{
			case 1:  JNTE_LZ4Decompress(compressedBuf, decompressedBuf, compressedSize, decompressedSize); break; // LZ4
			case 2:  break; // LZMA (unused at this time)
			case 3:  JNTE_ZSTDDecompress(decompressedBuf, decompressedSize, compressedBuf, compressedSize); break; // ZSTD
			default: break;
		}
	}

	return 0;
 }
}