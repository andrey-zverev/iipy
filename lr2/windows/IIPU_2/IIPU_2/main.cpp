#include <Windows.h>
#include <winioctl.h>

#include <bitset>
#include <stdio.h>
#include <stdlib.h>

#include "hddinfo.hpp"

int main() {
	hddinfo::HardDiskDrive drive("\\\\.\\PhysicalDrive0");

	drive.getBaseInfo();
	drive.getDiskGeometry();
	drive.getMemAccessMode();
	drive.getSataCompliance();
	drive.getAtaCompliance();
	drive.showInfo();
  
	system("pause");

	return EXIT_SUCCESS;
}