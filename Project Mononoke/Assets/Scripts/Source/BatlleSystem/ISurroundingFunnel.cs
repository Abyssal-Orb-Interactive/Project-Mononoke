using System;

namespace Source.BattleSystem
{
    public interface ISurroundingFunnel
    {
        public void Add(IDamager surrounder);
        public int SurroundersCount { get; }
    }
}