using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    public enum TransmissionStance : int
    {
        Undefined,
        ShutOut,
        OnlyReceive,
        OnlyTransmit,
        Both
    }

    public class CompProperties_Signal : CompProperties
    {
        public TransmissionStance defaultTransmissionStance = TransmissionStance.Undefined;
        public List<TransmissionStance> transmissionStanceFilter = new List<TransmissionStance>();
    }

    static class TransmissionStance_Expansion
    {
        public static string Label(this TransmissionStance value)
        {
            switch (value)
            {
                case TransmissionStance.Both:
                    return "StanceBoth".Translate();
                case TransmissionStance.ShutOut:
                    return "StanceShutOut".Translate();
                case TransmissionStance.OnlyReceive:
                    return "StanceReceive".Translate();
                case TransmissionStance.OnlyTransmit:
                    return "StanceTransmit".Translate();
                default:
                    return "StanceUndefined";
            }
        }
        public static string Desc(this TransmissionStance value)
        {
            switch (value)
            {
                case TransmissionStance.Both:
                    return "StanceBothDesc".Translate();
                case TransmissionStance.ShutOut:
                    return "StanceShutOutDesc".Translate();
                case TransmissionStance.OnlyReceive:
                    return "StanceReceiveDesc".Translate();
                case TransmissionStance.OnlyTransmit:
                    return "StanceTransmitDesc".Translate();
                default:
                    return "StanceUndefined";
            }
        }
        public static Texture2D Texture(this TransmissionStance value)
        {
            switch (value)
            {
                case TransmissionStance.Both:
                    return CT_TexCommandOf.TransStance_Both;
                case TransmissionStance.ShutOut:
                    return CT_TexCommandOf.TransStance_ShutOut;
                case TransmissionStance.OnlyReceive:
                    return CT_TexCommandOf.TransStance_Receive;
                case TransmissionStance.OnlyTransmit:
                    return CT_TexCommandOf.TransStance_Transmit;
                default:
                    return BaseContent.BadTex;
            }
        }
    }
    
}
