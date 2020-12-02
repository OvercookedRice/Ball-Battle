public class Constants
{
    public float ATTACKER__ENERGY_REGENERATION_RATE = 0.5f;
    public float ATTACKER__SPAWN_TIME = 0.5f;
    public float ATTACKER__INACTIVATE_WAIT_TIME = 2.5f;
    public float ATTACKER__NORMAL_SPEED_MULTIPLIER = 1.5f;
    public float ATTACKER__CARRY_SPEED_MULTIPLIER = 0.75f;

    public float DEFENDER__ENERGY_REGENERATION_RATE = 0.5f;
    public float DEFENDER__SPAWN_TIME = 0.5f;
    public float DEFENDER__INACTIVATE_WAIT_TIME = 4f;
    public float DEFENDER__NORMAL_SPEED_MULTIPLIER = 1f;
    public float DEFENDER__RETURN_TO_ORIGINAL_POSITION_SPEED_MULTIPLIER = 2f;
    public float DEFENDER__DETECTION_RANGE = 0.35f;

    public float BALL__SPEED = 1.5f;

    public int ATTACKER__ENERGY_COST_TO_SPAWN = 2;
    public int DEFENDER__ENERGY_COST_TO_SPAWN = 3;
}

public enum Faction
{
    Player, Opponent
}
