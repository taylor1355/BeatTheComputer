using BeatTheComputer.Core;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    struct CheckersAction : IAction
    {
        private Position start;
        private Position destination;
        private Position[] jumps;
        private bool leadsToPromotion;

        public CheckersAction(int promotionRow, params Position[] sequence)
        {
            start = sequence[0];
            destination = sequence[sequence.Length - 1];
            leadsToPromotion = false;

            if (Math.Abs((sequence[1] - sequence[0]).Row) > 1) {
                jumps = new Position[sequence.Length - 1];
                for (int i = 0; i < sequence.Length - 1; i++) {
                    jumps[i] = (sequence[i] + sequence[i + 1]) / 2;
                    if (sequence[i + 1].Row == promotionRow) {
                        leadsToPromotion = true;
                    }
                }
            } else {
                jumps = null;
                leadsToPromotion = destination.Row == promotionRow;
            }
        }

        public CheckersAction(int promotionRow, IList<Position> sequence)
        {
            start = sequence[0];
            destination = sequence[sequence.Count - 1];
            leadsToPromotion = false;

            if (Math.Abs((sequence[1] - sequence[0]).Row) > 1) {
                jumps = new Position[sequence.Count - 1];
                for (int i = 0; i < sequence.Count - 1; i++) {
                    jumps[i] = (sequence[i] + sequence[i + 1]) / 2;
                    if (sequence[i + 1].Row == promotionRow) {
                        leadsToPromotion = true;
                    }
                }
            } else {
                jumps = null;
                leadsToPromotion = destination.Row == promotionRow;
            }
        }

        // used for adding one more jump to a jump action
        public CheckersAction(int promotionRow, CheckersAction prevJumps, Position newDestination)
        {
            start = prevJumps.start;
            destination = newDestination;
            leadsToPromotion = prevJumps.leadsToPromotion || newDestination.Row == promotionRow;

            jumps = new Position[prevJumps.NumJumps + 1];
            Array.Copy(prevJumps.jumps, jumps, prevJumps.NumJumps);
            jumps[jumps.Length - 1] = (prevJumps.destination + newDestination) / 2;
        }

        private CheckersAction(CheckersAction cloneFrom)
        {
            start = cloneFrom.start;
            destination = cloneFrom.destination;

            if (cloneFrom.NumJumps == 0) {
                jumps = null;
            } else {
                jumps = (Position[]) cloneFrom.jumps.Clone();
            }

            leadsToPromotion = cloneFrom.leadsToPromotion;
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

        public IAction clone()
        {
            return new CheckersAction(this);
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

        public bool LeadsToPromotion {
            get { return leadsToPromotion; }
        }
    }
}
