# ModuleLoader GUI
A GUI version of ClementDreptin's ModuleLoader. It allows for loading & unloading modules via XDRPC.
The main reason I made this was because people were having difficulty with using Proto on WiFi with their BadUpdate consoles.

# How to load Proto on WiFi with BadUpdate
## Prerequisites:
- Ability to connect the console to internet
- Xbox 360 SDK installed on your PC
- USB that holds your DashLaunch config
- xbdm.xex, XDRPC.xex, and Proto.xex
- ModuleLoader GUI

## Instructions:
- Plug your USB into your computer.
- Load your DashLaunch launch.ini file in a text editor on your PC and look for the plugins list (plugin1=, plugin2=, etc).
- Remove any stealth server from your Dashlaunch plugins list.
- Make sure xbdm.xex is plugin1, XDRPC.xex plugin2. Also that "liveblock=true" and "livestrong=true".
    - Example:
      - plugin1 = Usb:\xbdm.xex
      - plugin2 = Usb:\XDRPC.xex
- Put Proto.xex on your USB.
- Save the modified launch.ini to your USB.
- Plug USB into your console and boot/reboot.
- Run BadUpdate and get the console into glitched state.
- Go to WiFi settings and connect. Don't run a connection test.
- Go back to Aurora.
- Grab your local IP address of the console by clicking the Back button. You will see "IP Address".
- Open Xbox Neighborhood on your computer.
- Click "Add Xbox 360" and enter the IP address you gathered from the console. Set it as the default console.
- Confirm connection in Xbox Neighborhood by double clicking the console name and seeing if drive list pops up.
- Open ModuleLoader GUI.
- Click "Load Proto" on the top right of the program.
- Confirm Proto has loaded by clicking guide.
- If it says Proto in the left corner of the guide menu, it has been loaded.
- Connect to Xbox Live.

If you have any further questions regarding SDK, DashLaunch, or xex files.. those will not be answered. You can find plenty of guides online for acquiring those files and setting them up.