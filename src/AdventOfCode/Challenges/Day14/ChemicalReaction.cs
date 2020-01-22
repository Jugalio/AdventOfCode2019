using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day14
{
    public class ChemicalReaction
    {
        /// <summary>
        /// The input for this reaction
        /// </summary>
        public List<ReactionTerm> Inputs = new List<ReactionTerm>();

        /// <summary>
        /// The output produced by this reaction
        /// </summary>
        public ReactionTerm Output;

        /// <summary>
        /// Creates a new chemical reaction object
        /// </summary>
        /// <param name="reactionEquation"></param>
        public ChemicalReaction(string reactionEquation)
        {
            var firstSplit = reactionEquation.Split("=>");

            var inputs = firstSplit[0];
            var inputParts = inputs.Split(',');
            foreach(string part in inputParts)
            {
                Inputs.Add(GetReactionTerm(part));
            }

            var output = firstSplit[1];
            Output = GetReactionTerm(output);
        }

        /// <summary>
        /// Takes one part of a equation and return quatity and the element name
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private ReactionTerm GetReactionTerm(string part)
        {
            var trimmed = part.Trim();
            var split = trimmed.Split(' ');
            return new ReactionTerm(split[1], long.Parse(split[0]));
        }

    }
}
