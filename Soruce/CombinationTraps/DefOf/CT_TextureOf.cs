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
        public static readonly Texture2D TransStance_Any = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_Any", true);
        public static readonly Texture2D TransStance_OnlyReceive = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_OnlyReceive", true);
        public static readonly Texture2D TransStance_OnlyTransmit = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_OnlyTransmit", true);
        public static readonly Texture2D TransStance_ShutDown = ContentFinder<Texture2D>.Get("UI/Commands/TransStance_ShutDown", true);

        public static readonly Texture2D ParameterCommand_Add = ContentFinder<Texture2D>.Get("UI/Commands/ParameterCommand_Add", true);
        public static readonly Texture2D ParameterCommand_Sub = ContentFinder<Texture2D>.Get("UI/Commands/ParameterCommand_Sub", true);
        public static readonly Texture2D ParameterCommand_Reset = ContentFinder<Texture2D>.Get("UI/Commands/ParameterCommand_Reset", true);

        public static readonly Texture2D SignalBehavior_Delay = ContentFinder<Texture2D>.Get("UI/Commands/SignalBehavior_Delay", true);
        public static readonly Texture2D SignalBehavior_LastDelay = ContentFinder<Texture2D>.Get("UI/Commands/SignalBehavior_LastDelay", true);
    }
}
