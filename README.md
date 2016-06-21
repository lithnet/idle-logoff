# Lithnet Idle Logoff
#### A group-policy enabled utility for logging off idle windows user sessions

The Lithnet Idle Logoff tool is a simple utility that allows you to log off users after a period of inactivity. It was designed specifically with kiosk and student lab scenarios in mind. 

- The tool runs in the background of each user session when installed
- It logs the user out after a preset period of inactivity
- It provides the ability to control all settings via a group policy

The app simply queries the relevant Windows API for the time since the user last interacted with the computer, and calls the logoff function after the specified period has elapsed. 

See the wiki for installation and configuration information
