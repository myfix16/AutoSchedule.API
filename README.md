﻿<h1 align="center">
  Auto Schedule
</h1>
<p align="center">
  Automatically generate class schedules.
</p>
<p align="center">
  <a style="text-decoration:none">
    <img src="https://img.shields.io/badge/Platform-Windows%20%7C%20macOS%20%7C%20Linux-yellow" alt="Platform" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/badge/Framework-Blazor%20WebAssembly-red" alt="Framework" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/badge/Version-1.1.0-blue" alt="Version" />
  </a>
  <a style="text-decoration:none">
    <img src="https://dev.azure.com/myfix16/AutoSchedule/_apis/build/status/AutoSchedule%20-%201%20-%20CI?branchName=master" alt="Build Status" />
  </a>
</p>

## ✨Feature
This program is designed to **automatically generate class schedules** for CUHK(SZ) students to help them in academic planning.

## 🔗Get Access
The program is deployed on Azure📦. You can access it from **[Here](https://autoschedule.azurewebsites.net/)**.

## 🌈Browsers support
| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="Microsoft Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)<br/>Microsoft Edge | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)<br/>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)<br/>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)<br/>Safari |
| --------- | --------- | --------- | --------- |
| last 2 versions| last 2 versions| last 2 versions| last 2 versions

## 📕 User Guidance
1. From available courses list, select the courses that you want to take by using the tool bar on the middle of available courses list and selected courses list.
2. After finishing selecting courses, click "Make Schedule" button to generate possible schedules.
3. All possible schedules will be listed in a listbox below. You can then select one and click "View Schedule" to view your schedule.

## 🤝Contributing
### Build and run
Prerequists:  
+ [.NET 6 SDK](https://dotnet.microsoft.com/download)
+ Visual Studio 2022 or Visual Studio for Mac 2022

Explaination of projects:
+ **AutoSchedule.Core**: A library that defines the basic classes and implements core course selection mechanism.
+ **AutoSchedule.UI**: The main web application built with blazor WASM.
+ **AutoSchedule.API**: A backend application whose only purpose is to provide session information.

### Bugs or Suggestions?
Feel free to fire an [issue](https://github.com/myfix16/AutoSchedule/issues/new).

## 🔧Dependencies and References
+ [AutoSchedule.Data](https://github.com/myfix16/AutoSchedule.Data) for course data service
+ [Syncfusion Blazor Components](https://www.syncfusion.com/blazor-components) for UI components
+ [Blazor Fluent UI](https://github.com/BlazorFluentUI/BlazorFluentUI) for UI style

## 💕Contributors
<span>
  <img src="https://contrib.rocks/image?repo=myfix16/AutoSchedule" />
</span>

Made with [contributors-img](https://contrib.rocks).
