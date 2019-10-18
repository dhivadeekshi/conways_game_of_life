# **Unity 3D** simulation of Conway's game of life
To understand what is Conways game of life refer [Conway's game of life](../). This simulation is made using Unity.
![Unity](unity-master-black.png)

## Tools
- Unity 3D - v5.6.7f1 Download : [x64](https://download.unity3d.com/download_unity/e80cc3114ac1/Windows64EditorInstaller/UnitySetup64-5.6.7f1.exe?_ga=2.129590991.2141448923.1571369570-1343944484.1569318718), [x86](https://download.unity3d.com/download_unity/e80cc3114ac1/Windows32EditorInstaller/UnitySetup32-5.6.7f1.exe?_ga=2.20655099.2141448923.1571369570-1343944484.1569318718)
- Java SDK - v8 [Download](https://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html)
- Android SDK - latest [Download](https://developer.android.com/studio)

## Setup
1. Install Unity version mentioned above

#### Aditional Steps for Android
1. Install JDK version mentioned above
1. Install latest Android SDK
1. Set **JAVA_HOME** as environment variable to the JDK installed path 
1. Set **JDK** path in Unity preferences -> External Tools
1. Set **Android SDK** path in Unity preferences -> External Tools

## How to build
#### PC
  1. Switch to ***Standalone PC*** platform in Unity build settings
  1. Press **Build** on the build settings and select the desired path
  1. Launch the generated ***\*.exe*** file to run
#### Android
  1. Switch to ***Android*** platform in Unity build settings
  1. Press **Build** on the build settings and select the desired path
  1. Install the generated ***\*.apk*** to a device / simulator to run
#### WebGL
  1. Switch to ***WebGL*** platform in Unity build settings
  1. Press **Build** on the build settings and select the desired path
  1. Launch the _index.html_ file using **Edge browser** to run