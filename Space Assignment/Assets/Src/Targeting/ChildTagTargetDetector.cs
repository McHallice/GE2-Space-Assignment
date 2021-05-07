﻿using Assets.Src.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Src.Targeting
{
    public class ChildTagTargetDetector : ITargetDetector
    {
        public List<string> Tags = new List<string> { "SpaceShip" };
        public float ProjectileSpeed = 0;

        public ChildTagTargetDetector()
        {

        }

        public IEnumerable<PotentialTarget> DetectTargets(bool includeNavigationTarets = false)
        {
            var targets = new List<PotentialTarget>();
            foreach (var tag in Tags)
            {
                var gameObjects = GameObject.FindGameObjectsWithTag(tag)
                    .Select(o => o.transform.parent)
                    .Where(o => o != null && o.GetComponent<Rigidbody>());
               
                targets.AddRange(gameObjects.Select(g => new PotentialTarget(g.transform)));
            }

            

            return targets;
        }
    }
}
