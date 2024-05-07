using System;
using Source.Character.AI;

namespace Source.Formations
{
    public struct PriorityPathfinder : IComparable<PriorityPathfinder>
    {
        public PathfinderAI AI { get; }
        public int Priority { get; }

        public PriorityPathfinder(PathfinderAI pathfinderAI, int priority)
        {
            AI = pathfinderAI;
            Priority = priority;
        }

        public int CompareTo(PriorityPathfinder other)
        {
           return Priority.CompareTo(other.Priority);
        }
    }
}