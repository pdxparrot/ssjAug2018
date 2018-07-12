namespace pdxpartyparrot.Core.Util
{
    public static class UnitUtils
    {
        public static float MetersPerSecondToMilesPerHour(float mps)
        {
            return mps * 2.24f;
        }

        public static float MetersPerSecondToKilometersPerHour(float mps)
        {
            return mps * 3.6f;
        }
    }
}
