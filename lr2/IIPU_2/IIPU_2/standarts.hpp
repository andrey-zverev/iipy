#ifndef STANDARTS_HPP
#define STANDARTS_HPP

#include <string>

namespace hddinfo {

const std::string transferStandarts[] = {
  "ATA8-AST", "SATA 1.0a", "SATA II: Ext.",
  "SATA 2.5", "SATA 2.6", "SATA 3.0", "SATA 3.1",
  "SAS", "Reserved", "Reserved", "Reserved", "Reserved",
};

const std::string ataStandarts[] = {
  "ATA-1", "ATA-2", "ATA-3", "ATA/ATAPI-4", "ATA/ATAPI-5",
  "ATA/ATAPI-6", "ATA/ATAPI-7", "ATA8-ACS", "ACS-2", "ACS-3"
};

}

#endif 