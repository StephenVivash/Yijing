# Yijing

## Yijing - The classic of change, the game of koans, play for the system.

## I Ching - Classification, generalisation and chance.

## Yi Jing - Judgement, order and contemplation.

## Zhouyi - Divination, prognostication and synchronicity.

https://apps.microsoft.com/detail/9n9hhltgvjsk?cid=DevShareMTwPCS&hl=en-US&gl=AU

## .NET 11 development

Clone Yijing and [Common](https://github.com/StephenVivash/Common) side by side.
Use Common's .NET 11 checkout with the shared Apple scene files under `Build/Apple`.
Yijing selects the same .NET 11.0.1xx SDK and workload set as Common. On macOS,
use Xcode 27.2 with that workload set. The Mac Catalyst build requires macOS 14
or later. Android also needs an Android SDK and JDK.

```sh
dotnet workload restore Yijing.maui/Yijing.maui.csproj
dotnet build Yijing.maui/Yijing.maui.csproj -f net11.0-maccatalyst -r maccatalyst-arm64
dotnet build Yijing.maui/Yijing.maui.csproj -f net11.0-ios -r iossimulator-arm64
dotnet test --project Yijing.test/Yijing.test.csproj
dotnet test --project Yijing.maui.test/Yijing.maui.test.csproj
```

For toolbar icons, place your local copy of `SegoeIcons.ttf` in
`Yijing.maui/Resources/Fonts`. Git ignores this file.

## Releases

* v7.0.8 (17-May-26)
 - Muse BT interface
 - Bug fixes
* v7.0.0 (2023) [C#, MAUI]
* v6.1.0 (2017) [Java, SWT]
* v6.0.0 (2016) [Java, Android5]
* v5.2.0 (2016) [C#, Windows10]
* v5.1.0 (2014) [C#, Windows8]
* v5.0.0 (2012) [C#, Windows7]
* v4.0.0 (2007) [VB, WindowsCE]
* v3.0.0 (2003) [C++, WindowsXP]
* v2.0.0 (1987) [Pascal, MS DOS]
* v1.0.0 (1985) [BASIC, Apple DOS]
* v0.0.0 (-3500) [Turtle Graphics]
