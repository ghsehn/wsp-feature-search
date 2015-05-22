# WSP Feature Search

This console application aims to help finding a specific SharePoint feature by its ID inside a bunch of solution packages (WSP files).

It is specially helpful when you need to make some kind of migration between environments and you start getting errors because of missing features. With this tool, you can locate which exact solution package contains the feature that is missing in your environment.

### Usage
1. Copy your solution packages to a single directory in your computer. Example: `C:\Solutions` (if you need to export them from SharePoint, you can use the [`Get-SPSolution`](https://technet.microsoft.com/en-us/library/ff607754.aspx) cmdlet)
2. Execute the console application.
3. Specify the directory where you put your solution files.
4. Specify the feature GUID you are looking for (leave empty if you want to list all features).
