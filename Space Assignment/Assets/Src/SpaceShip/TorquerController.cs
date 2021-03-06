using Assets.Src.Controllers;
using Assets.Src.Evolution;
using Assets.Src.ObjectManagement;
using UnityEngine;

public class TorquerController : AbstractDeactivatableController
{
    
    void Start()
    {
        Transform parent = transform.FindOldestParent();

        if (parent != transform)
        {
            NotifyParent(parent);
        }
    }

    private void NotifyParent(Transform parent)
    {
        parent.SendMessage("RegisterTorquer", transform, SendMessageOptions.DontRequireReceiver);
    }

    protected override GenomeWrapper SubConfigure(GenomeWrapper genomeWrapper)
    { 
        return genomeWrapper;
    }
}
