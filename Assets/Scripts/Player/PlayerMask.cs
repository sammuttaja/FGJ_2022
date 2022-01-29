namespace FGJ_2022.Player
{
    /// <summary>
    /// Custom script for defining
    /// player mask's power.
    /// </summary>
    internal sealed class PlayerMask
    {
        internal float _maskPower;
        internal float _minMaskPower;
        internal float _maxMaskPower;

        // Constructs PlayerMask object.
        public PlayerMask(int max)
        {
            _minMaskPower = 0;

            // Determine the max mask power.
            _maxMaskPower = (max > 0f) 
                ? _maxMaskPower = max 
                : _maxMaskPower = 1f;

            _maskPower = _maxMaskPower;
        }
    }
}