using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    class CheckersAction : IAction
    {
        Position start;
        Position destination;
        List<Position> jumps;

        public CheckersAction(List<Position> sequence)
        {
            start = sequence[0];
            destination = sequence[sequence.Count - 1];

            jumps = new List<Position>();
            if (Math.Abs((sequence[1] - sequence[0]).Row) > 1) {
                for (int i = 0; i < sequence.Count - 1; i++) {
                    jumps.Add((sequence[i] + sequence[i + 1]) / 2);
                }
            }
        }

        private CheckersAction(Position start, Position destination, List<Position> jumps)
        {
            this.start = start;
            this.destination = destination;
            this.jumps = jumps;
        }

        public bool isValid(IGameContext context)
        {
            return context.getValidActions().Contains(this);
        }

        public bool Equals(IAction other)
        {
            return equalTo(other);
        }

        public override bool Equals(object obj)
        {
            return equalTo(obj);
        }

        private bool equalTo(object obj)
        {
            CheckersAction other = obj as CheckersAction;
            if (other == null || start != other.start || destination != other.destination || jumps.Count != other.jumps.Count) {
                return false;
            }
            for (int i = 0; i < jumps.Count; i++) {
                if (jumps[i] != other.jumps[i]) {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = jumps.Count + start.GetHashCode() * 53 + destination.GetHashCode() * 193;
            foreach (Position pos in jumps) {
                hashCode ^= pos.GetHashCode();
            }
            return hashCode;
        }

        public IAction clone()
        {
            return new CheckersAction(start, destination, new List<Position>(jumps));
        }

        public Position Start {
            get { return start; }
        }

        public Position Destination {
            get { return destination; }
        }

        public List<Position> Jumps {
            get { return jumps; }
        }
    }
}
