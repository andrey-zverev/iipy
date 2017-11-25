sudo hdparm -I /dev/sda | grep Number
sudo hdparm -I /dev/sda | grep Firmware
sudo hdparm -I /dev/sda | grep Used
sudo hdparm -I /dev/sda | grep Supported
sudo hdparm -I /dev/sda | grep PIO
sudo hdparm -I /dev/sda | grep DMA
df | awk '{size+=$2; used+=$3; avail+=$4} END {print "Total Size: " size/1024/1024 "Gb" "\n" "Used: " used/1024/1024 "Gb" "\n" "Available: " avail/1024/1024 "Gb"}'
