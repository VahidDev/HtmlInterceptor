# Amazon Coupon Service

A desktop application that monitors browser activity and automatically displays coupon code notifications when [Amazon.com](https://www.amazon.com) is detected. This tool watches active browser windows and presents relevant coupon codes to help save money on your Amazon purchases.

## Table of Contents

- [Features](#-features)
- [Technical Implementation](#️-technical-implementation)
  - [Project Structure](#project-structure)
- [Current Coupon Functionality](#-current-coupon-functionality)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Usage](#usage)
- [Configuration](#️-configuration)
- [How It Works](#-how-it-works)
- [Design Features](#-design-features)
- [Troubleshooting](#️-troubleshooting)
- [Advanced Usage](#️-advanced-usage)
- [License](#-license)
- [Contributing](#-contributing)
- [Related Projects](#-related-projects)



## 📋 Features

- Monitors active browser windows (Chrome, Firefox, Edge, Internet Explorer)
- Detects when Amazon.com websites are being viewed
- Displays popup notifications with valid coupon codes
- Includes a user-friendly coupon notification with copy functionality
- Implements a cooldown system to prevent notification spam
- Handles multiple browsers and windows simultaneously
- Includes comprehensive error handling and debugging output

## 🛠️ Technical Implementation

This application is built using:
- [C# (.NET)](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Windows Forms](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/?view=netdesktop-6.0) for the system tray icon
- [HttpListener](https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener) for the local notification server
- [Windows API](https://docs.microsoft.com/en-us/windows/win32/api/) calls for window detection and interaction

### Project Structure

- **Program.cs**: Application entry point and main control flow
- **BrowserMonitor.cs**: Handles detection and monitoring of browser windows
- **NotificationServer.cs**: Manages the local HTTP server for displaying notifications
- **WindowUtils.cs**: Utility functions for working with Windows API window functions

## 📌 Current Coupon Functionality

The current implementation:
1. Monitors browser window titles for "amazon" mentions
2. Creates an attractive popup notification with the current coupon code
3. Allows users to copy the code to clipboard with a single click
4. Implements a 30-minute cooldown between notifications for the same browser instance
5. Provides a clean user interface with Amazon-themed styling

The default coupon code is "SAVE20NOW" but can be easily modified in the NotificationServer.cs file.

## 🚀 Getting Started

### Prerequisites

- [Windows 10](https://www.microsoft.com/en-us/windows) or higher
- [.NET Framework 4.6.1](https://dotnet.microsoft.com/download/dotnet-framework) or higher
- Administrator privileges (recommended for accessing window titles)
- One or more web browsers installed:
  - [Google Chrome](https://www.google.com/chrome/)
  - [Mozilla Firefox](https://www.mozilla.org/firefox/)
  - [Microsoft Edge](https://www.microsoft.com/edge)
  - Internet Explorer

### Installation

1. Clone this repository or download the source code
2. Build the project using Visual Studio or the .NET CLI:
   ```
   dotnet build
   ```

### Usage

1. Run the application with administrator privileges:
   ```
   dotnet run
   ```
   Or launch the compiled executable with administrator rights

2. The application will:
   - Start monitoring your browser activity
   - Start a notification server on port 45678
   - Begin detecting Amazon websites in your browser windows

3. Browse to any Amazon website, and within seconds you should see a popup notification with a coupon code

4. To stop the application:
   - Press any key in the console window
   - The application will terminate all monitoring and server activities

## ⚙️ Configuration

To modify the coupon code:

1. Edit the `currentCouponCode` string in `NotificationServer.cs`
2. Rebuild the application

To change the notification cooldown period:

1. Modify the `notificationCooldown` value in `BrowserMonitor.cs`
2. Rebuild the application

To customize the notification appearance:

1. Edit the HTML, CSS, and JavaScript in the `HandleNotificationRequest` method of `NotificationServer.cs`
2. Rebuild the application

## 🔍 How It Works

1. **Browser Monitoring**: Continuously scans running processes for known browser applications
2. **Window Detection**: Checks each browser's window title for Amazon-related strings
3. **Active Window Check**: Ensures notifications are only shown for the currently active window
4. **Notification Server**: Runs a local HTTP server to display coupon notifications
5. **Cross-Browser Support**: Uses browser-specific approaches to show notifications in each browser
6. **Cooldown System**: Prevents showing the same notification repeatedly in a short time period

## 🔒 Design Features

The application includes several important design features:

- **Modular architecture**: Separates concerns into distinct classes
- **Multi-browser support**: Works with Chrome, Firefox, Edge, and Internet Explorer
- **Fallback mechanisms**: Multiple approaches to display notifications if the primary method fails
- **Debugging output**: Comprehensive console logging for troubleshooting
- **Background operation**: Runs silently in the background without interfering with normal usage

## ⚠️ Troubleshooting

### Browser Detection Issues

- Ensure you're running the application with administrator privileges
- Check if your browser is supported (Chrome, Firefox, Edge, or Internet Explorer)
- Check the console output for errors related to process access

### Notifications Not Appearing

- Verify you're visiting a page with "amazon" in the title
- Ensure your browser is the active window
- Check if the cooldown period has elapsed since the last notification
- Verify port 45678 is not being used by another application

### Popup Styling Issues

- Different browsers may render the notification HTML/CSS differently
- Check the browser console for any JavaScript errors
- Try using a different browser to see if the issue persists

## 🛠️ Advanced Usage

### Extending Browser Support

To add support for additional browsers:

1. Modify the browser detection logic in the `MonitorBrowsers` method in `BrowserMonitor.cs`
2. Add browser-specific notification code to the `ShowCouponNotification` method
3. Consider support for browsers like [Opera](https://www.opera.com/) or [Brave](https://brave.com/)

### Implementing More Sophisticated Amazon Detection

The current implementation uses a simple window title check. For more accurate detection:

1. Consider implementing [browser extensions](https://developer.chrome.com/extensions/getstarted) that communicate with this application
2. Use browser automation libraries like [Selenium](https://www.selenium.dev/) to inspect page content
3. Implement browser-specific debugging protocols:
   - [Chrome DevTools Protocol](https://chromedevtools.github.io/devtools-protocol/)
   - [Firefox Remote Debugging Protocol](https://firefox-source-docs.mozilla.org/remote/index.html)
   - [Microsoft Edge DevTools Protocol](https://docs.microsoft.com/en-us/microsoft-edge/devtools-protocol/)

### Adding Multiple Coupon Codes

To support different coupon codes for different Amazon sections:

1. Modify the `IsVisitingAmazon` method to detect specific Amazon pages
2. Create a coupon selection mechanism in `NotificationServer.cs`
3. Maintain a dictionary of coupon codes for different page types
4. Consider integrating with coupon APIs like [Honey](https://www.joinhoney.com/api) or similar services


