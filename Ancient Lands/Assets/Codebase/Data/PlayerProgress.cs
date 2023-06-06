using System;
using UnityEngine;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public WorldData WorldData;
    public State HeroState;
    public Stats Stats;
    public KillData KillData;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroState = new State();
      Stats = new Stats();
      KillData = new KillData();
    }
  }
}