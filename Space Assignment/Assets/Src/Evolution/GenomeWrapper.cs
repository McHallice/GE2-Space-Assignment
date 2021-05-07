using Assets.Src.Interfaces;
using Assets.Src.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Src.Evolution
{
    public class GenomeWrapper
    {
        private readonly int _geneLength;
        private readonly string _genome;
        private const int DEFAULT_GENE_LENGTH = 3;
        private const float DEFAULT_BUDGET = 1000;

        public string Genome { get
            {
                return _genome;
            }
        }
        public float Cost { get; private set; }
        public float? Budget { get; set; }
        private int _position;
        public int Position { get { return _position; } }
        private readonly Stack<int> _previousPositions = new Stack<int>();

        public Dictionary<ModuleType, int> ModuleTypeCounts { get; private set; }
        public int ModulesAdded { get; private set; }

        public List<Vector3> UsedLocations { get; private set; }

        public bool UseJump = true;

        private readonly ModuleRecord _topModuleRecord;

        public string Species
        {
            get
            {
                return _topModuleRecord.ToSimpleString();
            }
        }

        public string VerboseSpecies
        {
            get
            {
                return _topModuleRecord.ToSimpleStringWithFullNames();
            }
        }

        public string Subspecies
        {
            get
            {
                return _topModuleRecord.ToString();
            }
        }

        public string VerboseSubspecies
        {
            get
            {
                return _topModuleRecord.ToStringWithFullNames();
            }
        }

        private readonly Stack<ModuleRecord> _previousModuleRecords = new Stack<ModuleRecord>();
        private ModuleRecord _currentModuleRecord;

        public string Tag { get; set; }

        public GenomeWrapper(string genome, float budget = DEFAULT_BUDGET, int geneLength = DEFAULT_GENE_LENGTH)
        {
            _genome = genome;
            _geneLength = geneLength;
            Budget = budget;
            UsedLocations = new List<Vector3>();
            ModuleTypeCounts = new Dictionary<ModuleType, int>();
            _topModuleRecord = new ModuleRecord();
            _currentModuleRecord = _topModuleRecord;
        }


        
        public bool ConfigureAddedModule(IModuleTypeKnower knower, Vector3 usedLocation, int? moduleNumber)
        {
            if (knower != null)
            {
                foreach (var type in knower.ModuleTypes.Distinct())
                {
                    if (ModuleTypeCounts.ContainsKey(type))
                    {
                        ModuleTypeCounts[type]++;
                    } else
                    {
                        ModuleTypeCounts[type] = 1;
                    }
                }
                ModulesAdded++;

                Cost += knower.ModuleCost;

                UsedLocations.Add(usedLocation);

                var nextMR = new ModuleRecord(knower, moduleNumber);
                _currentModuleRecord.AddModule(nextMR);
                _previousModuleRecords.Push(_currentModuleRecord);
                _currentModuleRecord = nextMR;

                
                Jump();
                knower.Configure(this);
                ModuleFinished();
                JumpBack();
            }
            else
            {
                NoModuleAddedHere();
            }
            return CanSpawn();
        }

       
        public void NoModuleAddedHere()
        {
            _currentModuleRecord.AddModule(null);
        }

      
        private void ModuleFinished()
        {
            _currentModuleRecord = _previousModuleRecords.Pop();
        }

        public string Name
        {
            get
            {
                return _topModuleRecord.ToString();
            }
        }

        public bool CanSpawn()
        {
            var isUnderBudget = IsUnderBudget();
            var canSpawn =
                isUnderBudget;
                
            return canSpawn;
        }
        
        public bool IsUnderBudget()
        {
            return !Budget.HasValue || Cost < Budget.Value;
        }

      
        public string GetGene()
        {
            var gene = new StringBuilder();

            for (int i = 0; i < _geneLength; i++)
            {
                var character =  _genome[_position];
                _position = (_position+1) % _genome.Length;
                gene.Append(character);
            }
            
            return gene.ToString();
        }

       
        public void Jump()
        {
            if (UseJump) {
                var gene = GetGeneAsInt();
                _previousPositions.Push(_position);
                if (gene.HasValue)
                {
                    _position = gene.Value;
                }
            }
        }

       
        public void JumpBack()
        {
            if (UseJump)
            {
                if (_previousPositions.Any())
                {
                    _position = _previousPositions.Pop();
                }
                else
                {
                    Debug.LogWarning("Tried to jump back without having jumped.");
                }
            }
        }

       
        public int? GetGeneAsInt()
        {
            var substring = GetGene();
            

            var simplified = substring.Replace(" ", "");
            if (string.IsNullOrEmpty(simplified))
            {
                return null;
            }

            if (int.TryParse(simplified, out int number))
            {
                return number;
            }
            return null;
        }

        public float GetProportionalNumber(float defaultProporion = 0.5f)
        {

            var substring = GetGene();

            var simplified = substring.TrimStart().Replace(" ", "0");
           
            int number;
            if (int.TryParse(simplified, out number))
            {
                if ((Math.Pow(10, _geneLength) - 1) != 0)
                {
                    return (float)(number / (Math.Pow(10, _geneLength) - 1));
                }
                else
                {
                    Debug.LogWarning("Avoided div 0");
                } 
            }

           
            return defaultProporion;
        }

        public float GetScaledNumber(float max, float min = 0, float defaultProportion = 0.5f)
        {
            var range = max - min;
            var randomValue = GetProportionalNumber(defaultProportion) * range;
            return min + randomValue;
        }

        public Color? ColorOverride = null;

        
        public Color GetColorForGenome()
        {
            if (ColorOverride.HasValue)
            {
                return ColorOverride.Value;
            }
            var r = GetNumberFromGenome(0, 0.5f, 8);
            var g = GetNumberFromGenome(10, 0.5f, 8);
            var b = GetNumberFromGenome(20, 0.5f, 8);

            return new Color(r, g, b);
        }

       
        private float GetNumberFromGenome(int fromEnd, float defaultProporion = 0.5f, int length = 2)
        {
            var reversed = Reverse(_genome);
            if (fromEnd + length < reversed.Length)
            {
                var substring = reversed.Substring(fromEnd, length);

                var simplified = substring.TrimStart().Replace(" ", "0");
               
                int number;
                if (int.TryParse(simplified, out number))
                {
                    if ((Math.Pow(10, length) - 1) != 0)
                        return (float)(number / (Math.Pow(10, length) - 1));
                    else
                        Debug.LogWarning("Avoided div0 error");
                }
            }
           
            return defaultProporion;
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
