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
        public Tactic? Parent = null;
        public TacticType TacticType { get; set; }
        private LinkedList<Tactic> _subTactics { get; set; }

        /// <summary>
        /// Gets or sets the guard of the tactic.
        /// </summary>
        protected Func<bool> Guard { get; set; } = () => true;
        public Tactic(TacticType tacticType, List<Tactic> subTactics)
        {
            TacticType = tacticType;
            _subTactics = new();

            foreach (Tactic tactic in subTactics)
            {
                tactic.Parent = this;
                _ = _subTactics.AddLast(tactic);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/> class with the specified tactic type, sub-tactics, and guard.
        /// </summary>
        /// <param name="tacticType">The type of the tactic.</param>
        /// <param name="subTactics">The sub-tactics of the tactic.</param>
        /// <param name="guard">The guard of the tactic.</param>
        public Tactic(TacticType tacticType, List<Tactic> subTactics, Func<bool> guard)
        {
            TacticType = tacticType;
            _subTactics = new();
            Guard = guard;

            foreach (Tactic tactic in subTactics)
            {
                tactic.Parent = this;
                _subTactics.AddLast(tactic);
            }
        }

        public Tactic? GetNextTactic()
        {
            if (Parent == null) return null;

            if (TacticType == TacticType.Primitive)
            {
                PrimitiveTactic tactic = (PrimitiveTactic)this;

                return tactic;
            }

            return Parent.GetNextTactic();
        }

        public List<PrimitiveTactic> GetFirstEnabledActions()
        {
            List<PrimitiveTactic> primitiveTactics = new();

            switch (TacticType)
            {
                case TacticType.FirstOf:
                    foreach (Tactic subTactic in _subTactics)
                    {
                        primitiveTactics = subTactic.GetFirstEnabledActions();

                        if (primitiveTactics.Count > 0) return primitiveTactics;
                    }
                    break;
                case TacticType.AnyOf:
                    foreach (Tactic subTactic in _subTactics)
                    {
                        primitiveTactics.AddRange(subTactic.GetFirstEnabledActions());
                    }
                    break;
                case TacticType.Primitive:
                    PrimitiveTactic tactic = (PrimitiveTactic)this;

                    if (tactic.Action.IsActionable()) primitiveTactics.Add(tactic);

                    break;
            }

            return primitiveTactics;
        }

        /// <summary>
        /// Determines whether the tactic is actionable.
        /// </summary>
        /// <returns>True if the tactic is actionable, false otherwise.</returns>
        public bool IsActionable() => Guard();
    }

    public class PrimitiveTactic : Tactic
    {
        public Action Action;

        public PrimitiveTactic(Action action) : base(TacticType.Primitive, new())
        {
            Action = action;
        }
    }
}
