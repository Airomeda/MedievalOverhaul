﻿using ProcessorFramework;
using RimWorld;
using Verse;

namespace MedievalOverhaul
{
    public class CompProperties_GlowerOnlyWhenUsed : CompProperties_Glower
    {
        public CompProperties_GlowerOnlyWhenUsed()
        {
            this.compClass = typeof(CompGlowerOnlyWhenUsed);
        }
    }

    public class CompGlowerOnlyWhenUsed : CompGlower
	{
		public CompProcessor comp;
		public bool IsWorkedOn
		{
			get
			{
				if (parent.Spawned)
				{
                    if (parent is Building_WorkTable workTable)
                    {
                        foreach (var reservation in this.parent.Map.reservationManager.ReservationsReadOnly)
                        {
                            if (reservation.Target.Thing == this.parent)
                            {
                                var pawn = reservation.Claimant;
                                if (!pawn.pather.MovingNow && pawn.Position == workTable.InteractionCell)
                                {
                                    return true;
                                }
                            }
                        }

                    }
                    if (comp != null && comp.activeProcesses.Any() && comp.activeProcesses.Any(x => x.Complete is false))
					{
                        return true;
                    }
                }
				return false;
			}
		}

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				comp = parent.GetComp<CompProcessor>();
			}
        }

        public override bool ShouldBeLitNow
		{
			get
			{
				var baseValue = base.ShouldBeLitNow;
				if (baseValue)
				{
					if (IsWorkedOn is false)
					{
                        baseValue = false;
                    }
                }
				return baseValue;
            }
		}
    }
}
