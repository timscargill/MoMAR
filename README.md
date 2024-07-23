# MoMAR (Multiuser Motion Mapping for AR)
This repository contains resources and research artifacts for the paper "_Environment Texture Optimization for Augmented Reality_" that will appear in Proceedings of ACM IMWUT 2024. It includes the [code required to implement MoMAR](#implementation-resources).

To create the semi-synthetic VI-SLAM datasets that we used to study the effect of environment texture on AR pose tracking performance, we used our previously published game engine-based emulator, **Virtual-Inertial SLAM**. For more information on this tool, implementation code and instructions, and examples of the types of projects it can support, please visit the [Virtual-Inertial SLAM GitHub repository](https://github.com/timscargill/Virtual-Inertial-SLAM/).

# MoMAR Overview
Our MoMAR system provides situated visualizations of AR user motion patterns, to inform texture placement. Our code facilitates two visualization modes: 1)  illustrated in the image below -- 1) highlighting environment regions AR users face when they are focused on virtual content (left), 2) highlighting environment regions AR users face when they are performing challenging device motions (right):

![MoMAR motion map](https://github.com/timscargill/MoMAR/blob/main/MoMAR_MotionMap.png?raw=true)

The system architecture for MoMAR is shown below. To create a persistent AR experience for which an environment will be optimized, an environment administrator uses the map creation module on an admin AR device to generate a world map and place one or more spatial anchors within that space. These data are then transferred to and stored on the server. When a user starts a new session, the map retrieval module on the user AR device requests the map and anchor data from the map provisioning module on the server. These data are then used to localize the new session within the saved world map. Upon successful localization, the motion logging module on the user AR device is activated, which periodically sends device motion data to the server while the session is active. The motion analysis module on the server can be run on demand or periodically to analyze all user motion data, or those from a specified time range, to produce the motion map data. Finally, the data visualization module on the admin AR device is used to request motion map data from the server and display a motion map using situated visualizations.

![MoMAR system architecture](https://github.com/timscargill/MoMAR/blob/main/MoMAR_SystemArchitecture.png?raw=true)

# Implementation Resources

Our implementation code and associated resources for MoMAR are provided in three parts, for the **admin AR device**, the **server** and the **user AR device** respectively. The code for each can be found in the repository folders named '_admin-AR-device_', '_server_', and '_user-AR-device_'. The implementation resources consist of the following:
