using System.Collections.Generic;

namespace Aplib.Core
{
    public enum TacticType
    {
        Primitive,
        FirstOf,
        AnyOf,
    }

    public class Tactic
    {
        Tactic? parent = null;
        public TacticType TacticType { get; set; }
        LinkedList<Tactic> SubTactics { get; set; }

        public Tactic(TacticType tacticType, List<Tactic> subTactics)
        {
            TacticType = tacticType;
            SubTactics = new();

            foreach (Tactic tactic in subTactics)
            {
                tactic.parent = this;
                SubTactics.AddLast(tactic);
            }
        }

        public Tactic? getNextTactic()
        {
            if (parent == null) return null;

            if (TacticType == TacticType.Primitive)
            {
                PrimitiveTactic tactic = (PrimitiveTactic)this;

                if (true) return this; // TODO: add check if action is completed
            }

            return parent.getNextTactic();
        }
    }

    public class PrimitiveTactic : Tactic
    {
        public PrimitiveTactic() : base(TacticType.Primitive, new())
        {

        }
    }
}
