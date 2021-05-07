using Assets.Src.Health;
using Assets.Src.Interfaces;
using Assets.Src.ObjectManagement;
using System.Collections.Generic;
using UnityEngine;

public class HealthControler : MonoBehaviour
{
    /// <summary>
    ///damage = collider momentum/resistance 
    /// </summary>
    public float Resilience = 10;

    /// <summary>
    /// final damage = damage after resilience applied - armour (min of 0, obviously)
    /// </summary>
    public float Armour = 0;

    public float Health = 200;

    public Rigidbody DeathExplosion;

    private IDestroyer _destroyer;
    
    public Rigidbody Shrapnel;
    public int ShrapnelCount2 = 30;
    public float ShrapnelSpeed2 = 20;
    
    public float SecondsOfInvulnerability = 1;

    private Rigidbody _rigidbody;
    public float OriginalHealth;

    
    public HealthControler DamageDelegate;

   
    public List<string> IgnoredTags = new List<string>
    {
        "ForceField"
    };

   
    public string TeamTagForceFieldSuffix = "FoceField";

    // Use this for initialization
    void Start()
    {
        OriginalHealth = Health;
        _rigidbody = GetComponent<Rigidbody>();

        var exploder = new ShrapnelExploder(_rigidbody, Shrapnel, DeathExplosion, ShrapnelCount2)
        {
            ShrapnelSpeed = ShrapnelSpeed2
        };

        _destroyer = new WithChildrenDestroyer()
        {
            Exploder = exploder,
            UntagChildren = false
        };

        if (!string.IsNullOrEmpty(TeamTagForceFieldSuffix))
        {
            IgnoredTags.Add(tag + TeamTagForceFieldSuffix);
        }
    }

    void FixedUpdate()
    {
        if(SecondsOfInvulnerability > 0)
        {
            SecondsOfInvulnerability -= Time.fixedDeltaTime;
            return;
        }
        if (Health <= 0)
        {
            //Debug.Log(transform + " is dead from lack of health");
            _destroyer.Destroy(gameObject, true);
        }
    }

    
    
    public void ApplyDamage(float damage)
    {
        if (SecondsOfInvulnerability > 0)
        {
            return;
        }
        if(DamageDelegate != null)
        {
            
            DamageDelegate.ApplyDamage(damage);

           
            damage = DamageDelegate.Health > 0 ? 0 : -DamageDelegate.Health;
            
        }
        Health -= damage;
    }

    public void ApplyDamage(DamagePacket damage)
    {
        if(damage.IsAOE && DamageDelegate != null)
        {
           
        } else
        {
            ApplyDamage(damage.Damage);
        }
    }

    public bool IsDamaged
    {
        get
        {
            return Health < (OriginalHealth * 0.99);
        }
    }

    
    public float HealthProportion { get
        {
            if(OriginalHealth != 0)
            {
                return Health / OriginalHealth;
            } else
            {
                Debug.LogWarning("Avoided div0 error");
                return Health;
            }
        }
    }
}
