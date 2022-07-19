using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exelent_awall
{
    public static class awall
    {
        public static void awall_shoot()
        {
            cl_Memory.met_WriteMemory<int>(cl_Memory.var_ClientAddress + klas.dwForceAttack, 6); //static shoot
        }

        public static int hitselect; //for hitbox selectors
        public static int hitted;    //firt healt
        public static int hitted1;   //hitted health

        public static void awall_spinmove() // currently not working
        {
            cl_Memory.met_WriteMemory<float>(ProgramUnsafe.var_ClientState + cl_Offsets.var_offset_ViewAngles, X2);
            cl_Memory.met_WriteMemory<float>(ProgramUnsafe.var_ClientState + cl_Offsets.var_offset_ViewAngles + 4, Y2);
        }

        public static void awall_move() // lock enemy
        {
            cl_Memory.met_WriteMemory<float>(ProgramUnsafe.var_ClientState + cl_Offsets.var_offset_ViewAngles, X);
            cl_Memory.met_WriteMemory<float>(ProgramUnsafe.var_ClientState + cl_Offsets.var_offset_ViewAngles + 4, Y);
        }

        public static float X = 0;
        public static float Y = 0;
        public static float X2 = 0;
        public static float Y2 = 0;

        public static bool awall_scale_for_helmet()
        {
            // other scale method (fast but not cool)

            int hitgroup = 0;


            float armor = ProgramUnsafe.var_entityArmor;

            if (armor > 0f && hitgroup < hitgroup_Lowchest)
            {
                if (hitgroup == hitgroup_Head && !ProgramUnsafe.var_entityHelmet)

                    return true;
            }
            return false;
        }

        public static float resolve_hit()
        {
            // Resolver math helper

            if (hitselect == hitgroup_Head)
            {
                return 4.0f;
            }
            else if (hitselect == hitgroup_Upchest)
            {
                return 2.5f;
            }
            else if (hitselect == hitgroup_Lowchest)
            {
                return 0.6f;
            }
            else
            {
                return 1f;
            }
        }

        public static int hitgroup_Head = 8;
        public static int hitgroup_Upchest = 6;
        public static int hitgroup_Lowchest = 4;


        public static int awall_hitcalculator()
        {
            // if enemy has not helmet, hitbox will select head.
            // if enemy has helmet but no armor, hitbox will select body.
            // if enemy has armor & helmet, hitbox will calculate with health. 


            int calculate = 8; //default head position
            int var_DetectedPlayerV_Team = cl_Memory.met_ReadMemory<int>(var_entity + klas.m_iTeamNum);
            if (var_DetectedPlayerV_Team != ProgramUnsafe.var_LocalPlayer_Team)
            {
                int var_entityHealth1 = cl_Memory.met_ReadMemory<int>(var_entity + klas.m_iHealth);
                if (var_entityHealth1 != 0)
                {
                    bool spotted = cl_Memory.met_ReadMemory<bool>(var_entity + klas.m_bSpottedByMask);

                    if (spotted == true) //if enemy on the screen (doesnt matter if its behind the wall)
                    {
                        bool hellmetted = var_entityHelmet;
                        if (hellmetted == true)
                        {
                            if (var_entityArmor != 0)
                            {
                                if (var_entityHealth1 > 70)
                                {
                                    calculate = awall.hitgroup_Head;
                                }
                                else
                                {
                                    if (var_entityHealth1 < 25)
                                    {
                                        calculate = awall.hitgroup_Lowchest;
                                    }
                                    else
                                    {
                                        calculate = awall.hitgroup_Upchest;
                                    }
                                }
                            }
                            else
                            {
                                calculate = awall.hitgroup_Upchest;
                            }
                        }
                        else
                        {
                            calculate = awall.hitgroup_Head;
                        }
                    }
                }
            }
            return calculate;
        }
    }
}
