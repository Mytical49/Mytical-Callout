using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using LSPD_First_Response.Mod.API;
using Mytical_Callout.Callouts;
using Rage;


namespace Mytical_Callout
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Game.LogTrivial("Mytical Callout version 1.0.0 by Mytical has been initialised !");
            Game.DisplayNotification("Mytical Callout version 1.0.0 by Mytical has been initialised !");
        }
        public override void Finally()
        {
            Game.LogTrivial("Mytical Callout version 1.0.0 by Mytical has been cleaned up !");

        }
        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.SimplePursuit));
          
        }
        private static void OnDutyChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                RegisterCallouts();
                Game.DisplayNotification("Mytical Callout version 1.0.0 by Mytical has been loaded !");
            }
        }
        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return Functions.GetAllUserPlugins().FirstOrDefault(assembly => args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()));
        }
        public static bool IsLSPDFRPLuginRunning(string Plugin, Version minversion = null)
        {
            
            return Functions.GetAllUserPlugins() 
            .Select(assembly => assembly.GetName())
            .Where(an => an.Name.ToLower() == Plugin.ToLower())
            .Any(an => minversion == null || an.Version.CompareTo(minversion) >= 0); 
        }
        
    }
}
