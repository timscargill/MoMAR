# MoMAR (Multiuser Motion Mapping for AR)
This repository contains resources and research artifacts for the paper "_Environment Texture Optimization for Augmented Reality_" that will appear in Proceedings of ACM IMWUT 2024. You can find the the code required to implement MoMAR [here](#implementation-resources).

To create the semi-synthetic VI-SLAM datasets that we used to study the effect of environment texture on AR pose tracking performance, we used our previously published game engine-based emulator, **Virtual-Inertial SLAM**. For more information on this tool, implementation code and instructions, and examples of the types of projects it can support, please visit the [Virtual-Inertial SLAM GitHub repository](https://github.com/timscargill/Virtual-Inertial-SLAM/).

# MoMAR Overview
Our MoMAR system provides situated visualizations of AR user motion patterns, to inform environment designers where texture patches (e.g., posters or mats) should be placed in an environment. The premise of MoMAR is that distinct texture is required to support accurate pose tracking in regions users view while performing certain challenging motions with their AR device, but that fine, low-contrast textures (e.g., a carpet) are sufficient where users are slowly inspecting virtual content. Our code facilitates two visualization modes: 1)  illustrated in the image below -- 1) highlighting environment regions AR users face when they are focused on virtual content (left), 2) highlighting environment regions AR users face when they are performing challenging device motions (right):

![MoMAR motion map](https://github.com/timscargill/MoMAR/blob/main/MoMAR_MotionMap.png?raw=true)

The system architecture for MoMAR is shown below. To create a persistent AR experience, an administrator uses the map creation module on an admin AR device to generate a world map and place one or more spatial anchors within that space. These data are then transferred to and stored on the server. When a user starts a new session, the map retrieval module on the user AR device requests the map and anchor data from the map provisioning module on the server. These data are then used to localize the new session within the saved world map. Upon successful localization, the motion logging module on the user AR device is activated, which periodically sends device motion data to the server while the session is active. The motion analysis module on the server can be run on demand or periodically to analyze all user motion data, or those from a specified time range, to produce the motion map data. Finally, the data visualization module on the admin AR device is used to request motion map data from the server and display a motion map using situated visualizations.

![MoMAR system architecture](https://github.com/timscargill/MoMAR/blob/main/MoMAR_SystemArchitecture.png?raw=true)

# Implementation Resources

Our current MoMAR implementation is for ARKit (iOS) AR devices. The required code is provided in three parts, for the **admin AR device**, the **server** and the **user AR device** respectively. The code for each can be found in the repository folders named '_admin-AR-device_', '_server_', and '_user-AR-device_'. The implementation resources consist of the following:

**User AR device:** A C# script _DrawTrajectory.cs_, which implements the 'Trajectory creation' and 'Trajectory visualization' modules in SiTAR. Unity prefabs for base trajectory visualization, _Start.prefab_, _Stop.prefab_, _Cylinder.prefab_, _Joint.prefab_ and _Frustum.prefab_. Unity prefabs and materials for pose error visualizations, _ErrorAreaHigh.prefab_, _ErrorAreaMedium.prefab_, _ErrorPatchHigh.prefab_, _ErrorPatchMedium.prefab_, _ErrorHigh.mat_ and _ErrorMedium.mat_.   

**Server:** a Python script _SiTAR-Server.py_, which implements the 'Sequence assignment' and 'Uncertainty-based error estimation' modules in SiTAR.

**Playback AR device:** a C# script _TrajectoryPlayback.cs_, which implements the 'Sequence playback' module in SiTAR.


# Implementation Instructions

**Prerequisites:** 1 or more iOS or iPad OS devices running iOS/iPad OS 15 or above, and a server with Python 3.8 or above and and FastAPI (https://fastapi.tiangolo.com/lo/) Python packages installed. For building the necessary apps to AR devices, Unity 2021.3 or later is required, with the AR Foundation framework v4.2 or later installed.

Tested with an iPhone 13 (iOS 16), an iPhone 13 Pro Max (iOS 15), an iPhone 14 Pro Max (iOS 17), an iPad Pro 2nd gen. (iPad OS 17, and an iPad Pro 4th gen. (iPad OS 16) as AR devices, and a desktop PC with an Intel i7-9700K CPU and an Nvidia GeForce RTX 2060 GPU as an edge server (Python 3.8).

**Admin AR device:** 
1) Create a Unity project with the AR Foundation template. Make sure the ARCore Extensions is fully set up by following the instructions here: https://developers.google.com/ar/develop/unity-arf/getting-started-extensions.
2) Add the _DrawTrajectory.cs_ script (in the _user-AR-device_ folder) to the AR Session Origin GameObject.
3) Drag the AR Camera GameObject to the 'Camera Manager' and 'Camera' slots in the Draw Trajectory inspector panel.
4) Add the _Start.prefab_, _Stop.prefab_, _Cylinder.prefab_, _Joint.prefab_ and _Frustum.prefab_ files (in the _user-AR-device_ folder) to your Assets folder, and drag them to the 'Start Prefab', 'Stop Prefab', 'Cylinder Prefab', 'Joint Prefab' and 'Frustum Prefab' slots in the Draw Trajectory inspector panel.
5) (Optional) If using the exclamation points or warning signs visualizations, add the _ErrorAreaHigh.prefab_, _ErrorAreaMedium.prefab_, _ErrorPatchHigh.prefab_, and _ErrorPatchMedium.prefab_ files (in the _user-AR-device_ folder) to your Assets folder, and drag them to the 'Error Area High Prefab', 'Error Area Medium Prefab', 'Error Patch High Prefab', and 'Error Patch Medium Prefab' slots in the Draw Trajectory inspector panel.
7) Add the _ErrorHigh.mat_ and _ErrorMedium.mat_ files (in the _user-AR-device_ folder) to your Assets folder, and drag them to the 'Error High' and 'Error Medium' slots in the Draw Trajectory inspector panel.
8) Add Start and Stop UI buttons, drag them to the 'Start Button' and 'Stop Button' slots in the Draw Trajectory inspector panel, and set their OnClick actions to 'DrawTrajectory.HandleStartClick' and 'DrawTrajectory.HandleStopClick' respectively.
9) Either hardcode your server IP address into line 481 of _DrawTrajectory.cs_, or add a UI panel with a text field to capture this data from the user.
10) (Optional) Add UI text objects to display SiTAR status, trajectory duration, length, average environment depth, and drag them to the 'Status', 'Trajectory Duration', 'Trajectory Length' and 'Trajectory Depth' slots in the Draw Trajectory inspector panel.
11) (Optional) Add audio clips for notifying when error estimates are ready, user captures image, and user has captured all regions, and drag them to the 'Audio Results', 'Audio Capture' and 'Audio Complete' slots in the Draw Trajectory inspector panel.
12) Set the Build platform to Android, select your device under Run device, and click Build and Run.

**Server:**
1) Create a folder on the server where SiTAR files will be located. Add an additional sub-folder named 'trajectories'.
2) Download the _server_ folder in the repository to your SiTAR folder.
3) Open the _SiTAR-Server.py_ file in the _server_ folder, complete the required configuration parameters on lines 20-29, and save.
4) In Terminal or Command Prompt, navigate to your SiTAR folder.
5) Start the server using the following command: ```uvicorn server.SiTAR-Server:app --host 0.0.0.0```

**User AR device:**
1) Create a Unity project with the AR Foundation template. Make sure the ARCore Extensions is fully set up by following the instructions here: https://developers.google.com/ar/develop/unity-arf/getting-started-extensions.
2) (Optional) Add the AR Plane Manager and AR Point Cloud Manager scripts (included in AR Foundation) to the AR Session Origin GameObject if you wish to visualize planes and feature points during playback.
3) Add the _TrajectoryPlayback.cs_ script (in the _playback-AR-device_ folder) to the AR Session GameObject.
4) Create a UI text object to display log messages, and drag it to the 'Log' slot in the Trajectory Playback inspector panel.
5) Drag the AR Camera GameObject to the 'Camera Manager' and 'Camera' slots in the Trajectory Playback inspector panel.
6) Set the Build platform to Android, select your device under Run device, and click Build and Run.

# Citation

If you use MoMAR in an academic work, please cite: 

```
@inproceedings{MoMAR,
  title={Environment texture optimization for augmented reality},
  author={Scargill, Tim and Janamsetty, Ritvik and Fronk, Christian and Eom, Sangjun and Gorlatova, Maria},
  booktitle={Proceedings of ACM IMWUT 2024},
  year={2024}
 }
 ```

# Acknowledgements 

The authors of this repository are Tim Scargill and Maria Gorlatova. Contact information of the authors:

* Tim Scargill (timothyjames.scargill AT duke.edu)
* Maria Gorlatova (maria.gorlatova AT duke.edu)

This work was supported in part by NSF grants CNS-1908051, CNS-2112562, CSR-2312760 and IIS-2231975, NSF CAREER Award IIS-2046072, a Meta Research Award and a CISCO Research Award. 
