using Assets.Src.ObjectManagement;
using Assets.Src.Targeting;
using UnityEngine;

public class RegisterAsTarget : MonoBehaviour
{
   
    public bool Shooting = true;

   
    public bool Navigation = false;

  
    void Start () {
        var target = new Target(transform);
        if (Navigation)
        {
            TargetRepository.RegisterNavigationTarget(target);
        }
        if (Shooting)
        {
            TargetRepository.RegisterTarget(target);
        }
	}

    public void Deactivate()
    {
        TargetRepository.DeregisterTarget(transform);
    }
}
