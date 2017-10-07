using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace CombinationTraps
{
    [StaticConstructorOnStartup]
    public static class CT_TexCommandOf
    {
        public static readonly Texture2D TransStance_Both = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_Both", true);
        public static readonly Texture2D TransStance_Receive = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_Receive", true);
        public static readonly Texture2D TransStance_Transmit = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_Transmit", true);
        public static readonly Texture2D TransStance_ShutOut = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_ShutOut", true);

        public static readonly Texture2D ParameterCommand_Add = ContentFinder<Texture2D>.Get("UI/Commands/ParameterCommand_Add", true);
        public static readonly Texture2D ParameterCommand_Sub = ContentFinder<Texture2D>.Get("UI/Commands/ParameterCommand_Sub", true);

        public static readonly Texture2D SignalVerb_DelayedLaunch = ContentFinder<Texture2D>.Get("UI/Commands/SignalVerb_DelayedLaunch", true);
        public static readonly Texture2D SignalVerb_DelayedLaunchUpdated = ContentFinder<Texture2D>.Get("UI/Commands/SignalVerb_DelayedLaunchUpdated", true);
        public static readonly Texture2D SignalVerb_Suppression = ContentFinder<Texture2D>.Get("UI/Commands/SignalVerb_Suppression", true);
        public static readonly Texture2D SignalVerb_Reverse = ContentFinder<Texture2D>.Get("UI/Commands/SignalVerb_Reverse", true);
    }
}
