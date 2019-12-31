using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day6
{
    public class OrbitMap
    {
        /// <summary>
        /// All orbit relations between objects
        /// </summary>
        public List<string> OrbitRelations;

        /// <summary>
        /// All Objects in space
        /// </summary>
        public HashSet<SpaceObject> SpaceObjects = new HashSet<SpaceObject>();

        /// <summary>
        /// Creates an orbit map from a list of oribital relations
        /// </summary>
        /// <param name="relations"></param>
        public OrbitMap(IEnumerable<string> relations)
        {
            OrbitRelations = relations.ToList();
            OrbitRelations.SelectMany(rel => rel.Split(')')).ToHashSet().ToList().ForEach(s => InitObject(s));
        }

        /// <summary>
        /// Get the number of orbits in this map
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfOrbits()
        {
            return SpaceObjects.Select(obj => obj.GetOrbitCount()).Sum();
        }

        /// <summary>
        /// Get all orbits for a sepcific space object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private SpaceObject InitObject(string id)
        {
            var existingObj = SpaceObjects.FirstOrDefault(o => o.Id == id);
            if (existingObj == null)
            {
                var newObj = new SpaceObject(id);
                var directOrbit = OrbitRelations.FirstOrDefault(s => s.Split(')')[1] == id)?.Split(')')[0];
                if (directOrbit != null) 
                {
                    newObj.DirectlyOrbits = InitObject(directOrbit);
                    newObj.Orbits.Add(directOrbit);
                    newObj.DirectlyOrbits.Orbits.ForEach(a => newObj.Orbits.Add($"{directOrbit}-{a}"));
                };

                SpaceObjects.Add(newObj);
                return newObj;
            }
            else
            {
                return existingObj;
            }
        }

    }
}
