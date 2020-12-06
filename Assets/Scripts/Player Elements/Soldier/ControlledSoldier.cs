using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControlledSoldier : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject indicator = null;
    [SerializeField] private GameObject highlighter = null;

    [SerializeField] private Joystick joystick;
    [SerializeField] private GameField game_field;
    private bool got_ball = false;
    void Start()
    {
        indicator.SetActive(false);
        highlighter.SetActive(false);

        game_field = FindObjectOfType<GameField>();

#if UNITY_ANDROID || UNITY_IOS
        joystick = FindObjectOfType<Joystick>();
#endif
        got_ball = false;
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#elif UNITY_ANDROID || UNITY_IOS
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
#endif
        if (direction == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            indicator?.SetActive(false);
        }
        else
        {
            transform.forward = direction.normalized;

            rb.velocity = direction.normalized * Constants.ATTACKER__NORMAL_SPEED_MULTIPLIER * game_field.GetScale();
            indicator?.SetActive(true);
        }

        if (!got_ball)
        {
            Ball b = GameManager.GetInstance().GetBall();
            if ((transform.position - b.transform.position).sqrMagnitude <= Constants.ATTACKER__SQUARE_DISTANCE_TO_CAPTURE_BALL * game_field.GetScale())
            {
                b.Attach(this);
                highlighter?.SetActive(true);

                got_ball = true;
            }
        }

    }
}
