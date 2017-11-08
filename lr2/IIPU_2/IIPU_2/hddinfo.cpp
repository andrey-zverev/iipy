#include "ata.h"
#include "hddinfo.hpp"
#include "standarts.hpp"

#include <ntddscsi.h>

#include <allocators>
#include <bitset>
#include <vector>

using namespace std;

namespace hddinfo {

	ostream &setup(ostream &stream) {
	  stream.setf(ios::left);
	  stream << setw(15);
	  return stream;
	}

	ostream &setupRight(ostream &stream) {
	  stream.setf(ios::right);
	  stream << setw(15);
	  return stream;
	}

	ostream &setupAta(ostream &stream) {
	  stream.setf(ios::right);
	  stream << setw(25);
	  return stream;
	}

	HardDiskDrive::HardDiskDrive(string name) {
	  drive = name;
	  diskSize = 0;
	  freeSize = 0;
	  busySize = 0;

	  sataDevice = FALSE;
	  pataDevice = FALSE;
	  pioSupport = FALSE;
	  dmaSupport = FALSE;

	  ataSupportBits = 0;

	  hDevice = CreateFile(drive.c_str(),
					   GENERIC_READ | GENERIC_WRITE,
					   FILE_SHARE_READ | FILE_SHARE_WRITE,
					   NULL, OPEN_EXISTING, 0, NULL);

	  if (hDevice == INVALID_HANDLE_VALUE) {
		cout << "Can't get access to HDD: Error #" << GetLastError();
		if(GetLastError() == 5)
		{
			cout << endl << "No access to file!" << endl;
		}
		system("pause");
		exit(EXIT_FAILURE);
	  }
	}

	HardDiskDrive::~HardDiskDrive() {
	  CloseHandle(hDevice);
	}

	string HardDiskDrive::getBits(int16_t number) {
	   bitset<16> b(number);

	  string bits = b.to_string<char, char_traits<char>,
		allocator<char>>();

	  return bits;
	}

	BOOL HardDiskDrive::getBaseInfo() {
	  BOOL   bResult = FALSE;

	  STORAGE_PROPERTY_QUERY    storagePropertyQuery;
	  STORAGE_DESCRIPTOR_HEADER storageDescHeader = { 0 };
	  STORAGE_DEVICE_DESCRIPTOR *pDeviceDesc;

	  DWORD dwBytes = 0;
	  DWORD dwOutBufferSize = 0;
	  DWORD dwSerialNumberOffset = 0;
	  DWORD dwModelOffset = 0;
	  DWORD dwFirmwareOffset = 0;

	  BYTE *pOutBuffer = nullptr;

	  ZeroMemory(&storagePropertyQuery, sizeof(STORAGE_PROPERTY_QUERY));
	  storagePropertyQuery.PropertyId = StorageDeviceProperty;
	  storagePropertyQuery.QueryType = PropertyStandardQuery;

	  bResult = DeviceIoControl(hDevice, IOCTL_STORAGE_QUERY_PROPERTY,
				  &storagePropertyQuery, sizeof(storagePropertyQuery),
				  &storageDescHeader, sizeof(storageDescHeader),
				  &dwBytes, NULL);
  
	  if (bResult == FALSE) {
		cout << "HardDiskDrive::getBaseInfo(): "
				  << GetLastError() << endl;
		return bResult;
	  }
  
	  dwOutBufferSize = storageDescHeader.Size;

	  try {
		pOutBuffer = new BYTE[dwOutBufferSize];
		ZeroMemory(pOutBuffer, sizeof(pOutBuffer));
	  } catch (bad_alloc exp) {
		CloseHandle(hDevice);
		cout << "HardDiskDrive::getBaseInfo(): " 
				  << exp.what() << endl;
		return bResult;
	  }

	  bResult = DeviceIoControl(hDevice, IOCTL_STORAGE_QUERY_PROPERTY,
				  &storagePropertyQuery, sizeof(STORAGE_PROPERTY_QUERY),
				  pOutBuffer, dwOutBufferSize, &dwBytes, NULL);

	  if (bResult == FALSE) {
		cout << "HardDiskDrive::getBaseInfo(): "
				  << GetLastError() << endl;
		return bResult;
	  }

	  pDeviceDesc = (STORAGE_DEVICE_DESCRIPTOR *)pOutBuffer;

	  dwSerialNumberOffset = pDeviceDesc->SerialNumberOffset;
	  dwModelOffset        = pDeviceDesc->Version;
	  dwFirmwareOffset     = pDeviceDesc->ProductRevisionOffset;

	  diskModel    = CString(pOutBuffer + dwModelOffset);
	  diskSerial   = CString(pOutBuffer + dwSerialNumberOffset);
	  diskFirmware = CString(pOutBuffer + dwFirmwareOffset);

	  delete[] pOutBuffer;

	  return bResult;
	}

	BOOL HardDiskDrive::getDiskGeometry() {
	  BOOL bResult = FALSE;
  
	  DWORD dwBytesReturned = 0;

	  DISK_GEOMETRY diskGeometry;

	  ZeroMemory(&diskGeometry, sizeof(DISK_GEOMETRY));

	  bResult = DeviceIoControl(hDevice,
				  IOCTL_DISK_GET_DRIVE_GEOMETRY, NULL, 0,
				  &diskGeometry, sizeof(diskGeometry),
				  &dwBytesReturned, NULL);

	  if (bResult == FALSE) {
		cout << "HardDiskDrive::getDiskGeometry(): "
				  << GetLastError() << endl;
		return bResult;
	  }

	  diskSize = diskGeometry.Cylinders.QuadPart
			   * (ULONG)diskGeometry.TracksPerCylinder
			   * (ULONG)diskGeometry.SectorsPerTrack
			   * (ULONG)diskGeometry.BytesPerSector;

	  getFreeSpace("C:");
	  getFreeSpace("F:");

	  busySize = diskSize - freeSize;
  
	  return bResult;
	}

	BOOL HardDiskDrive::getFreeSpace(string partition) {
	  ULARGE_INTEGER uliFreeBytes;
  
	  BOOL bResult = GetDiskFreeSpaceEx(partition.c_str(), NULL,
						NULL, &uliFreeBytes);
  
	  if (bResult == FALSE) {
		return bResult;
	  }

	  freeSize += uliFreeBytes.QuadPart;

	  return bResult;
	}

	BOOL HardDiskDrive::getSataCompliance() { 
	  DWORD dwBytes;
	  BOOL  bResult;

	  CONST UINT bufferSize = 512;
	  CONST BYTE identifyDataCommandId =  0xEC;
  
	  UCHAR identifyDataBuffer[bufferSize
		+ sizeof(ATA_PASS_THROUGH_EX)] = { 0 };
   
	  ATA_PASS_THROUGH_EX & PTE = *(ATA_PASS_THROUGH_EX *) identifyDataBuffer;
	  PTE.Length = sizeof(PTE);
	  PTE.TimeOutValue = 10;
	  PTE.DataTransferLength = 512;
	  PTE.DataBufferOffset = sizeof(ATA_PASS_THROUGH_EX);
 
	  IDEREGS * ideRegs = (IDEREGS *) PTE.CurrentTaskFile;
	  ideRegs->bCommandReg = identifyDataCommandId;
	  ideRegs->bSectorCountReg = 1;
 
	  PTE.AtaFlags = ATA_FLAGS_DATA_IN | ATA_FLAGS_DRDY_REQUIRED;
 
	  bResult = DeviceIoControl(hDevice, IOCTL_ATA_PASS_THROUGH, &PTE,
				  sizeof(identifyDataBuffer), &PTE,
				  sizeof(identifyDataBuffer), &dwBytes, 0);
  
	  if (bResult == FALSE) {
		cout << "HardDiskDrive::getSataCompliance(): "
				  << GetLastError() << endl;
		return bResult;
	  }
 
	  WORD *data = (WORD *)(identifyDataBuffer + sizeof(ATA_PASS_THROUGH_EX));
  
	  int interfaceId = data[222];
	  ataSupportBits = data[80];

	  if ((data[80] >> 8) == 0) {
		data[93] == 0 ? sataDevice = TRUE : pataDevice = TRUE;
	  } else if ((interfaceId >> 12) == 0) {
		pataDevice = TRUE;
	  } else if ((interfaceId >> 12) == 1) {
		sataDevice = TRUE;
		interfaceId &= 0x7FF;

		int bitIndex = 0;
        
		while(interfaceId > 0) {
		  if ((interfaceId & 0x01) != 0) {
			this->transferStandarts.push_back(
			  hddinfo::transferStandarts[bitIndex]);
		  }
		  interfaceId >>= 1;
		  bitIndex++;
		}
	  } else {
		cout << "Disk is interface is unknown" << endl;
	  }

	  return bResult;
	}

	BOOL HardDiskDrive::getAtaCompliance() {
	  char ataBits[17];

	  string bitSet = getBits(ataSupportBits);

	  for (int i = 0, j = 15; i < 16; i++, j--) {
		ataBits[i] = bitSet.at(j);
	  }

	  ataBits[16] = '\0';
  
	  for (int i = 1; i < 11; i++) {
		if (ataBits[i] == '1') {
		  this->ataStandarts.push_back(hddinfo::ataStandarts[i - 1]);
		}
	  }

	  return TRUE;
	}

	BOOL HardDiskDrive::getMemAccessMode() {
	  DWORD dwBytesRead;

	  BOOL bResult;

	  STORAGE_PROPERTY_QUERY storagePropertyQuery = {};
	  STORAGE_ADAPTER_DESCRIPTOR storageAdapterDesc = {};

	  ZeroMemory(&storagePropertyQuery, sizeof(STORAGE_PROPERTY_QUERY));
	  storagePropertyQuery.PropertyId = StorageAdapterProperty;
	  storagePropertyQuery.QueryType = PropertyStandardQuery;

	  bResult = DeviceIoControl(hDevice,
				  IOCTL_STORAGE_QUERY_PROPERTY, &storagePropertyQuery,
				  sizeof(STORAGE_PROPERTY_QUERY), &storageAdapterDesc,
				  sizeof(STORAGE_ADAPTER_DESCRIPTOR), &dwBytesRead, NULL);

	  if (bResult == FALSE) {
		cout << "HardDiskDrive::getMemAccessMode(): " << GetLastError()
				  << endl;
		return bResult;
	  }

	  storageAdapterDesc.AdapterUsesPio == TRUE ?
		pioSupport = TRUE : dmaSupport = TRUE;
  
	  return bResult;
	}

	void HardDiskDrive::showInfo() {
		cout << "__________________________________________________________" << endl;
		cout << "|                  Information about HDD                 |" << endl;
		cout << "|________________________________________________________|" << endl;
		cout << setup << "|     Model     | " << diskModel << "         |" << endl;
		cout << "|_______________|________________________________________|" << endl;
		cout << setup << "|    Firmware   | " << diskFirmware << "                               |" <<endl;
		cout << "|_______________|________________________________________|" << endl;
		cout << setup << "|     Serial    | " << diskSerial << "                   |" << endl;
		cout << "|_______________|________________________________________|" << endl;
		cout << setup << "|   Memory HDD  |                                        |" << endl;
		cout << setup << "|  - Full size  | " << setupRight << diskSize / GB_SIZE << " GigaByte               |" << endl;
		cout << setup << "|  - Busy size  | " << setupRight << busySize / GB_SIZE << " GigaByte               |" << endl;
		cout << setup << "|  - Free size  | " << setupRight << freeSize / GB_SIZE << " GigaByte               |" << endl;
		cout << "|_______________|________________________________________|" << endl;
		cout << "|  ATA support  |                                        |" << endl;
   
		for (auto entry : this->ataStandarts) 
		{
			cout << "|               |" << setupAta << entry << "               |" << endl;
		}
        cout << "|_______________|________________________________________|" << endl; 

		cout << setup << "| Transfer mode |";
		if(pioSupport == TRUE)
		{
			cout << setup << "PIO" << setw(26) << "|" << endl;
		}
		else
		{
			cout << setup << "DMA" << setw(26) << "|" << endl;
		}
		cout << "|_______________|________________________________________|" << endl; 
	}
}