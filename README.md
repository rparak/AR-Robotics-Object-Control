# Augmented reality - Object control in the field of Robotics

## Requirements:

**Software:**
```bash
Unity3D 2020.1.8f1, Blender, Vuforia 8.3.8 and later, Visual Studio 2017/2019
```

**Supported on the following operating systems:**
```bash
Universal Windows Platform, Android
```

| Software/Package      | Link                                                                                  |
| --------------------- | ------------------------------------------------------------------------------------- |
| Blender               | https://www.blender.org/download/                                                     |
| Unity3D               | https://unity3d.com/get-unity/download/archive                                        |
| Vuforia Engine        | https://developer.vuforia.com/downloads/sdk                                           |
| Visual Studio         | https://visualstudio.microsoft.com/downloads/                                         |

## Project Description:
The principle of Augmented Reality (AR) consists in detecting a key object (in our case a QR code), which after successful localization, displays the 3D model on the screen of the mobile phone, tablet or computer together with the control panel of the object.

The augmented reality application was developed for simple control of objects (Universal Robots, ABB, etc.) and for a better understanding of robotic movements and 3D visualization. The application was created in Unity3D using the Vuforia Engine and tested on Android (Tablet, Phone) and Windows (USB Webcam). In our case, the application uses a simple QR code to detect and display the scene.

The augmented reality application demonstrates several cases of control:
- movement of joints / links, scaling, information about the object (robot), animation, etc.

The project was realized at Institute of Automation and Computer Science, Brno University of Technology, Faculty of Mechanical Engineering (NETME Centre - Cybernetics and Robotics Division).

<p align="center">
<img src="https://github.com/rparak/AR-Robotics-Object-Control/blob/main/images/2.PNG" width="700" height="350">
</p>

## Project Hierarchy:

**Repositary [/AR-Robotics-Object-Control/tree/main/ar_object_control/Assets/]:**
```bash
[ Object Control              ] /Script/ar_object_control.cs
[ Main Control (Global)       ] /Script/main_control_gVar.cs
[ Individual objects (.blend) ] /Object/
[ Images (UI)                 ] /Image/
[ Animation of the objects    ] /Animation/
[ Scene of the Application    ] /Scenes/ar_object_control
```

<p align="center">
<img src="https://github.com/rparak/AR-Robotics-Object-Control/blob/main/images/1.PNG" width="800" height="500">
</p>

## Augmented Reality Application:

<p align="center">
<img src="https://github.com/rparak/AR-Robotics-Object-Control/blob/main/images/AR_1.png" width="800" height="500">
</p>

<p align="center">
<img src="https://github.com/rparak/AR-Robotics-Object-Control/blob/main/images/ar_21.JPG" width="400" height="250">
<img src="https://github.com/rparak/AR-Robotics-Object-Control/blob/main/images/ar_22.JPG" width="400" height="250">
<img src="https://github.com/rparak/AR-Robotics-Object-Control/blob/main/images/ar_23.JPG" width="400" height="250">
<img src="https://github.com/rparak/AR-Robotics-Object-Control/blob/main/images/ar_24.JPG" width="400" height="250">
</p>


## Result:

Youtube: coming soon

## Contact Info:
Roman.Parak@outlook.com

## License
[MIT](https://choosealicense.com/licenses/mit/)
