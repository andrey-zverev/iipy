#ifndef HHDINFO_H_
#define HDDINFO_H_

#include <atlstr.h>
#include <Windows.h>
#include <winioctl.h>

#include <cstdio>
#include <cstdint>
#include <cstdlib>
#include <iomanip>
#include <ios>
#include <iostream>
#include <string>
#include <vector>

#define KB_SIZE 1024
#define MB_SIZE ((KB_SIZE) * (KB_SIZE))
#define GB_SIZE ((MB_SIZE) * (KB_SIZE))

using namespace std;

namespace hddinfo {

ostream &setup(ostream &);

class HardDiskDrive {
public:
  HardDiskDrive(string);
  ~HardDiskDrive();

  BOOL getBaseInfo();
  BOOL getDiskGeometry();
  BOOL getFreeSpace(string);
  BOOL getStandartCompliance();
  BOOL getMemAccessMode();
  BOOL getSataCompliance();
  BOOL getAtaCompliance();
  
  string getBits(int16_t);

  void showInfo();
  void showFeaturesSupport();

private:
  string drive;

  HANDLE hDevice;

  BOOL sataDevice;
  BOOL pataDevice;
  BOOL pioSupport;
  BOOL dmaSupport;

  int16_t ataSupportBits;

  vector<string> transferStandarts;
  vector<string> ataStandarts;

  CString diskModel;
  CString diskFirmware;
  CString diskSerial;

  ULONGLONG diskSize;
  ULONGLONG freeSize;
  ULONGLONG busySize;
};

} /* namespace hddinfo */

#endif /* HDDINFO_H_ */