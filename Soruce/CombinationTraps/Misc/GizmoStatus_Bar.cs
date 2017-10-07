using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace CombinationTraps
{
    class GizmoStatus_Bar : GizmoStatus
    {
        private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));
        private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
        private const float Padding = 2f;
        private const float ButtonSize = 20f;
        
        public SignalVerbBar verb;
        private static float sync;

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            this.verb.Value = GizmoStatus_Bar.sync;
            DebugLog.LogDebugPoint();
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft)
        {
            bool flag = false;
            Rect overRect = new Rect(topLeft.x, topLeft.y, this.Width, 75f);
            
            Widgets.DrawWindowBackground(overRect);

            Rect baseRect = overRect.ContractedBy(6f);
            float paragraph = baseRect.height / 3f;
            Rect labelRect = new Rect(baseRect.x, baseRect.y, baseRect.width, paragraph);
            Text.Font = GameFont.Tiny;
            Widgets.Label(labelRect, base.defaultLabel);
                
            float topLeft2 = labelRect.yMax;
            float paragraph2 = paragraph * 2 - Padding;
            Rect buttonRect1 = new Rect(baseRect.x, topLeft2, ButtonSize, paragraph2);
            if(Widgets.ButtonImage(buttonRect1, CT_TexCommandOf.ParameterCommand_Sub))
            {
                this.verb.Sub();
                flag = true;
            }
            Rect buttonRect2 = new Rect(baseRect.xMax - ButtonSize, topLeft2, ButtonSize, paragraph2);
            if (Widgets.ButtonImage(buttonRect2, CT_TexCommandOf.ParameterCommand_Add))
            {
                this.verb.Add();
                flag = true;
            }

            Rect sliderRect = new Rect(buttonRect1.xMax + Padding, topLeft2, buttonRect2.xMin - buttonRect1.xMax - Padding * 2, paragraph2 / 2);
            float num = Widgets.HorizontalSlider(sliderRect, this.verb.Value, this.verb.Min, this.verb.Max, true, null, null, null, this.verb.Rate);
            if(num != this.verb.Value)
            {
                this.verb.Value = num;
                flag = true;
            }

            Rect barRect = new Rect(sliderRect.x, sliderRect.yMax, sliderRect.width, sliderRect.height);
            float fillPercent = this.verb.Value / Mathf.Max(1f, this.verb.Max);
            Widgets.FillableBar(barRect, fillPercent, GizmoStatus_Bar.FullShieldBarTex, GizmoStatus_Bar.EmptyShieldBarTex, false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(barRect, this.verb.LabelValue + " / " + this.verb.LabelMax);
            Text.Anchor = TextAnchor.UpperLeft;

            if (flag)
            {
                if (this.disabled)
                {
                    if (!this.disabledReason.NullOrEmpty())
                    {
                        Messages.Message(this.disabledReason, MessageSound.RejectInput);
                    }
                    return new GizmoResult(GizmoState.Mouseover, null);
                }
                if (!TutorSystem.AllowAction(this.TutorTagSelect))
                {
                    return new GizmoResult(GizmoState.Mouseover, null);
                }
                GizmoStatus_Bar.sync = this.verb.Value;
                GizmoResult result = new GizmoResult(GizmoState.Interacted, Event.current);
                TutorSystem.Notify_Event(this.TutorTagSelect);
                return result;
            }

            return new GizmoResult(GizmoState.Clear);
        }

        public override bool GroupsWith(Gizmo other)
        {
            return other is GizmoStatus_Bar;
        }
    }
}
