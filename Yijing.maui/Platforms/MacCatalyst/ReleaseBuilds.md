# Release Bluetooth privacy crash

On 2026-10-15, the arm64 Release app launched but macOS terminated it when Muse
Live requested Bluetooth. The system log reported a missing
`NSBluetoothAlwaysUsageDescription`. The source Info.plist already contained
the description, while the packaged Release Info.plist was still cached from
October 3. Debug had a freshly compiled manifest and worked.

The installed Apple SDK 26.5 tracks the source manifest's path in
`_CompileAppManifest.inputs`, but omits the source file itself from the
`_CompileAppManifest` target's incremental inputs. `TrackAppleAppManifestInput`
in the application project adds that file for both Mac Catalyst and iOS, so
editing privacy descriptions invalidates the compiled manifest in Debug and
Release builds.

Build the arm64 Release app with:

```sh
dotnet build Yijing.maui/Yijing.maui.csproj -c Release \
  -f net10.0-maccatalyst -p:RuntimeIdentifier=maccatalyst-arm64
```

If an Apple/.NET toolchain change leaves stale AOT outputs, add `-t:Rebuild`.
Keep each referenced project's normal target frameworks; overriding
`TargetFrameworks` globally can break restores for plain .NET dependencies.

Check the packaged description and signature, rather than just the source:

```sh
/usr/libexec/PlistBuddy -c 'Print :NSBluetoothAlwaysUsageDescription' \
  'Yijing.maui/bin/Release/net10.0-maccatalyst/maccatalyst-arm64/Yijing EEG.app/Contents/Info.plist'
codesign -d --entitlements - \
  'Yijing.maui/bin/Release/net10.0-maccatalyst/maccatalyst-arm64/Yijing EEG.app'
```

The description must be present and the signed entitlements must include
`com.apple.security.device.bluetooth`. Launch the Release app and use
Muse / Live to exercise the Bluetooth permission path.

Validation: a normal incremental arm64 Release build refreshed the previously
stale manifest. The signed Bluetooth entitlement was present. The resulting
Release app launched, connected to the physical Muse and displayed live EEG
data. The short test was stopped and Summary mode restored. iOS manifest input
inspection also confirmed that `Platforms/iOS/Info.plist` is now tracked.
