using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using Rage;
using System.Drawing;
using Rage.Native;

namespace Mytical_Callout.Callouts
{
    [CalloutInfo("SimplePursuitMytical", CalloutProbability.High)]
    class SimplePursuit: Callout
    {
        private Blip blip;
        private Vector3 spawnPoint;
        private Ped suspect;
        // private Ped suspect2;
        private Vehicle suspectVehicle;
        private LHandle pursuit;
        
        private bool pursuitCreated;
        
        public override bool OnBeforeCalloutDisplayed()
        {
            Game.DisplayNotification("OnBeforeCalloutDisplayed");
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(150f));
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            

            CalloutMessage = "THIIIIIIIIEEEEEEPoursuit!";
            CalloutPosition = spawnPoint;
            CalloutAdvisory = "pursuit is in progress!";
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_RESIST_ARREST_04 IN_OR_ON_POSITION", spawnPoint);


            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.DisplayNotification("OnCalloutAccepted");
            suspectVehicle = new Vehicle("POLICE2", spawnPoint);
            suspectVehicle.IsPersistent = true;
            

           suspect = suspectVehicle.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            //suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", -1, true);
            suspect.WarpIntoVehicle(suspectVehicle, -1);
            /*
            suspect2 = new Ped(suspectVehicle.GetOffsetPositionFront(5f));
            suspect2.IsPersistent = true;
            suspect2.BlockPermanentEvents = true;
            suspect2.Inventory.GiveNewWeapon("WEAPON_ASSAULTRIFLE", -1, true);
            suspect2.WarpIntoVehicle(suspectVehicle, 2);
            */

            blip = suspect.AttachBlip();
            blip.Color = Color.Red;
            blip.IsRouteEnabled = true;

            



            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            base.Process();

            Game.DisplayNotification("Process");
            if (!pursuitCreated && Game.LocalPlayer.Character.DistanceTo(suspectVehicle) <= 40f)
            {
                blip.Delete();
                pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(pursuit, suspect);
                Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                pursuitCreated = true;

            }
           /* if (Game.LocalPlayer.Character.DistanceTo(suspectVehicle) <= 5f)
            {
                NativeFunction.Natives.TASK_OPEN_VEHICLE_DOOR(suspect2, suspectVehicle, 18000, 2, 1.47f);
                Game.DisplayNotification("SHOT FIRED !");
                NativeFunction.Natives.SetPedCombatAttributes(suspect, 1, true);
                suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                Functions.PlayScannerAudio("WE_HAVE CRIME_SHOTS_FIRED_01");
            }

            if (Game.LocalPlayer.GetFreeAimingTarget() || Game.LocalPlayer.Character.IsDoingDriveby == suspect || suspectVehicle)
            {
                NativeFunction.Natives.TASK_OPEN_VEHICLE_DOOR(suspect2, suspectVehicle, 18000, 2, 1.47f);
                suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", -1, true);
                Game.DisplayNotification("SHOT FIRED !");
                NativeFunction.Natives.SetPedCombatAttributes(suspect, 1, true);
                suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                Functions.PlayScannerAudio("WE_HAVE CRIME_SHOTS_FIRED_01");
            }
            if (!suspect.IsOnFoot)
            {
                Game.DisplayNotification("IS ON FOOT !");
                
                suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", -1, true);
                suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, 20000, FiringPattern.BurstFirePistol);
            }
            if (suspect.IsOnFoot)
            {
                Game.DisplayNotification("IS ON FOOT");
                
                suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", -1, true);
                suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, 20000,FiringPattern.BurstFirePistol);
            }
            if (Game.LocalPlayer.Character.IsShooting)
            {
                Game.DisplayNotification("IS SHOOTING");
                suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", -1, true);
                NativeFunction.Natives.TASK_OPEN_VEHICLE_DOOR(suspect2,suspectVehicle, 18000, 2, 1.47f);
                suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                
                suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, 20000, FiringPattern.BurstFirePistol);
            }*/

            if (pursuitCreated == true && !Functions.IsPursuitStillRunning(pursuit))
            {
                End();
            }
            if (!suspectVehicle.Exists() || !suspect.Exists())
            {
                End();
               
            }

            base.Process();
        }
    
        public override void End()
        {
            Game.DisplayNotification("End");
            base.End();
            if (suspect.Exists())
            {
                suspect.Dismiss();
            }
            if (blip.Exists())
            {
                blip.Delete();
            }
            if (suspectVehicle.Exists())
            {
                suspectVehicle.Delete();
            }
            Game.LogTrivial("Mytical Callout cleaned up.");
        }


    }
}
