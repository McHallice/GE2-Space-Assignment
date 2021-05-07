using Assets.Src.Interfaces;
using Assets.Src.Targeting;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.ObjectManagement
{
    public class WithChildrenDestroyer : IDestroyer
    {
        public bool UntagChildren = true;
        public string DeadObjectTag = "Untagged";
        
        public IExploder Exploder;

       
        public bool KillCompletely = false;

        public void Destroy(GameObject toDestroy, bool useExplosion, Vector3? velocityOverride = null)
        {
            
            toDestroy = FindNextParentRigidbody(toDestroy);
            toDestroy.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);

            DestroyWithoutLookingForParent(toDestroy, useExplosion, velocityOverride);
        }

        private void DestroyWithoutLookingForParent(GameObject toDestroy, bool useExplosion, Vector3? velocityOverride)
        {
            var allChilldren = FindImediateChildren(toDestroy);
            foreach (var child in allChilldren)
            {
                child.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
                if (KillCompletely)
                {
                    DestroyWithoutLookingForParent(child.gameObject, false, velocityOverride);
                } else
                {
                    var rigidbody = child.GetComponent<Rigidbody>();
                    child.parent = null;

                    if (rigidbody != null)
                    {
                       
                        rigidbody.angularDrag = 0;
                        var fixedJoint = child.GetComponent<FixedJoint>();
                        if (fixedJoint != null)
                        {
                            Object.Destroy(fixedJoint);
                        }
                        var hingeJoint = child.GetComponent<HingeJoint>();
                        if (hingeJoint != null)
                        {
                            Object.Destroy(fixedJoint);
                        }
                        if(fixedJoint==null && hingeJoint == null)
                        {
                           
                            DestroyWithoutLookingForParent(child.gameObject, false, velocityOverride);
                        }
                    }
                    else
                    {
                        DestroyWithoutLookingForParent(child.gameObject, false, velocityOverride);
                    }
                }
            }

            if (useExplosion && Exploder != null)
            {
                var rigidBodyToExplode = toDestroy.GetComponent<Rigidbody>();
                Exploder.SetExplodingObject(rigidBodyToExplode);
                Exploder.ExplodeNow(velocityOverride);
            }

            toDestroy.transform.SendMessage("DieNow", SendMessageOptions.DontRequireReceiver);

            GameObject.Destroy(toDestroy);
        }

        private IEnumerable<Transform> FindImediateChildren(GameObject parent)
        {
            var children = new List<Transform>();
            var childCount = parent.transform.childCount;
            if(childCount > 0)
            {
            for(int i =0; i<childCount; i++)
                {
                    var child = parent.transform.GetChild(i);
                    children.Add(child);
                   
                }
            }
            return children;
        }

        private GameObject FindNextParentRigidbody(GameObject thing)
        {
            var rb = thing.GetComponent<Rigidbody>();

            if(rb != null || thing.transform.parent == null)
            {
               
                return thing;
            }
            return FindNextParentRigidbody(thing.transform.parent.gameObject);
        }
    }
}
