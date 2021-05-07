using Assets.Src.Evolution;
using Assets.Src.ObjectManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Src.Targeting.TargetPickers
{
    public class HasTagTargetPicker : GeneticallyConfigurableTargetPicker
    {
      
        public string Tag;
        public bool KullInvalidTargets = false;


        public override bool AllowNegative { get { return true; } }
        
       
        public bool TargetsWithTagAreValid = false;

        public override IEnumerable<PotentialTarget> FilterTargets(IEnumerable<PotentialTarget> potentialTargets)
        {
            if(FlatBoost != 0 && !string.IsNullOrEmpty(Tag))
            {
                return potentialTargets.Select(t => {
                    if(t.Transform.IsValid() && t.Transform.tag == Tag)
                    {
                        
                        t.IsValidForCurrentPicker = TargetsWithTagAreValid;
                        t.Score += FlatBoost;
                    } else
                    {
                        
                        t.IsValidForCurrentPicker = !TargetsWithTagAreValid;
                    }
                    return t;
                });
            }
            if(KullInvalidTargets && potentialTargets.Any(t => t.IsValidForCurrentPicker))
            {
                return potentialTargets.Where(t => t.IsValidForCurrentPicker);
            }
            return potentialTargets;
        }
    }
}
