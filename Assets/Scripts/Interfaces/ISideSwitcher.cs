public interface ISideSwitcher
{
    /// <summary>
    /// <para>Switch the object's data and behaviour based on its faction of this match.</para>
    /// <para>Only called once on new match start.</para>
    /// <para><paramref name="faction"/>: Parent's Faction.</para>
    /// </summary>
    /// <param name="faction"></param>
    void Switch(Faction faction);

    /// <summary>
    /// Get this SideSwitcher's faction.
    /// </summary>
    /// <returns>A Faction value corresponding to this element's side</returns>
    Faction GetSide();
}
