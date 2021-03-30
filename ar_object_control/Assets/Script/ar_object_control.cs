/****************************************************************************

MIT License

Copyright(c) 2020 Roman Parak

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*****************************************************************************

Author   : Roman Parak
Email    : Roman.Parak @outlook.com
Github   : https://github.com/rparak
File Name: ar_object_control.cs

****************************************************************************/

// ------------------------------------------------------------------------------------------------------------------------//
// ----------------------------------------------------- LIBRARIES --------------------------------------------------------//
// ------------------------------------------------------------------------------------------------------------------------//

// -------------------- System -------------------- //
using System;
// -------------------- Unity -------------------- //
using UnityEngine;
using UnityEngine.UI;
// -------------------- TMPro -------------------- //
using TMPro;

public class ar_object_control : MonoBehaviour
{
    // ************************************************ //
    // -------------------- PUBLIC -------------------- //
    // ************************************************ //

    // -------------------- Image -------------------- //
    public Image information_panel_image, control_panel_image, animation_panel_image;
    // -------------------- Slider -------------------- //
    public Slider scale_cp;
    // -------------------- TextMeshProUGUIT -------------------- //
    public TextMeshProUGUI[] r_param = new TextMeshProUGUI[7];
    public TextMeshProUGUI info_title, info_main_txt;
    // -------------------- Animator -------------------- //
    public Animator anim_abb, anim_smcT, anim_UR3;
    // -------------------- GameObject -------------------- //
    public GameObject UR3_Base;
    public GameObject ABB_IRB120_Base;
    public GameObject axis_7th_children_base;
    public GameObject SMCtrak_base;
    public GameObject stage1_go, stage2_go, stage3_go;

    // ************************************************ //
    // -------------------- PRIVATE ------------------- //
    // ************************************************ //

    // -------------------- Bool -------------------- //
    private bool stage_1_v3dB, stage_2_v3dB, stage_3_v3dB;
    private bool reset_cpB, info_cpB, info_cbB, control_cbB;
    private bool disass_apB, move_apB, stop_apB;
    // -------------------- Int -------------------- //
    private int counter_infoB = 0;
    private int counter_valueF_info;
    private int min_page_v = 0;
    private int max_page_v = 5;
    // -------------------- String -------------------- //
    private string URL = "http://uai.fme.vutbr.cz/en/";
    // -------------------- GameObject-------------------- //
    private GameObject[] joint_UR3 = new GameObject[6];
    private GameObject[] joint_ABB_IRB120 = new GameObject[6];
    private GameObject joint_7th_axis;
    private GameObject[] joint_smcT = new GameObject[2];
    // -------------------- Float -------------------- //
    // Initialization Position (7th Axis)
    private float[] init_pos_7thAx = { -371.2827f, 61.08827f, -4.083694f };
    // Initialization Position (SMCTrak)
    private float[,] init_pos_smcT = new float[,] {
        {0.005264282f, 25.58661f, 729.0347f},
        {0.002716064f, 25.59161f, 729.0468f},
    };
    private float ex_param = 100f;
    // -------------------- Vector3 -------------------- //
    private Vector3 set_position_UR3;
    private Vector3 set_position_ABB_IRB120;
    private Vector3 set_position_7th_axis;
    private Vector3[] set_position_smcT = new Vector3[2];

    // ------------------------------------------------------------------------------------------------------------------------//
    // ------------------------------------------------ INITIALIZATION {START} ------------------------------------------------//
    // ------------------------------------------------------------------------------------------------------------------------//
    void Start()
    {
        // ------------------------ Initialization Panel ------------------------//
        // Information Panel -> visible (off)
        information_panel_image.transform.localPosition = new Vector3(-550f + (ex_param * 100), 15f, 0f);
        // Animation Panel -> visible (off)
        animation_panel_image.transform.localPosition = new Vector3(-500f + (ex_param * 100), 50f, 0f);
        // Control Panel -> visible (off)
        control_panel_image.transform.localPosition = new Vector3(-833f + (ex_param * 100), 90f, 0f);

        // ------------------------ Initialization Object ------------------------//
        // Stage 1 (UR3)
        stage1_go.transform.localPosition = new Vector3(-298.8625f, -351.417f, 0f);
        stage1_go.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        stage1_go.transform.localScale = new Vector3(1f, 1f, 1f);
        // Stage 2 (ABB IRB 120)
        stage2_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
        stage2_go.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        stage2_go.transform.localScale = new Vector3(1f, 1f, 1f);
        // Stage 3 (SMCTrak)
        stage3_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
        stage3_go.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        stage3_go.transform.localScale = new Vector3(1f, 1f, 1f);

        // ------------------------ Initialization Text (Information Panel) ------------------------//
        // title
        info_title.text    = "Institute of Automation and Computer Science";
        // main text
        info_main_txt.text = "Main research activities are focused on the following areas: Mobile robot construction and control (omni directional wheels, electrical sensors, control and navigation systems, trajectory generation, motion control), mechatronics (identification and simulation of dynamic system parameters via genetic algorithm and neural networks), automatic control systems (linear and non-linear automatic control, digital automatic control, binary and optimal control, robust control, discrete PSD controllers, large-scale systems), mathematical modelling and methods in project management (processes of identification, multi-criteria selection, scheduling, monitoring and realisation of projects), production management (scheduling and lot sizing in flow shops and job shops), soft computing (fuzzy logic, neural networks, evolutionary and hybrid algorithms, knowledge based reasoning). Additionally, the Institute is responsible for design, development and administration of the Faculty network. The Institute organises the International Mendel Conference on Soft Computing every year.";

        // ------------------------ Initialization Text (Object Control Panel) ------------------------//
        for (int i = 0; i < GlobalVariables_stage_1_UR3.max_joints; i++)
        {
            r_param[i].text = (GlobalVariables_stage_1_UR3.actual_jPos[i] / 100).ToString();
        }

        // first -> go on stage no. 1
        stage_1_v3dB = true;

        // animation fce. is stopped
        stop_apB = true;

        // initialization main GameObjects (robots, linear conveyors, ..)
        initialization_joints_UR3();
        initialization_joints_ABB_IRB120();
        initialization_joint_7th_axis();
        initialization_joints_SMCtrak();
    }

    // ------------------------------------------------------------------------------------------------------------------------//
    // ------------------------------------------------ MAIN FUNCTION {Cyclic} ------------------------------------------------//
    // ------------------------------------------------------------------------------------------------------------------------//
    void Update()
    {
        // ------------------------ RESET ALL 3D View ------------------------//
        // Reset all (go to initialization position of each panels, rect, etc.)
        if (reset_cpB == true || (stage_1_v3dB == false && stage_2_v3dB == false && stage_3_v3dB == false))
        {
            // ------------------------ Initialization Object ------------------------//
            // Stage 1 (UR3)
            stage1_go.transform.localPosition = new Vector3(4973.2f, -850.4f, -1190.002f);
            stage1_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            stage1_go.transform.localScale = new Vector3(1f, 1f, 1f);
            // Stage 2 (ABB IRB 120)
            stage2_go.transform.localPosition = new Vector3(4973.2f, -850.4f - ex_param * 10, -1190.002f);
            stage2_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            stage2_go.transform.localScale = new Vector3(1f, 1f, 1f);
            // Stage 3 (SMCTrak)
            stage3_go.transform.localPosition = new Vector3(4973.2f, -351.417f - ex_param * 10, -1190.002f);
            stage3_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            stage3_go.transform.localScale = new Vector3(1f, 1f, 1f);

            // ------------------------ Read Text (Object Control Panel) ------------------------//
            for (int i = 0; i < GlobalVariables_stage_1_UR3.max_joints; i++)
            {
                r_param[i].text = (GlobalVariables_stage_1_UR3.actual_jPos[i] / 100).ToString();
            }

            // ------------------------ Initialization Text (Information Panel) ------------------------//
            // title
            info_title.text = "Institute of Automation and Computer Science";
            // main text
            info_main_txt.text = "Main research activities are focused on the following areas: Mobile robot construction and control (omni directional wheels, electrical sensors, control and navigation systems, trajectory generation, motion control), mechatronics (identification and simulation of dynamic system parameters via genetic algorithm and neural networks), automatic control systems (linear and non-linear automatic control, digital automatic control, binary and optimal control, robust control, discrete PSD controllers, large-scale systems), mathematical modelling and methods in project management (processes of identification, multi-criteria selection, scheduling, monitoring and realisation of projects), production management (scheduling and lot sizing in flow shops and job shops), soft computing (fuzzy logic, neural networks, evolutionary and hybrid algorithms, knowledge based reasoning). Additionally, the Institute is responsible for design, development and administration of the Faculty network. The Institute organises the International Mendel Conference on Soft Computing every year.";

            // Change actual state -> Stage 1 (initial state)
            stage_1_v3dB = true;

            // reset -> scale 
            scale_cp.value = 0;

            // reset -> false
            reset_cpB = false;

            // animation control (enabled -> true)
            anim_UR3.enabled  = true;
            anim_abb.enabled  = true;
            anim_smcT.enabled = true;
        }

        if (stage_1_v3dB == true)
        {
            // ------------------------ Control Actual Object (UR3) ------------------------//
            // Stage 1 (UR3)
            stage1_go.transform.localPosition = new Vector3(4973.2f, -850.4f, -1190.002f);
            stage1_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            //  Slider Control (Scale of the object) //
            stage1_go.transform.localScale = new Vector3(scale_cp.value / 100, scale_cp.value / 100, scale_cp.value / 100);
            // Stage 2 (ABB + 7th axis)
            stage2_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
            stage2_go.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            stage2_go.transform.localScale = new Vector3(1f, 1f, 1f);
            // Stage 3 (SMCTrak)
            stage3_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
            stage3_go.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            stage3_go.transform.localScale = new Vector3(1f, 1f, 1f);

            // Animation is stopped
            if (stop_apB == true)
            {
                // ------------------------ Move UR3 { Joint (1 - 6) } ------------------------//
                for (int i = 0; i < GlobalVariables_stage_1_UR3.max_joints; i++)
                {
                    if (i == 0 || i == 4)
                    {
                        // Position {TCP/IP Control -> joint_1 - joint_6 (TCPip_read_thread_function function)}
                        // Read Actual Position
                        set_position_UR3 = joint_UR3[i].transform.localEulerAngles;
                        //Debug.Log(set_position_UR3);
                        set_position_UR3.x = 0;
                        set_position_UR3.y = (-1) * GlobalVariables_stage_1_UR3.actual_jPos[i] / 100;
                        set_position_UR3.z = 0;
                        // Set New Position
                        joint_UR3[i].transform.localEulerAngles = set_position_UR3;

                    }
                    else
                    {
                        // Position {TCP/IP Control -> joint_1 - joint_6 (TCPip_read_thread_function function)}
                        // Read Actual Position
                        set_position_UR3 = joint_UR3[i].transform.localEulerAngles;
                        //Debug.Log(set_position_UR3);
                        set_position_UR3.x = 0;
                        set_position_UR3.y = 0;

                        if (i == 1 || i == 3)
                        {
                            set_position_UR3.z = 90f + GlobalVariables_stage_1_UR3.actual_jPos[i] / 100;
                        }
                        else
                        {
                            set_position_UR3.z = GlobalVariables_stage_1_UR3.actual_jPos[i] / 100;
                        }
                        // Set New Position
                        joint_UR3[i].transform.localEulerAngles = set_position_UR3;
                    }

                    // read actual position -> transform to text
                    r_param[i].text = (GlobalVariables_stage_1_UR3.actual_jPos[i] / 100).ToString();
                }
            }
        }
        else if (stage_2_v3dB == true)
        {
            // ------------------------ Control Actual Object (ABB IRB 120, 7th Axis) ------------------------//
            // Stage 2 (ABB + 7th axis)
            stage2_go.transform.localPosition = new Vector3(4973.2f, -850.4f, -1190.002f);
            stage2_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            //  Slider Control (Scale of the object) //
            stage2_go.transform.localScale = new Vector3(scale_cp.value / 100, scale_cp.value / 100, scale_cp.value / 100);
            // Stage 1 (Ur3)
            stage1_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
            stage1_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            stage1_go.transform.localScale = new Vector3(1f, 1f, 1f);
            // Stage 3 (SMCtrak)
            stage3_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
            stage3_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            stage3_go.transform.localScale = new Vector3(1f, 1f, 1f);

            // Animation is stopped
            if (stop_apB == true)
            {
                // ------------------------ Move ABB IRB 120 { Joint (1 - 6) } ------------------------//
                for (int i = 0; i < GlobalVariables_stage_2_ABB.max_joints; i++)
                {
                    if (i == 0)
                    {
                        // Position {RWS Control -> joint1 - joint6 (display_data function)}
                        // Read Actual Position
                        set_position_ABB_IRB120 = joint_ABB_IRB120[i].transform.localEulerAngles;
                        //Debug.Log(set_position_ABB_IRB120);
                        set_position_ABB_IRB120.x = 0;
                        set_position_ABB_IRB120.y = 0;
                        set_position_ABB_IRB120.z = (-1) * (GlobalVariables_stage_2_ABB.actual_jPos[i] / 100);
                        // Set New Position
                        joint_ABB_IRB120[i].transform.localEulerAngles = set_position_ABB_IRB120;
                    }
                    else if (i == 3 || i == 5)
                    {
                        // Position {RWS Control -> joint1 - joint6 (display_data function)}
                        // Read Actual Position
                        set_position_ABB_IRB120 = joint_ABB_IRB120[i].transform.localEulerAngles;
                        //Debug.Log(set_position_ABB_IRB120);
                        set_position_ABB_IRB120.x = (GlobalVariables_stage_2_ABB.actual_jPos[i] / 100);
                        set_position_ABB_IRB120.y = 0;
                        set_position_ABB_IRB120.z = 0;
                        // Set New Position
                        joint_ABB_IRB120[i].transform.localEulerAngles = set_position_ABB_IRB120;
                    }
                    else if (i == 1 || i == 2 || i == 4)
                    {
                        // Position {RWS Control -> joint1 - joint6 (display_data function)}
                        // Read Actual Position
                        set_position_ABB_IRB120 = joint_ABB_IRB120[i].transform.localEulerAngles;
                        //Debug.Log(set_position_ABB_IRB120);
                        set_position_ABB_IRB120.x = 0;
                        set_position_ABB_IRB120.y = (-1) * (GlobalVariables_stage_2_ABB.actual_jPos[i] / 100);
                        set_position_ABB_IRB120.z = 0;
                        // Set New Position
                        joint_ABB_IRB120[i].transform.localEulerAngles = set_position_ABB_IRB120;
                    }else if (i == 6)
                    {
                        // ------------------------ Move 7th Axis {ABB IRB 120} ------------------------//
                        // Position {OPCUa Control -> axis_7th_pos}
                        // Read Actual Position
                        set_position_7th_axis = joint_7th_axis.transform.localPosition;
                        set_position_7th_axis.x = init_pos_7thAx[0] + Math.Abs(GlobalVariables_stage_2_ABB.actual_jPos[i] / 100);
                        set_position_7th_axis.y = init_pos_7thAx[1];
                        set_position_7th_axis.z = init_pos_7thAx[2];
                        // Set New Position
                        joint_7th_axis.transform.localPosition = set_position_7th_axis;
                    }
                    // read actual position -> transform to text
                    r_param[i].text = (GlobalVariables_stage_2_ABB.actual_jPos[i] / 100).ToString();
                }
            }
        }
        else if (stage_3_v3dB == true)
        {
            // ------------------------ Control Actual Object (SMCTrak) ------------------------//
            // Stage 3 (SMCTrak)
            stage3_go.transform.localPosition = new Vector3(4973.2f, -850.4f, -1190.002f);
            stage3_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            //  Slider Control (Scale of the object) //
            stage3_go.transform.localScale = new Vector3(scale_cp.value / 100, scale_cp.value / 100, scale_cp.value / 100);
            // Stage 1 (Ur3)
            stage1_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
            stage1_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            stage1_go.transform.localScale = new Vector3(1f, 1f, 1f);
            // Stage 2 (ABB + 7th axis)
            stage2_go.transform.localPosition = new Vector3(-298.8628f, -351.417f - ex_param * 10, 0f);
            stage2_go.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            stage2_go.transform.localScale = new Vector3(1f, 1f, 1f);


            // Animation is stopped
            if (stop_apB == true)
            {
                // ------------------------ Move SMCTrak {SMC PAD -> 0, BR PAD -> 1} ------------------------//
                for (int i = 0; i < GlobalVariables_stage_3_SMCt.max_joints; i++)
                {
                    // Position {OPCUa Control -> smcT_pad_pos}
                    // Read Actual Position
                    set_position_smcT[i] = joint_smcT[i].transform.localPosition;
                    set_position_smcT[i].x = init_pos_smcT[i, 0];
                    set_position_smcT[i].y = init_pos_smcT[i, 1];
                    set_position_smcT[i].z = init_pos_smcT[i, 2] - Math.Abs(GlobalVariables_stage_3_SMCt.actual_jPos[i] / 100);
                    // Set New Position
                    joint_smcT[i].transform.localPosition = set_position_smcT[i];

                    // read actual position -> transform to text
                    r_param[i].text = (GlobalVariables_stage_3_SMCt.actual_jPos[i] / 100).ToString();
                }
            }
        }

        // ------------------------ Text Control (Information Panel) ------------------------//
        // Text (IACS, B&R, SMC, UR, ABB)
        switch (counter_valueF_info)
        {
            case 0:
                {
                    // ----- Institute of Automation and Computer Science ----- //
                    // title
                    info_title.text = "Institute of Automation and Computer Science";
                    // main text
                    info_main_txt.text = "Main research activities are focused on the following areas: Mobile robot construction and control (omni directional wheels, electrical sensors, control and navigation systems, trajectory generation, motion control), mechatronics (identification and simulation of dynamic system parameters via genetic algorithm and neural networks), automatic control systems (linear and non-linear automatic control, digital automatic control, binary and optimal control, robust control, discrete PSD controllers, large-scale systems), mathematical modelling and methods in project management (processes of identification, multi-criteria selection, scheduling, monitoring and realisation of projects), production management (scheduling and lot sizing in flow shops and job shops), soft computing (fuzzy logic, neural networks, evolutionary and hybrid algorithms, knowledge based reasoning).";
                    // Webpage URL
                    URL = "http://uai.fme.vutbr.cz/en/";
                }
                break;
            case 1:
                {
                    // ----- B&R Automation----- //
                    // title
                    info_title.text = "B&R Automation";
                    // main text
                    info_main_txt.text = "B&R Industrial Automation GmbH is an Austrian automation and process control technology company. It was founded in 1979 by Erwin Bernecker and Josef Rainer, and is headquartered in Eggelsberg, near Braunau in the state of Upper Austria. The company specializes in machine and factory control systems, HMI and motion control. In addition to scalable complete solutions, B&R also offers individual components.\n\nThe product range is oriented toward machinery and equipment manufacturing, and the company is also active in the field of factory and process automation.";
                    // Webpage URL
                    URL = "https://www.br-automation.com";
                }
                break;
            case 2:
                {
                    // ----- ABB Robotics" ----- //
                    // title
                    info_title.text = "ABB Robotics";
                    // main text
                    info_main_txt.text = "ABB, formerly ASEA Brown Boveri, is a Swiss-Swedish multinational corporation headquartered in Zurich, Switzerland and Västerås, Sweden. operating mainly in robotics, power, heavy electrical equipment, and automation technology areas. It is ranked 341st in the Fortune Global 500 list of 2018 and has been a global Fortune 500 company for 24 years.\n\nABB is a leading global technology company that energizes the transformation of society and industry to achieve a more productive, sustainable future. By connecting software to its electrification, robotics, automation and motion portfolio, ABB pushes the boundaries of technology to drive performance to new levels. With a history of excellence stretching back more than 130 years, ABB’s success is driven by about 110,000 talented employees in over 100 countries.";
                    // Webpage URL
                    URL = "https://global.abb/group/en";
                }
                break;
            case 3:
                {
                    // ----- SMC Industrial Automation ----- //
                    // title
                    info_title.text = "SMC Industrial Automation";
                    // main text
                    info_main_txt.text = "SMC is pursuing worldwide customer satisfaction and supporting automation through the most advanced pneumatic and electric technologies.\n\nAs a worldwide leading company and with an engineering staff exceeding 1,500 persons, SMC provides you the best expertise and support for your automation projects accross more than 80 countries.\n\nBe ensured that we have the right component or solution to fit your need with more than 12, 000 products and 700, 000 variations available.Located in 28 countries in Europa, we provide a fast delivery and competitive prices due to our unique production system, and by maximizing our local production capabilities, a stable supply of product is guaranteed.";
                    // Webpage URL
                    URL = "https://www.smc.eu/en-eu";
                }
                break;
            case 4:
                {
                    // ----- Universal Robots ----- //
                    // title
                    info_title.text = "Universal Robots";
                    // main text
                    info_main_txt.text = "Universal Robots is a Danish manufacturer of smaller flexible industrial collaborative robot arms (cobots), based in Odense, Denmark. The business volume in 2019 was USD 248 million. The company has 680+ employees (2019) and 1,100+ partners around the world.\n\nThe products consist of the heavy-duty UR16e, the table-top UR3 and UR3e, the UR5 and UR5e, and the UR10 and UR10e.\n\nUniversal Robots collaborative robots(cobots) can work right alongside personnel with no safety guarding, based on the results of a mandatory risk assessment.";
                    // Webpage URL
                    URL = "universal-robots.com";
                }
                break;
        }

        // ------------------------ Animation Control ------------------------//
        if (stop_apB == true)
        {
            // ----- Animation is stopped ----- //
            // empty state -> each animation of game objects
            anim_UR3.Play("none");
            anim_abb.Play("none");
            anim_smcT.Play("none");
        }
        else
        {
            // ----- Animation is enabled ----- //
            // enable -> each animation of game objects
            anim_UR3.enabled = true;
            anim_abb.enabled = true;
            anim_smcT.enabled = true;


            if (stage_1_v3dB == true && move_apB == true)
            {
                // start animate -> move UR3
                anim_UR3.Play("move_ur3");
                // empty state -> other objects
                anim_abb.Play("none");
                anim_smcT.Play("none");
            }
            else if(stage_1_v3dB == true && disass_apB == true)
            {
                // start animate -> disassembly UR3
                anim_UR3.Play("disass_ur3");
                // empty state -> other objects
                anim_abb.Play("none");
                anim_smcT.Play("none");
            }

            if (stage_2_v3dB == true && move_apB == true)
            {
                // start animate -> move ABB, 7th axis
                anim_abb.Play("move_abb");
                // empty state -> other objects
                anim_UR3.Play("none");
                anim_smcT.Play("none");
            }
            else if (stage_2_v3dB == true && disass_apB == true)
            {
                // start animate -> disassembly ABB, 7th axis
                anim_abb.Play("disass_abb");
                // empty state -> other objects
                anim_UR3.Play("none");
                anim_smcT.Play("none");
            }

            if (stage_3_v3dB == true && move_apB == true)
            {
                // start animate -> move SMCtrak
                anim_smcT.Play("move_smct");
                // empty state -> other objects
                anim_UR3.Play("none");
                anim_abb.Play("none");
            }
            else if (stage_3_v3dB == true && disass_apB == true)
            {
                // start animate -> disassembly SMCtrak
                anim_smcT.Play("disass_smct");
                // empty state -> other objects
                anim_UR3.Play("none");
                anim_abb.Play("none");
            }
        }
    }

    // ------------------------------------------------------------------------------------------------------------------------//
    // -------------------------------------------------- AUX. FUNCTIONS ------------------------------------------------------//
    // ------------------------------------------------------------------------------------------------------------------------//

    // -------------------- Initialization joints/links {UR3-> (1 - 6)} -------------------- //
    void initialization_joints_UR3()
    {
        // UR3 Base {Declaration}
        var UR3_Children = UR3_Base.GetComponentsInChildren<Transform>();

        // Initialization Joints/Links
        for (int i = 0; i < UR3_Children.Length; i++)
        {
            if (UR3_Children[i].name == "Link1.001")
            {
                joint_UR3[0] = UR3_Children[i].gameObject;
            }
            else if (UR3_Children[i].name == "Link2.001")
            {
                joint_UR3[1] = UR3_Children[i].gameObject;
            }
            else if (UR3_Children[i].name == "Link3.001")
            {
                joint_UR3[2] = UR3_Children[i].gameObject;
            }
            else if (UR3_Children[i].name == "Link4.002")
            {
                joint_UR3[3] = UR3_Children[i].gameObject;
            }
            else if (UR3_Children[i].name == "Link5.001")
            {
                joint_UR3[4] = UR3_Children[i].gameObject;
            }
            else if (UR3_Children[i].name == "Link6.001")
            {
                joint_UR3[5] = UR3_Children[i].gameObject;
            }
        }
    }

    // -------------------- Initialization joints/links {ABB IRB 120 -> (1 - 6)} -------------------- //
    void initialization_joints_ABB_IRB120()
    {
        // ABB IRB 120 Base {Declaration}
        var ABB_IRB120_Children = ABB_IRB120_Base.GetComponentsInChildren<Transform>();

        // Initialization Joints/Links
        for (int i = 0; i < ABB_IRB120_Children.Length; i++)
        {
            if (ABB_IRB120_Children[i].name == "IRB_Link1")
            {
                joint_ABB_IRB120[0] = ABB_IRB120_Children[i].gameObject;
            }
            else if (ABB_IRB120_Children[i].name == "IRB_Link2")
            {
                joint_ABB_IRB120[1] = ABB_IRB120_Children[i].gameObject;
            }
            else if (ABB_IRB120_Children[i].name == "IRB_Link3")
            {
                joint_ABB_IRB120[2] = ABB_IRB120_Children[i].gameObject;
            }
            else if (ABB_IRB120_Children[i].name == "IRB_Link4")
            {
                joint_ABB_IRB120[3] = ABB_IRB120_Children[i].gameObject;
            }
            else if (ABB_IRB120_Children[i].name == "IRB_Link5")
            {
                joint_ABB_IRB120[4] = ABB_IRB120_Children[i].gameObject;
            }
            else if (ABB_IRB120_Children[i].name == "IRB_Link6")
            {
                joint_ABB_IRB120[5] = ABB_IRB120_Children[i].gameObject;
            }
        }
    }

    // -------------------- Initialization joints/links {ABB 7th Axis} -------------------- //
    void initialization_joint_7th_axis()
    {
        // 7th Axis Base {Declaration}
        var axis_7th_children = axis_7th_children_base.GetComponentsInChildren<Transform>();
        // Initialization Joint/Link
        for (int i = 0; i < axis_7th_children.Length; i++)
        {
            if (axis_7th_children[i].name == "ABB_7th_Ax_Link1")
            {
                // 7th Axis
                joint_7th_axis = axis_7th_children[i].gameObject;
            }
        }
    }

    // -------------------- Initialization joints/links {SMCTrak -> SMC PAD, BR PAD} -------------------- //
    void initialization_joints_SMCtrak()
    {
        // SMCTrak Base {Declaration}
        var SMCtrak_children = SMCtrak_base.GetComponentsInChildren<Transform>();
        // Initialization Joints/Links
        for (int i = 0; i < SMCtrak_children.Length; i++)
        {
            if (SMCtrak_children[i].name == "PAD_S")
            {
                // SMC PAD {blue}
                joint_smcT[0] = SMCtrak_children[i].gameObject;
            }
            else if (SMCtrak_children[i].name == "PAD_B")
            {
                // BR PAD {orange}
                joint_smcT[1] = SMCtrak_children[i].gameObject;
            }
        }
    }

    // -------------------- Reset Button -------------------- //
    public void TaskOnClick_ResetBTN()
    {
        // change variable -> True {go to initialization state}
        reset_cpB = true;
        // false -> all stages
        stage_1_v3dB = false;
        stage_2_v3dB = false;
        stage_3_v3dB = false;

        // reset global var. -> actual state
        GlobalVariables_Main.actual_state = 0;

        // reset all panels
        information_panel_image.transform.localPosition = new Vector3(-550f + (ex_param * 100), 15f, 0f);
        animation_panel_image.transform.localPosition = new Vector3(-500f + (ex_param * 100), 50f, 0f);
        control_panel_image.transform.localPosition = new Vector3(-833f + (ex_param * 100), 90f, 0f);

        // reset animation
        stop_apB = true;
    }

    // -------------------- Visible Information {1} Panel -------------------- //
    public void TaskOnClick_InfoBTN()
    {
        // Set information panel {1} -> Initialization panel position to visible -> ON
        information_panel_image.transform.localPosition = new Vector3(-550f, 15f, 0f);
        // Other panels -> visible = OFF
        // animation panel
        animation_panel_image.transform.localPosition = new Vector3(-500f + (ex_param * 100), 50f, 0f);
        // control panel
        control_panel_image.transform.localPosition = new Vector3(-833f + (ex_param * 100), 90f, 0f);

    }
    // -------------------- Exit Information {1} Panel -------------------- //
    public void TaskOnClick_EndInfoBTN()
    {
        // panel visible -> OFF
        information_panel_image.transform.localPosition = new Vector3(-550f + (ex_param * 100), 15f, 0f);
    }
    // -------------------- Visible Animation {2} Panel -------------------- //
    public void TaskOnClick_AnimBTN()
    {
        // Set Animation panel {2} -> Initialization panel position to visible->ON
        animation_panel_image.transform.localPosition = new Vector3(-500f, 50f, 0f);
        // Other panels -> visible = OFF
        // information panel
        information_panel_image.transform.localPosition = new Vector3(-550f + (ex_param * 100), 15f, 0f);
        // object control panel
        control_panel_image.transform.localPosition = new Vector3(-833f + (ex_param * 100), 90f, 0f);

        // animation control (enabled -> true)
        anim_UR3.enabled = true;
        anim_abb.enabled = true;
        anim_smcT.enabled = true;
    }
    // -------------------- Exit Animation {2} Panel -------------------- //
    public void TaskOnClick_EndAnimBTN()
    {
        // panel visible -> OFF
        animation_panel_image.transform.localPosition = new Vector3(-500f + (ex_param * 100), 50f, 0f);
    }

    // -------------------- Visible Object Control {3} Panel -------------------- //
    public void TaskOnClick_ObjControlBTN()
    {
        // Set Object Control panel {2} -> Initialization panel position to visible->ON
        control_panel_image.transform.localPosition = new Vector3(-833f, 90f, 0f);
        // Other panels -> visible = OFF
        // animation panel
        animation_panel_image.transform.localPosition = new Vector3(-500f + (ex_param * 100), 50f, 0f);
        // information panel
        information_panel_image.transform.localPosition = new Vector3(-550f + (ex_param * 100), 15f, 0f);

        // animation control (enabled -> false)
        anim_UR3.enabled = false;
        anim_abb.enabled = false;
        anim_smcT.enabled = false;
    }
    // -------------------- Exit Object Control {3} Panel -------------------- //
    public void TaskOnClick_EndObjControlBTN()
    {
        // panel visible -> OFF
        control_panel_image.transform.localPosition = new Vector3(-833f + (ex_param * 100), 90f, 0f);

        // animation control (enabled -> true)
        anim_UR3.enabled = true;
        anim_abb.enabled = true;
        anim_smcT.enabled = true;
    }

    // -------------------- Main Control Panel -> Stage 1 (UR3) -------------------- //
    public void TaskOnClick_Stage1_v3dBTN()
    {
        // actual page (set) -> true
        stage_1_v3dB = true;
        // reset -> false
        reset_cpB = false;
        // other pages -> false
        stage_2_v3dB = false;
        stage_3_v3dB = false;

        // set actual state -> global var.
        GlobalVariables_Main.actual_state = 1;
    }
    // -------------------- Main Control Panel -> Stage 2 (ABB IRB 120) -------------------- //
    public void TaskOnClick_Stage2_v3dBTN()
    {
        // actual page (set) -> true
        stage_2_v3dB = true;
        // reset -> false
        reset_cpB = false;
        // other pages -> false
        stage_1_v3dB = false;
        stage_3_v3dB = false;

        // set actual state -> global var.
        GlobalVariables_Main.actual_state = 2;
    }
    // -------------------- Main Control Panel -> Stage 3 (SMCTrak) -------------------- //
    public void TaskOnClick_Stage3_v3dBTN()
    {
        // actual page (set) -> true
        stage_3_v3dB = true;
        // reset -> false
        reset_cpB = false;
        // other pages -> false
        stage_1_v3dB = false;
        stage_2_v3dB = false;

        // set actual state -> global var.
        GlobalVariables_Main.actual_state = 3;
    }

    // -------------------- Animation (Disassembly) -------------------- //
    public void TaskOnClick_DisassAnimPBTN()
    {
        // active disassembly var.
        disass_apB = true;
        // inactive -> other var.
        move_apB = false;
        stop_apB = false;
    }
    // -------------------- Animation (Move) -------------------- //
    public void TaskOnClick_MoveAnimPBTN()
    {
        // active move var.
        move_apB = true;
        // inactive -> other var.
        disass_apB = false;
        stop_apB = false;
    }
    // -------------------- Animation (Stop) -------------------- //
    public void TaskOnClick_StopAnimPBTN()
    {
        // active stop animation var.
        stop_apB = true;
        // inactive -> other var.
        disass_apB = false;
        move_apB = false;
    }

    // -------------------- Information Page (Right button) -------------------- //
    public void TaskOnClick_info_eSB_RightBTN()
    {
        counter_infoB = counter_valueF_info + 1;

        if (counter_infoB == max_page_v)
        {
            counter_infoB = min_page_v;
        }

        counter_valueF_info = counter_infoB;
    }
    // -------------------- Information Page (Left button) -------------------- //
    public void TaskOnClick_info_eSB_LeftBTN()
    {
        counter_infoB = counter_infoB - 1;

        if (counter_infoB == ((-1) * max_page_v))
        {
            counter_infoB = min_page_v;
        }

        if (counter_infoB >= min_page_v)
        {
            counter_valueF_info = counter_infoB;
        }
        else
        {
            counter_valueF_info = max_page_v - (-1) * counter_infoB;
        }
    }

    // -------------------- Open WebSite {URL} -------------------- //
    public void TaskOnClick_WebSiteInfoBTN()
    {
        // used to open specific web pages - defined in script
        Application.OpenURL(URL);
    }
}
