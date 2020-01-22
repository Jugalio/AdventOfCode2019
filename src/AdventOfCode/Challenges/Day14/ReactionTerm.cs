using Extension.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.Day14
{
    /// <summary>
    /// One part of the chemical reation
    /// </summary>
    public class ReactionTerm
    {
        public string Element;
        public long Quantity;

        public ReactionTerm(string element, long quantity)
        {
            Element = element;
            Quantity = quantity;
        }

        public static ReactionTerm operator +(ReactionTerm t1, long quantity)
        {
            return new ReactionTerm(t1.Element, t1.Quantity + quantity);
        }

        public static ReactionTerm operator *(ReactionTerm t1, long multiplier)
        {
            return new ReactionTerm(t1.Element, t1.Quantity * multiplier);
        }

        public static ReactionTerm operator -(ReactionTerm t1, long quantity)
        {
            return new ReactionTerm(t1.Element, t1.Quantity - quantity);
        }
    }
}
