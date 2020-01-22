using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Text;
using Extension.Mathematics;

namespace AdventOfCode.Challenges.Day14
{
    /// <summary>
    /// The nano factory is capable of some chemical transformations
    /// by starting chemical reactions
    /// </summary>
    public class NanoFactory
    {
        /// <summary>
        /// All reations which can be performed by this nanofactory
        /// </summary>
        public List<ChemicalReaction> Reactions;

        /// <summary>
        /// Creates a new instance of the nano factory with
        /// a list of reactions
        /// </summary>
        /// <param name="reactions">Each string is one reactions</param>
        public NanoFactory(IEnumerable<string> reactions)
        {
            Reactions = reactions.Select(r => new ChemicalReaction(r)).ToList();
        }

        /// <summary>
        /// Get the maximum production capactity of the target element
        /// given a defined amount of one input material
        /// </summary>
        /// <param name="target"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public long GetProductionCapacity(string target, ReactionTerm input)
        {
            var forOne = GetRawMaterialFor(new ReactionTerm(target, 1)).FirstOrDefault(m => m.Element == input.Element);

            //The minimum if no waste is produced in the full reaction chain
            var min = input.Quantity / forOne.Quantity;
            var max = min * 4;

            while(min != max)
            {
                var middle = min + (max - min) / 2;
                var need = GetRawMaterialFor(new ReactionTerm(target, middle)).FirstOrDefault(m => m.Element == input.Element);

                if (need.Quantity == input.Quantity)
                {
                    max = min = middle;
                }
                else if (need.Quantity < input.Quantity)
                {
                    min = middle;
                }
                else
                {
                    max = middle;
                }

                if (max == min + 1)
                {
                    break;
                }
            }

            return min;
        }

        /// <summary>
        /// Get all elements that need to be provided as raw material to start a series
        /// of reaction which lead to the production of the given target element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public List<ReactionTerm> GetRawMaterialFor(ReactionTerm target)
        {
            var tryToProduce = new Queue<ReactionTerm>();
            var overProductions = new List<ReactionTerm>();
            var rawMaterials = new List<ReactionTerm>();

            tryToProduce.Enqueue(target);

            while(tryToProduce.Count != 0)
            {
                var next = tryToProduce.Dequeue();
                next = UpdateDemand(ref overProductions, next);
                if (next.Quantity != 0)
                {
                    var hasReaction = FindReaction(next, out var inputs, out var overProduction);
                    if (hasReaction)
                    {
                        inputs.ForEach(i => tryToProduce.Enqueue(i));

                        //Add the overproduction
                        next.Quantity = overProduction;
                        AddOverProduction(ref overProductions,  next);
                    }
                    else
                    {
                        AddRawMaterial(ref rawMaterials, next);
                    }
                }
            }

            return rawMaterials;
        }

        /// <summary>
        /// Adds new raw material to the list of needed materials
        /// </summary>
        private void AddRawMaterial(ref List<ReactionTerm> rawMaterials, ReactionTerm newElement)
        {
            var existing = rawMaterials.FirstOrDefault(e => e.Element == newElement.Element);
            if (existing != null)
            {
                var index = rawMaterials.IndexOf(existing);
                rawMaterials[index] = existing + newElement.Quantity;
            }
            else
            {
                rawMaterials.Add(newElement);
            }
        }

        /// <summary>
        /// Adds a new overproduction
        /// </summary>
        /// <param name="overProductions"></param>
        /// <param name="newOverProduction"></param>
        private void AddOverProduction(ref List<ReactionTerm> overProductions, ReactionTerm newOverProduction)
        {
            var existing = overProductions.FirstOrDefault(l => l.Element == newOverProduction.Element);
            if (existing != null)
            {
                var index = overProductions.IndexOf(existing);
                overProductions[index] = existing + newOverProduction.Quantity;
            }
            else
            {
                overProductions.Add(newOverProduction);
            }
        }

        /// <summary>
        /// Updates the demand on a specific element depending on the overProduction still stored
        /// </summary>
        /// <param name="leftOvers"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private ReactionTerm UpdateDemand(ref List<ReactionTerm> overProductions, ReactionTerm target)
        {
            var existing = overProductions.FirstOrDefault(l => l.Element == target.Element);
            if (existing != null)
            {
                var index = overProductions.IndexOf(existing);
                if (existing.Quantity >= target.Quantity)
                {
                    overProductions[index] = existing - target.Quantity;
                    target.Quantity = 0;
                }
                else
                {
                    target -= existing.Quantity;
                    overProductions[index].Quantity = 0;
                }
                
                return target;
            }

            return target;
        }

        /// <summary>
        /// Gets the list of input elemez to produce a specific element
        /// with exatly one reaction
        /// </summary>
        /// <param name="element"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool FindReaction(ReactionTerm target, out List<ReactionTerm> inputs, out long overProduction)
        {
            var reaction = Reactions.FirstOrDefault(r => r.Output.Element == target.Element);

            if(reaction == null)
            {
                overProduction = 0;
                inputs = null;
                return false;
            }
            else
            {
                var multiplier = (long)Math.Ceiling((double)target.Quantity / reaction.Output.Quantity);
                overProduction = Operations.SafeMultiplication(reaction.Output.Quantity, multiplier) - target.Quantity;
                inputs = reaction.Inputs.Select(i => i * multiplier).ToList();
                return true;
            }
        }

    }
}
