using Assets.Src.Interfaces;
using Assets.Src.ModuleSystem;
using Assets.Src.ObjectManagement;

namespace Assets.Src.Controllers
{
    public abstract class AbstractDeactivatableController : GeneticConfigurableMonobehaviour, IDeactivatable
    {
        
        public const string InactiveTag = "Untagged";

        protected bool _active = true;

        public virtual void Deactivate()
        {
            
            _active = false;
            tag = InactiveTag;
        }
    }
}
