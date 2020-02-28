using Moonlight.Game.Entities;

namespace Moonlight.Game
{
    /// <summary>
    ///     Contains some method allowing to make unsafe action (some kind of cheat)
    /// </summary>
    public sealed class Unsafe
    {
        private readonly Character _character;

        internal Unsafe(Character character) => _character = character;

        /// <summary>
        ///     Activate/desactivate lever without delay
        ///     This method can be used even if your character is not close to lever
        ///     Please make sure GroundItem is a lever before using this method.
        /// </summary>
        /// <param name="lever">Lever to use</param>
        public void UseLeverWithoutDelay(GroundItem lever)
        {
            _character.Client.SendPacket($"#git^{lever.Id}");
        }
    }
}