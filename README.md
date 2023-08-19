![](https://github.com/lithnet/idle-logoff/wiki/images/logo_ex_small.png)
# Lithnet Idle Logoff
#### A group-policy enabled utility for logging off idle windows user sessions

The Lithnet Idle Logoff tool is a simple utility that allows you to log off users, reboot, or shutdown a computer after a period of user inactivity, and optionally display a warning message before this happens. It was designed specifically with kiosk and student lab scenarios in mind. 

- The tool runs in the background of each user session when installed
- It logs the user out after a preset period of inactivity
- It provides the ability to control all settings via a group policy

The app simply queries the relevant Windows API for the time since the user last interacted with the computer, and calls the logoff function after the specified period has elapsed. 

## Getting started
See the [wiki](https://github.com/lithnet/idle-logoff/wiki) for installation and configuration information

## How can I contribute to the project?
* Found an issue and want us to fix it? [Log it](https://github.com/lithnet/idle-logoff/issues)
* Want to fix an issue yourself or add functionality? Clone the project and submit a pull request

## Enteprise support
Lithnet offer enterprise support plans for our open-source products. Deploy our tools with confidence that you have the backing of the dedicated Lithnet support team if you run into any issues, have questions, or need advice. Fill out our [contact form](https://lithnet.io/contact-us), let us know the number of devices you are using the product on, and we'll put together a quote.

## Keep up to date
* [Visit our blog](http://blog.lithnet.io)
* [Follow us on twitter](https://twitter.com/lithnet_io)![](http://twitter.com/favicon.ico)
