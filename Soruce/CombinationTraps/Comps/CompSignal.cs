using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace CombinationTraps
{
    public abstract class CompSignal : ThingComp
    {
        protected CompProperties_Signal compDef => (CompProperties_Signal)base.props;
        protected virtual string TransmitingSignal => "CT_TransmitingSignal";

        private TransmissionStance currentTransmissionStance = TransmissionStance.Undefined;
        private int lastReceivedTick;
        private List<SignalVerb> verbs = new List<SignalVerb>();

        private int curVerbIndex = 0;
        protected SignalVerb CurVerb
        {
            get { return verbs.ElementAtOrDefault(curVerbIndex); }
        }

        public abstract IEnumerable<SignalVerb> MakeVerbs();

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!compDef.transmissionStanceFilter.Contains(compDef.defaultTransmissionStance))
            {
                this.currentTransmissionStance = compDef.defaultTransmissionStance;
            } else
            {
                Log.Warning(DebugLog.Sign() + "Value of <defaultTransmissionStance> has been filtered.");
                this.SwitchStance();
            }
            foreach(var verb in this.MakeVerbs())
            {
                this.verbs.Add(verb);
            }
        }

        public override void CompTick()
        {
            if (this.CurVerb != default(SignalVerb) && this.CurVerb.ShouldRun)
            {
                this.CurVerb.Run();
            }
        }

        public void TrySpreadSignal(CompPower trriger)
        {
            int num = Find.TickManager.TicksGame;
            if (this.lastReceivedTick == num || !this.CanTransmitSignal() || trriger == null || trriger.PowerNet == null)
            {
                return;
            }
            this.lastReceivedTick = num;
            List<CompPower> list = trriger.PowerNet.connectors;
            for(int i = 0; i < list.Count; i++)
            {
                list[i].parent.BroadcastCompSignal(this.TransmitingSignal);
            }
        }
        public override void ReceiveCompSignal(string signal)
        {
            if(this.CheckSignal(signal) && this.CanReceiveSignal())
            {
                this.Transmit();
            }
        }
        public void Transmit()
        {
            this.CurVerb?.Triggered();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            SignalVerb verb = this.CurVerb;
            if(verb != default(SignalVerb))
            {
                yield return new Command_Action
                {
                    defaultLabel = verb.LabelCap,
                    defaultDesc = verb.Desc,
                    icon = verb.Texture,
                    action = this.SwitchVerb
                };
                foreach (var gizmo in CurVerb.SignalVerbGizmos())
                {
                        yield return gizmo;
                }
            }
            yield return new Command_Action
            {
                defaultLabel = this.currentTransmissionStance.Label(),
                defaultDesc = this.currentTransmissionStance.Desc(),
                icon = this.currentTransmissionStance.Texture(),
                action = this.SwitchStance
            };

            if (DebugSettings.godMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Launch",
                    defaultDesc = "Debug mode",
                    icon = BaseContent.BadTex,
                    action = this.Transmit
                };
            }
        }
        protected virtual bool CheckSignal(string signal)
        {
            return signal == this.TransmitingSignal;
        }
        public void SwitchStance()
        {
            int num = (int)this.currentTransmissionStance;
            int count = Enum.GetNames(typeof(TransmissionStance)).Length;
            for(int i = 0; i < count; i++)
            {
                num = (num + 1) % count;
                if(!this.compDef.transmissionStanceFilter.Contains((TransmissionStance)num) && num != 0)
                {
                    this.currentTransmissionStance = (TransmissionStance)num;
                    return;
                }
            }
            this.currentTransmissionStance = TransmissionStance.Undefined;
        }
        public void SwitchVerb()
        {
            SignalVerb preVerb = this.CurVerb;
            int num = curVerbIndex + 1;
            this.curVerbIndex = num < this.verbs.Count ? num: 0;
            preVerb.Notify_Changed(this.CurVerb);
            this.CurVerb.Reset();
        }

        protected bool CanReceiveSignal()
        {
            TransmissionStance tm = this.currentTransmissionStance;
            return tm == TransmissionStance.Both || tm == TransmissionStance.OnlyReceive;
        }
        protected bool CanTransmitSignal()
        {
            TransmissionStance tm = this.currentTransmissionStance;
            return tm == TransmissionStance.Both || tm == TransmissionStance.OnlyTransmit;
        }
    }
}
