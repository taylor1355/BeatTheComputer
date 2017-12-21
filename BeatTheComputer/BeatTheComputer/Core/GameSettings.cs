using System;
using System.Linq;

namespace BeatTheComputer.Core
{
    abstract class GameSettings : IEquatable<GameSettings>
    {
        public Type GameType { get; }

        public GameSettings(Type gameType)
        {
            if (!gameType.GetInterfaces().Contains(typeof(IGameContext))) {
                throw new ArgumentException("Game type must implement IGameContext");
            }

            GameType = gameType;
        }

        public abstract IGameContext newContext();

        public abstract string guid();

        public bool Equals(GameSettings context) { return equalTo(context); }
        public override bool Equals(object obj) { return equalTo(obj); }
        public abstract bool equalTo(object obj);
        public override int GetHashCode() { throw new NotImplementedException(); }
    }
}
