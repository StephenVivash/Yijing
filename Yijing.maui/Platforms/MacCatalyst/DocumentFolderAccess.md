# Mac data folder access

The app keeps App Sandbox enabled. `DataSetupPage` runs after a window is loaded,
before constructing `AppShell` or opening the database. On first use, choose
**Documents** to use `Documents/Yijing`, or choose an existing **Yijing** folder.
Other selected parent folders also get a `Yijing` subfolder. Existing data is reused.

`MacDocumentFolderAccess` stores the data folder's security-scoped bookmark as
Base64 in the app's preferences (`Yijing.DocumentFolderBookmark`). A plain path
does not retain sandbox permission. Each launch resolves the bookmark, starts
security-scoped access, checks that the folder exists and is writable, and
refreshes stale bookmark data. Access remains active until app termination so
database connections and background EEG work can use the folder.

Cancel leaves the setup page open. If access cannot be restored, the page offers
Retry and Choose folder. The previous bookmark is preserved until a new selection
is successfully validated and saved; an unavailable drive does not erase it.

The project explicitly includes `Entitlements.plist` when signing Mac Catalyst
builds. Apple's `NSURL.h` declares both `WithSecurityScope` flags available on
Mac Catalyst 13+, although the installed .NET bindings annotate them as macOS-only.
The two narrowly scoped CA1416 suppressions document that binding discrepancy.

Reference: [Apple: Accessing files from the macOS App Sandbox](https://developer.apple.com/documentation/security/accessing-files-from-the-macos-app-sandbox).

## Validation

Storage regression tests can be run with the repository's current xUnit runner:

```sh
dotnet build Yijing.maui.test/Yijing.maui.test.csproj
dotnet Yijing.maui.test/bin/Debug/net10.0/Yijing.maui.test.dll -class Yijing.Maui.Test.AppSettingsTests
```

The tests cover awaiting sample initialization, preserving existing files, and
changing EEG device without changing the authorized folder. They do not emulate
Apple sandbox permissions.

Check these scenarios using a signed, sandboxed Mac build with a stable bundle
identifier and signing identity (an unsigned build only validates compilation):

1. With no saved bookmark, launch and cancel the picker. Setup should remain
   usable; the main app and database should not open.
2. Choose Documents. Confirm that `Documents/Yijing` and its database, Log, Muse,
   Emotiv, Questions, and Answers folders are usable.
3. Fully quit and relaunch. Confirm that no folder selection is required and the
   same data is present. Repeat after a Mac restart.
4. Choose an existing Yijing folder in a fresh app profile. Confirm it is reused
   directly, without creating `Yijing/Yijing` or replacing existing files.
5. Quit, rename or move the Yijing folder on the same volume, and relaunch. Confirm
   bookmark resolution follows it and that a subsequent launch also succeeds.
6. Make the saved folder unavailable or revoke access, then relaunch. Confirm
   setup offers recovery instead of opening a new database elsewhere. Restore
   access and retry, or reselect the folder.
