using Assets.Src.Interfaces;
using Assets.Src.ObjectManagement;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurret : MonoBehaviour
{
    public IKnowsEnemyTags EnemyTagSource;

   
    public bool TagChildren = true;

    public Transform ParentForTurret;
    
   
    public List<string> EnemyTags;

    public Transform TurretPrefab;

	// Use this for initialization
	void Start () {
        if(TurretPrefab != null)
        {
            if(EnemyTagSource == null && transform.parent != null)
            {
                EnemyTagSource = transform.GetComponentInParent<IKnowsEnemyTags>();
            }

            if(EnemyTagSource != null)
            {
                EnemyTags = EnemyTagSource.KnownEnemyTags;
            }

            if(ParentForTurret == null && transform.parent != null)
            {
                ParentForTurret = transform.parent;
            }

            var turret = Instantiate(TurretPrefab, transform.position, transform.rotation, ParentForTurret);

            if(ParentForTurret != null)
            {
                var parentRigidbody = ParentForTurret.GetComponent<Rigidbody>();
                if(parentRigidbody != null)
                {
                    turret.SetVelocity(parentRigidbody.velocity);
                }

                turret.parent = ParentForTurret;
                turret.GetComponent<FixedJoint>().connectedBody = parentRigidbody;

                var renderer = ParentForTurret.GetComponentInChildren<Renderer>();

                if (renderer != null)
                {
                    //Debug.Log("has renderer");
                    turret.transform.SetColor(renderer.material.color);
                }
            }

            var tagKnower = turret.GetComponent<IKnowsEnemyTags>();
            if(tagKnower != null)
            {
                tagKnower.KnownEnemyTags = EnemyTags;
            }

            if (TagChildren)
            {
                turret.tag = ParentForTurret != null ? ParentForTurret.tag : tag;
            }
        
            Destroy(gameObject);
        } else
        {
            Debug.LogWarning(name + " Has no turret to spawn.");
        }
    }
}
