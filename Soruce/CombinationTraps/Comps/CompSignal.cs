using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace CombinationTraps
{
    public class CompSignal : ThingComp
    {

        protected CompProperties_Signal compDef => (CompProperties_Signal)base.props;
        protected TransmissionStance currentTransmissionStance = TransmissionStance.Undefined;
        protected List<SignalBehavior> behaviors = new List<SignalBehavior>();
        protected virtual string TransmitingSignal => "CT_TransmitingSignal";

        private int lastReceivedTick;

        private int curBehaviorIndex = 0;
        protected SignalBehavior CurBehavior
        {
            get { return behaviors.ElementAtOrDefault(curBehaviorIndex); }
        }

        public Action SignalCallBack;

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
        }

        public void TrySpreadSignal(CompPower trriger)
        {
            int num = Find.TickManager.TicksGame;
            if (this.lastReceivedTick == num || trriger == null || !this.CanTransmitSignal())
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
        public virtual void Transmit()
        {
            SignalCallBack?.Invoke();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Action
            {
                defaultLabel = this.currentTransmissionStance.Label(),
                defaultDesc = this.currentTransmissionStance.Desc(),
                icon = this.currentTransmissionStance.Texture(),
                action = this.SwitchStance
            };
            SignalBehavior behavior = this.CurBehavior;
            if(behavior != default(SignalBehavior))
            {
                yield return new Command_Action
                {
                    defaultLabel = behavior.LabelCap,
                    defaultDesc = behavior.Desc,
                    icon = behavior.Texture,
                    action = this.SwitchBehavior
                };

                foreach(var a in BehaviorCommandsUtility.GetParameterCommand(CurBehavior))
                {
                    yield return a;
                }
            }
            if (DebugSettings.godMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Call Transmit",
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
        public void SwitchBehavior()
        {
            int num = curBehaviorIndex + 1;
            this.curBehaviorIndex = num < this.behaviors.Count ? num: 0;
        }

        private bool CanReceiveSignal()
        {
            TransmissionStance tm = this.currentTransmissionStance;
            return tm == TransmissionStance.Any || tm == TransmissionStance.OnlyReceive;
        }
        private bool CanTransmitSignal()
        {
            TransmissionStance tm = this.currentTransmissionStance;
            return tm == TransmissionStance.Any || tm == TransmissionStance.OnlyTransmit;
        }
    }
}
