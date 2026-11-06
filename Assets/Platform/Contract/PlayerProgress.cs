using System;

namespace GamePlatform
{
    [Serializable]
    public sealed class PlayerProgress
    {
        public int coins;
        public int[] upgradeLevels = new int[6];
    }
}
