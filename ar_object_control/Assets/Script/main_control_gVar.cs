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
File Name: main_control_gVar.cs

****************************************************************************/

// ------------------------------------------------------------------------------------------------------------------------//
// ----------------------------------------------------- LIBRARIES --------------------------------------------------------//
// ------------------------------------------------------------------------------------------------------------------------//

// -------------------- Unity -------------------- //
using UnityEngine;
using UnityEngine.UI;

// ------------------------------------------------------------------//
// -------------------- Class {Global Variable} -------------------- //
// ------------------------------------------------------------------//

// -------------------- Universal Robots UR3 (Stage 1) -------------------- //
public static class GlobalVariables_stage_1_UR3
{
    // joint limit
    public static float[] joint_max = new float[6];
    public static float[] joint_min = new float[6];
    // joint actual position
    public static float[] actual_jPos = { 0f, 0f, 0f, 0f, 0f, 0f};
    // calculation length
    public static int max_joints = joint_max.Length;
}

// -------------------- ABB IRB 120 with 7th axis (Stage 2) -------------------- //
public static class GlobalVariables_stage_2_ABB
{
    // joint limit
    public static float[] joint_max = new float[7];
    public static float[] joint_min = new float[7];
    // joint actual position
    public static float[] actual_jPos = { 0f, 0f, 0f, 0f, 0f, 0f, 0f};
    // calculation length
    public static int max_joints = joint_max.Length;
}

// -------------------- SMCTrak {SMC, BR Pad} (Stage 3) -------------------- //
public static class GlobalVariables_stage_3_SMCt
{
    // joint limit
    public static float[] joint_max = new float[2];
    public static float[] joint_min = new float[2];
    // joint actual position
    public static float[] actual_jPos = { 0f, 0f};
    // calculation length
    public static int max_joints = joint_max.Length;
}

// -------------------- Main Global Var. -------------------- //
public static class GlobalVariables_Main
{
    // actual state (main switch)
    public static int actual_state;
    // max. number of joints
    public static int max_joints = GlobalVariables_stage_2_ABB.max_joints;
}

// -------------------- Class {Unity - Control Program} -------------------- //
public class main_control_gVar : MonoBehaviour
{
    // ************************************************ //
    // -------------------- PUBLIC -------------------- //
    // ************************************************ //

    // -------------------- Slider -------------------- //
    public Slider[] s_param_cObjP = new Slider[7];

    // ************************************************ //
    // -------------------- PRIVATE ------------------- //
    // ************************************************ //

    // -------------------- Bool -------------------- //
    private bool reset_p_s1, reset_p_s2, reset_p_s3;

    // ------------------------------------------------------------------------------------------------------------------------//
    // ------------------------------------------------ INITIALIZATION {START} ------------------------------------------------//
    // ------------------------------------------------------------------------------------------------------------------------//
    void Start()
    {
        // UR3 -> Joint Limit (1 - 6) : Min
        GlobalVariables_stage_1_UR3.joint_min[0] = (-1) * 36000;
        GlobalVariables_stage_1_UR3.joint_min[1] = (-1) * 36000;
        GlobalVariables_stage_1_UR3.joint_min[2] = (-1) * 36000;
        GlobalVariables_stage_1_UR3.joint_min[3] = (-1) * 36000;
        GlobalVariables_stage_1_UR3.joint_min[4] = (-1) * 36000;
        GlobalVariables_stage_1_UR3.joint_min[5] = (-1) * 36000;
        // UR3 -> Joint Limit (1 - 6) : Max
        GlobalVariables_stage_1_UR3.joint_max[0] = 36000;
        GlobalVariables_stage_1_UR3.joint_max[1] = 36000;
        GlobalVariables_stage_1_UR3.joint_max[2] = 36000;
        GlobalVariables_stage_1_UR3.joint_max[3] = 36000;
        GlobalVariables_stage_1_UR3.joint_max[4] = 36000;
        GlobalVariables_stage_1_UR3.joint_max[5] = 36000;
        // ABB IRB 120 -> Joint Limit (1 - 6), 7th Axis (1) : Min
        GlobalVariables_stage_2_ABB.joint_min[0] = (-1) * 16500;
        GlobalVariables_stage_2_ABB.joint_min[1] = (-1) * 11000;
        GlobalVariables_stage_2_ABB.joint_min[2] = (-1) * 11000;
        GlobalVariables_stage_2_ABB.joint_min[3] = (-1) * 16000;
        GlobalVariables_stage_2_ABB.joint_min[4] = (-1) * 12000;
        GlobalVariables_stage_2_ABB.joint_min[5] = (-1) * 40000;
        GlobalVariables_stage_2_ABB.joint_min[6] = 0;
        // ABB IRB 120 -> Joint Limit (1 - 6), 7th Axis (1) : Max
        GlobalVariables_stage_2_ABB.joint_max[0] = 16500;
        GlobalVariables_stage_2_ABB.joint_max[1] = 11000;
        GlobalVariables_stage_2_ABB.joint_max[2] = 7000;
        GlobalVariables_stage_2_ABB.joint_max[3] = 16000;
        GlobalVariables_stage_2_ABB.joint_max[4] = 12000;
        GlobalVariables_stage_2_ABB.joint_max[5] = 40000;
        GlobalVariables_stage_2_ABB.joint_max[6] = 80000;
        // SMCTrak -> Joint Limit (1 - 2) : Min
        GlobalVariables_stage_3_SMCt.joint_min[0] = 0;
        GlobalVariables_stage_3_SMCt.joint_min[1] = 0;
        // SMCTrak -> Joint Limit (1 - 2) : Max
        GlobalVariables_stage_3_SMCt.joint_max[0] = 140000;
        GlobalVariables_stage_3_SMCt.joint_max[1] = 140000;
        // Read actual stat from ar_object_control script
        GlobalVariables_Main.actual_state = 0;

        // reset variables (initialization)
        reset_p_s1 = reset_p_s2 = reset_p_s3 = false;
    }

    // ------------------------------------------------------------------------------------------------------------------------//
    // ------------------------------------------------ MAIN FUNCTION {Cyclic} ------------------------------------------------//
    // ------------------------------------------------------------------------------------------------------------------------//
    private void Update()
    {
        // ------------------------------------------------ Actual State (Slider Parameters Control) ------------------------------------------------//
        switch (GlobalVariables_Main.actual_state)
        {
            case 0:
                {
                    // -------------------- RESET STATE -------------------- //

                    for (int i = 0; i < GlobalVariables_Main.max_joints; i++)
                    {
                        if (i < GlobalVariables_stage_1_UR3.max_joints)
                        {
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_1_UR3.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_1_UR3.joint_min[i];
                            s_param_cObjP[i].value = 0;
                            s_param_cObjP[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_2_ABB.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_2_ABB.joint_min[i];
                            s_param_cObjP[i].value = 0;
                            s_param_cObjP[i].gameObject.SetActive(false);
                        }
                    }

                    // reset variables (initialization)
                    reset_p_s1 = reset_p_s2 = reset_p_s3 = false;

                    // Change actual state -> Stage 1 (initial state)
                    GlobalVariables_Main.actual_state = 1;
                }
                break;
            case 1:
                {
                    // -------------------- Stage 1 STATE (UR3 robot) -------------------- //

                    if (reset_p_s1 == false)
                    {
                        // reset stage 2, 3
                        reset_p_s2 = false;
                        reset_p_s3 = false;

                        // initialization of the actual stage (ur3 robot)
                        for (int i = 0; i < GlobalVariables_stage_1_UR3.max_joints; i++)
                        {
                            // set min, max limit
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_1_UR3.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_1_UR3.joint_min[i];
                            // initial parameter -> Null
                            s_param_cObjP[i].value = 0;
                            // set active sliders/parameters for actual stage
                            s_param_cObjP[i].gameObject.SetActive(true);
                        }

                        // reset -> done
                        reset_p_s1 = true;
                    }

                    // move joints -> Universal Robot Ur3
                    for (int i = 0; i < GlobalVariables_Main.max_joints; i++)
                    {
                        if (i < GlobalVariables_stage_1_UR3.max_joints)
                        {
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_1_UR3.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_1_UR3.joint_min[i];
                            // set parameters for UR3 from each slider
                            GlobalVariables_stage_1_UR3.actual_jPos[i] = s_param_cObjP[i].value;
                            // set active sliders/parameters for actual stage
                            s_param_cObjP[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            // set inactive sliders/parameters for other stages
                            s_param_cObjP[i].value = 0;
                            s_param_cObjP[i].gameObject.SetActive(false);
                        }
                    }
                }
                break;
            case 2:
                {
                    // -------------------- Stage 2 STATE (ABB IRB 120 ROBOT, 7th Axis) -------------------- //
                    if (reset_p_s2 == false)
                    {
                        // reset stage 1, 3
                        reset_p_s1 = false;
                        reset_p_s3 = false;

                        // initialization of the actual stage (abb robot and 7th axis)
                        for (int i = 0; i < GlobalVariables_stage_2_ABB.max_joints; i++)
                        {
                            // set min, max limit
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_2_ABB.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_2_ABB.joint_min[i];
                            // initial parameter -> Null
                            s_param_cObjP[i].value = 0;
                            // set active sliders/parameters for actual stage
                            s_param_cObjP[i].gameObject.SetActive(true);
                        }

                        // reset -> done
                        reset_p_s2 = true;
                    }

                    // move joints -> ABB IRB 120 and 7th Axis
                    for (int i = 0; i < GlobalVariables_Main.max_joints; i++)
                    {
                        if (i < GlobalVariables_stage_2_ABB.max_joints)
                        {
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_2_ABB.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_2_ABB.joint_min[i];
                            // set parameters for ABB, 7th axis from each slider
                            GlobalVariables_stage_2_ABB.actual_jPos[i] = s_param_cObjP[i].value;
                            // set active sliders/parameters for actual stage
                            s_param_cObjP[i].gameObject.SetActive(true);
                        }
                    }
                }
                break;
            case 3:
                {
                    // -------------------- Stage 3 STATE (SMCTrak {SMC, BR PAD}) -------------------- //
                    if (reset_p_s3 == false)
                    {
                        // reset stage 1, 2
                        reset_p_s1 = false;
                        reset_p_s2 = false;

                        // initialization of the actual stage (smctrak)
                        for (int i = 0; i < GlobalVariables_stage_3_SMCt.max_joints; i++)
                        {
                            // set min, max limit
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_3_SMCt.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_3_SMCt.joint_min[i];
                            // initial parameter -> Null
                            s_param_cObjP[i].value = 0;
                            // set active sliders/parameters for actual stage
                            s_param_cObjP[i].gameObject.SetActive(true);
                        }

                        // reset -> done
                        reset_p_s3 = true;
                    }

                    // move joints -> SMCTrak {SMC, BR PAD}
                    for (int i = 0; i < GlobalVariables_Main.max_joints; i++)
                    {
                        if (i < GlobalVariables_stage_3_SMCt.max_joints)
                        {
                            s_param_cObjP[i].maxValue = GlobalVariables_stage_3_SMCt.joint_max[i];
                            s_param_cObjP[i].minValue = GlobalVariables_stage_3_SMCt.joint_min[i];
                            // set parameters for SMCTrak from each slider
                            GlobalVariables_stage_3_SMCt.actual_jPos[i] = s_param_cObjP[i].value;
                            // set active sliders/parameters for actual stage
                            s_param_cObjP[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            // set inactive sliders/parameters for other stages
                            s_param_cObjP[i].value = 0;
                            s_param_cObjP[i].gameObject.SetActive(false);
                        }
                    }
                }
                break;
        }
    }
}