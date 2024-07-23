# MoMAR (Multiuser Motion Mapping for AR)
This repository contains resources and research artifacts for the paper "_Environment Texture Optimization for Augmented Reality_" that will appear in Proceedings of ACM IMWUT 2024. You can find the the code required to implement MoMAR [here](#implementation-resources).

To create the semi-synthetic VI-SLAM datasets that we used to study the effect of environment texture on AR pose tracking performance, we used our previously published game engine-based emulator, **Virtual-Inertial SLAM**. For more information on this tool, implementation code and instructions, and examples of the types of projects it can support, please visit the [Virtual-Inertial SLAM GitHub repository](https://github.com/timscargill/Virtual-Inertial-SLAM/).

# MoMAR Overview
Our MoMAR system provides situated visualizations of AR user motion patterns, to inform environment designers where texture patches (e.g., posters or mats) should be placed in an environment. The premise of MoMAR is that distinct texture is required to support accurate pose tracking in regions users view while performing certain challenging motions with their AR device, but that fine, low-contrast textures (e.g., a carpet) are sufficient where users are slowly inspecting virtual content. Our code facilitates two visualization modes, illustrated in the image below: 1) highlighting environment regions AR users face when they are focused on virtual content (left), 2) highlighting environment regions AR users face when they are performing challenging device motions (right).

![MoMAR motion map](https://github.com/timscargill/MoMAR/blob/main/MoMAR_MotionMap.png?raw=true)

The system architecture for MoMAR is shown below. To create a persistent AR experience, an administrator uses the map creation module on an admin AR device to generate a world map and place one or more spatial anchors within that space. These data are then transferred to and stored on the server. When a user starts a new session, the map retrieval module on the user AR device requests the map and anchor data from the map provisioning module on the server. These data are then used to localize the new session within the saved world map. Upon successful localization, the motion logging module on the user AR device is activated, which periodically sends device motion data to the server while the session is active. The motion analysis module on the server can be run on demand or periodically to analyze all user motion data, or those from a specified time range, to produce the motion map data. Finally, the data visualization module on the admin AR device is used to request motion map data from the server and display a motion map using situated visualizations.

![MoMAR system architecture](https://github.com/timscargill/MoMAR/blob/main/MoMAR_SystemArchitecture.png?raw=true)

# Implementation Resources

Our current MoMAR implementation is for ARKit (iOS) AR devices. The required code is provided in three parts, for the **admin AR device**, the **server** and the **user AR device** respectively. The code for each can be found in the repository folders named '_admin-AR-device_', '_server_', and '_user-AR-device_'. The implementation resources consist of the following:

**Admin AR device:** the C# scripts _PlaceAnchorOnPlane.cs_ and _ARWorldMapController.cs_, which implement the 'Map creation' module in MoMAR. The C# script _DataVisualization.cs_, which implements the 'Data visualization' module in MoMAR.

**Server:** a Python script _MoMAR-Server.py_, which implements the 'Map provisioning' module and handles HTTP POST and GET requests between the AR devices and the server, and a Python script _motionAnalysis.py_, which implements the 'Motion analysis' module.

**User AR device:** the C# scripts _RenderAnchorContent.cs_ and _ARWorldMapController.cs_, which implement the 'Map retrieval' module in MoMAR. _RenderAnchorContent.cs_ also provides a framework for how virtual content can be rendered based on the contents of the anchor dictionary. The C# script _MotionLog.cs_, which implements the 'Motion logging' module in MoMAR.

# Implementation Instructions

**Prerequisites:** 1 or more iOS or iPad OS devices running iOS/iPad OS 15 or above, and an edge server with Python 3.8 or above and and FastAPI (https://fastapi.tiangolo.com/lo/) Python packages installed. For building the necessary apps to AR devices, Unity 2021.3 or later is required, with the AR Foundation framework v4.2 or later installed.

Tested with an iPhone 13 (iOS 16), an iPhone 13 Pro Max (iOS 15), an iPhone 14 Pro Max (iOS 17), an iPad Pro 2nd gen. (iPad OS 17, and an iPad Pro 4th gen. (iPad OS 16) as AR devices, and a desktop PC with an Intel i7-9700K CPU and an Nvidia GeForce RTX 2060 GPU as an edge server (Python 3.8).

**Admin AR device:** 
1) Create a Unity project with the AR Foundation template.
2) Add the AR Anchor Manager script (provided with the AR Foundation template) to the AR Session Origin GameObject.
3) Add the _PlaceAnchorOnPlane.cs_ script (in the _admin-AR-device_ folder) to the AR Session Origin GameObject. Insert the IP address of your edge server on line 365.
4) Add UI canvas buttons to handle placing different anchors, and link each button to the appropriate method in _PlaceAnchorOnPlane.cs_ (e.g., _PlaceAnchorA()_).
5) Add the _DataVisualization.cs_ script (in the _admin-AR-device_ folder) to the AR Session Origin GameObject. Insert the IP address of your edge server on line 65 and line 96.
6) Create a new GameObject 'ARWorldMapController'. Drag the AR Session Game Object to the appropriate slot in the inspector. Add the _ARWorldMapController.cs_ script (in the _admin-AR-device_ folder, adapted from the example in the AR Foundation template) to the ARWorldMapController GameObject. Insert the IP address of your edge server on line 179 and line 289.
7) Set the Build platform to iOS, and click Build.
8) Load your built project in XCode, sign it using your Apple Developer ID, and run it on your admin AR device.

**Server:**
1) Create a folder on the server where MoMAR files will be located.
2) Download the _server_ folder in the repository to your new MoMAR folder.
3) In Terminal or Command Prompt, navigate to your MoMAR folder.
4) Start the server using the following command: ```uvicorn server.MoMAR-Server:app --host 0.0.0.0```. The server will now facilitate the setup and loading of your persistent AR experience, and capture motion log data. The motion log data will be appended to a new file in your MoMAR folder, 'motionlog.txt'.
5) When you wish to run motion analysis (required before data visualization), run the _motionAnalysis.py_ script, which will create the 'motionData.csv' file. This file is downloaded by the data visualization module on the admin AR device. 

**User AR device:**
1) Create a Unity project with the AR Foundation template.
2) Add the AR Anchor Manager script (provided with the AR Foundation template) to the AR Session Origin GameObject.
3) Add the _RenderAnchorContent.cs_ script (in the _user-AR-device_ folder) to the AR Session Origin GameObject. Add code to declare and instantiate the specific GameObjects and Colliders you wish to render as indicated in the script. Insert the IP address of your edge server on line 66.
4) Drag the Prefabs you wish to render to the appropriate places in the inspector for the _RenderAnchorContent.cs_ script.
6) Add the _MotionLog.cs_ script (in the _user-AR-device_ folder) to the AR Session Origin GameObject. Insert the IP address of your edge server on line 131.
7) Create a new GameObject 'ARWorldMapController'. Drag the AR Session Game Object to the appropriate slot in the inspector. Add the _ARWorldMapController.cs_ script (in the _admin-AR-device_ folder, adapted from the example in the AR Foundation template) to the ARWorldMapController GameObject. Insert the IP address of your edge server on line 179 and line 289.
8) Set the Build platform to iOS, and click Build.
9) Load your built project in XCode, sign it using your Apple Developer ID, and run it on your admin AR device.

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
