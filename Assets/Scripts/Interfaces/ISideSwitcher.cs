public interface ISideSwitcher
{
    /// <summary>
    /// <para>Switch the object's data and behaviour based on its faction of this match.</para>
    /// <para>Only called once on new match start.</para>
    /// <para><paramref name="player"/>: Parent</para>
    /// </summary>
    /// <param name="player"></param>
    void Switch(Player player);

    /// <summary>
    /// Get this SideSwitcher's parent.
    /// </summary>
    /// <returns>Player instance corresponding to this element's parent</returns>
    Player GetPlayer();
}
