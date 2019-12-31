using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day6
{
    public class SpaceObject
    {
        public string Id;

        public List<string> Orbits = new List<string>();
        public SpaceObject DirectlyOrbits;

        public SpaceObject(string id)
        {
            Id = id;
        }

        public int GetOrbitCount()
        {
            return Orbits.Count();
        }

        public int GetDistanceTo(string id)
        {
            var orbitalPath = GetPathTo(id);
            if (orbitalPath == null)
            {
                return -1;
            }
            else
            {
                return orbitalPath.Split('-').Count();
            }
        }

        public List<string> GetRachableObjects()
        {
            return Orbits.Select(o => o.Split('-').Last()).ToList();
        }

        public string GetPathTo(string id)
        {
            return Orbits.FirstOrDefault(o => o.EndsWith(id));
        }

        /// <summary>
        /// Returns the shortest transfer route
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public (int distance, string path) GetTransfersToReach(SpaceObject target)
        {
            //A transfer only happens from object directly orbit by this object and the target object
            var objA = DirectlyOrbits;
            var objB = target.DirectlyOrbits;
            var reachableA = objA.GetRachableObjects();

            if (reachableA.Contains(objB.Id))
            {
                return (objA.GetDistanceTo(objB.Id), GetPathTo(objB.Id));
            }

            var reachableB = objB.GetRachableObjects();

            if (reachableB.Contains(objA.Id))
            {
                return (objB.GetDistanceTo(objA.Id), target.GetPathTo(objA.Id));
            }

            var commonOrbits = reachableA.Where(o => reachableB.Contains(o)).ToList();

            if (commonOrbits.Count() != 0)
            {
                var distance = commonOrbits.Select(o => objA.GetDistanceTo(o) + objB.GetDistanceTo(o)).ToList();
                var shortestDistance = distance.Min();
                var nearestCommonObj = commonOrbits[distance.LastIndexOf(shortestDistance)];
                var path = GetPathTo(nearestCommonObj) + string.Concat(target.GetPathTo(nearestCommonObj).Reverse()).Substring(1);
                return (shortestDistance, path);
            }

            throw new ArgumentException($"There is no path from {Id} to {target.Id}");
        }

    }
}
