﻿using Assets.Src.Evolution;
using Assets.Src.Interfaces;
using Assets.Src.ModuleSystem;
using Assets.Src.ObjectManagement;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.src.Evolution
{
    public class ShipBuilder
    {
        private GenomeWrapper _genome;
        public int GeneLength = 1;

        private ModuleList _moduleList;
        
        private ModuleHub _rootHub;
        private TestCubeChecker _testCubePrefab;
        
        private Color _colour;
        
        public ShipBuilder(GenomeWrapper genomeWrapper, ModuleHub rootHub)
        {
            _rootHub = rootHub;
            if (_rootHub == null)
            {
                throw new ArgumentNullException("shipToBuildOn", "shipToBuildOn must be a valid Transform.");
            }
            _moduleList = _rootHub.ModuleList;
            if(_moduleList == null)
            {
                throw new ArgumentNullException("moduleList", "moduleList must be a valid ModuleList objet.");
            }
            _genome = genomeWrapper;
            _testCubePrefab = _rootHub.TestCube;
        }

        public GenomeWrapper BuildShip(bool setColour = true)
        {
            //Debug.Log("Building " + _genome);
            if (setColour)
            {
                _colour = _genome.GetColorForGenome();
                _rootHub.transform.SetColor(_colour);
            }

            //Debug.Log("Spawning modules on " + _hubToBuildOn.name);

            _genome.UsedLocations.Add(_rootHub.transform.position);
            _genome = SpawnModules();
            
            return _genome;
        }

        [Obsolete("This shouldn't be setable")]
        public void SetGenome(GenomeWrapper genomeWrapper)
        {
            _genome = genomeWrapper;
        }

        private GenomeWrapper SpawnModules()
        {
            var spawnPoints = _rootHub.SpawnPoints;

            foreach (var spawnPoint in spawnPoints)
            {
                var newUsedLocation = Vector3.zero;
                if (CanSpawnHere(spawnPoint, out newUsedLocation))
                {
                    int? moduleIndex;
                    var moduleToAdd = SelectModule(out moduleIndex);

                    if (moduleToAdd != null)
                    {
                        if(_genome.IsUnderBudget())
                        {
                            //Debug.Log("adding " + moduleToAdd + " total cost = " + _genome.Cost);
                            var addedModule = GameObject.Instantiate(moduleToAdd, spawnPoint.position, spawnPoint.rotation, _rootHub.transform);
                            
                            addedModule.GetComponent<FixedJoint>().connectedBody = _rootHub.GetComponent<Rigidbody>();
                            
                            var hub = addedModule.GetComponent<ModuleHub>();
                            if(hub != null)
                            {
                                hub.AllowedModuleIndicies = _rootHub.AllowedModuleIndicies;
                            }

                            addedModule.tag = _rootHub.tag;

                            addedModule.transform.SetColor(_colour);
                            addedModule.GetComponent<Rigidbody>().velocity = _rootHub.Velocity;

                            addedModule.transform.SetColor(_colour);

                            _genome.ConfigureAddedModule(addedModule, newUsedLocation, moduleIndex);
                            continue;
                        }
                       
                    }
                   
                }
               
                _genome.NoModuleAddedHere();
            }
            return _genome;
        }

        private const float THRESHOLD_DISTANCE = 1;

        private bool CanSpawnHere(Transform spawnPoint, out Vector3 newUsedLocation)
        {
            if (_testCubePrefab != null)
            {
               
                var testCube = GameObject.Instantiate(_testCubePrefab, spawnPoint.position, spawnPoint.rotation);
                var collider = testCube.GetComponent<BoxCollider>();
                var center = collider.center;
                newUsedLocation = testCube.transform.TransformPoint(center);
                GameObject.Destroy(testCube.gameObject);
                if (IsUsedLocation(newUsedLocation))
                {
                    Debug.Log("Can't spawn at " + center + " because there is already something here");
                    return false;
                }
            }
            else
            {
                newUsedLocation = Vector3.zero; 
            }
            var canSpawn = _genome.CanSpawn();
           
            return canSpawn;
        }

        private bool IsUsedLocation(Vector3 worldLocation)
        {
            var distances = _genome.UsedLocations.Select(l => Vector3.Distance(l, worldLocation));
            return distances.Any(d => d < THRESHOLD_DISTANCE);
        }

        private ModuleTypeKnower SelectModule(out int? moduleIndex)
        {
            if (_genome.CanSpawn())
            {
                int? number = _genome.GetGeneAsInt();
                if (number.HasValue)
                {
                    var numberInRange = number.Value % _moduleList.Modules.Count();
                    if (_rootHub.AllowedModuleIndicies == null || !_rootHub.AllowedModuleIndicies.Any() || _rootHub.AllowedModuleIndicies.Contains(numberInRange))
                    {
                        //Debug.Log("Adding Module " + number + ": " + Modules[number.Value % _moduleList.Modules.Count()] );
                        moduleIndex = numberInRange;
                        return _moduleList.Modules[numberInRange];
                    }
                   
                }
                
            }
           
            moduleIndex = null;
            return null;
        }
    }
}

