using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    struct CheckersAction : IAction
    {
        Position start;
        Position destination;
        Position[] jumps;

        public CheckersAction(params Position[] sequence)
        {
            start = sequence[0];
            destination = sequence[sequence.Length - 1];

            if (Math.Abs((sequence[1] - sequence[0]).Row) > 1) {
                jumps = new Position[sequence.Length - 1];
                for (int i = 0; i < sequence.Length - 1; i++) {
                    jumps[i] = (sequence[i] + sequence[i + 1]) / 2;
                }
            } else {
                jumps = null;
            }
        }

        public CheckersAction(IList<Position> sequence)
        {
            start = sequence[0];
            destination = sequence[sequence.Count - 1];

            if (Math.Abs((sequence[1] - sequence[0]).Row) > 1) {
                jumps = new Position[sequence.Count - 1];
                for (int i = 0; i < sequence.Count - 1; i++) {
                    jumps[i] = (sequence[i] + sequence[i + 1]) / 2;
                }
            } else {
                jumps = null;
            }
        }

        private CheckersAction(Position start, Position destination, Position[] jumps)
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
            if (!(obj is CheckersAction)) {
                return false;
            }

            CheckersAction other = (CheckersAction) obj;
            if (start != other.start || destination != other.destination || NumJumps != other.NumJumps) {
                return false;
            }

            for (int i = 0; i < NumJumps; i++) {
                if (jumps[i] != other.jumps[i]) {
                    return false;
                }
            }

            return true;
        }

        //there is only be 1 valid action per destination, so factoring jumps into hashcode is unnecessary
        public override int GetHashCode()
        {
            return NumJumps + start.GetHashCode() * 53 + destination.GetHashCode() * 193;
        }

        public Position Start {
            get { return start; }
        }

        public Position Destination {
            get { return destination; }
        }

        public int NumJumps {
            get {
                if (jumps == null) {
                    return 0;
                } else {
                    return jumps.Length;
                }
            }
        }

        public Position[] Jumps {
            get { return jumps; }
        }
    }
}
