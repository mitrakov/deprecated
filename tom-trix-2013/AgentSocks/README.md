Система агентного моделирования
========

1) Make sure the jdk-7 is installed on your computer
2) Unpack the archive in the directory you wish (e.g. "C:\agentsocks\" on Windows or "/home/user/agentsocks/" on Linux)
3) Set the environment variable "MPJ_HOME" to the "mpj" subfolder
    Windows: press <Win+PAUSE>, click "Advanced system settings", then "Environment Variables..." and add new variable (e.g. name = MPJ_HOME, value = C:\agentsocks\mpj)
    Linux: add the variable to "/etc/environment" file (e.g. MPJ_HOME="/home/user/agentsocks/mpj") and reboot the computer. Also you can use an "export" command
4) To launch the application run the script "start.bat" (Windows) or "start.sh" (Linux). By the way it just runs 1 simple command:
    Windows: %MPJ_HOME%\bin\mpjrun.bat -np 2 -jar agentsocks.jar
    Linux: $MPJ_HOME/bin/mpjrun.sh -np 2 -jar agentsocks.jar

P.S. -np = count of nodes
P.P.S. to launch the application in the editor mode open the command shell, change directory to your working folder and run "java -jar agentsocks.jar -gui" or "java -jar agentsocks.jar -console"
