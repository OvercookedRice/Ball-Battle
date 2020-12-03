public class Constants
{
    public const float ATTACKER__ENERGY_REGENERATION_RATE = 0.5f;
    public const float ATTACKER__SPAWN_TIME = 0.5f;
    public const float ATTACKER__SQUARE_DISTANCE_TO_CAPTURE_BALL = 2f;
    public const float ATTACKER__INACTIVATE_WAIT_TIME = 2.5f;
    public const float ATTACKER__NORMAL_SPEED_MULTIPLIER = 1.5f;
    public const float ATTACKER__CARRY_SPEED_MULTIPLIER = 0.75f;

    public const float DEFENDER__ENERGY_REGENERATION_RATE = 0.5f;
    public const float DEFENDER__SPAWN_TIME = 0.5f;
    public const float DEFENDER__SQUARE_DISTANCE_TO_CATCH = 1f;
    public const float DEFENDER__INACTIVATE_WAIT_TIME = 4f;
    public const float DEFENDER__NORMAL_SPEED_MULTIPLIER = 1f;
    public const float DEFENDER__RETURN_TO_ORIGINAL_POSITION_SPEED_MULTIPLIER = 2f;
    public const float DEFENDER__DETECTION_RANGE = 0.35f;

    public const float BALL__SPEED = 1.5f;

    public const int ATTACKER__ENERGY_COST_TO_SPAWN = 2;
    public const int DEFENDER__ENERGY_COST_TO_SPAWN = 3;
    public const int MATCHES = 5;
}

public enum Faction
{
    Player, Opponent
}

public enum MatchScenario
{
    PlayerWon, OpponentWon, Draw
}