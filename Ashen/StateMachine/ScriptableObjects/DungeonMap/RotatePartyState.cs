using UnityEngine;
using System.Collections;

public class RotatePartyState : I_GameState
{
    private Transform player;
    private Vector3 rotation;

    private bool smoothTransition = true;
    private float transitionRotationSpeed = 500f;

    public void Initialize(Transform player, bool smoothTransition, float transitionRotationSpeed, RotationDirection rotation)
    {
        this.player = player;
        this.smoothTransition = smoothTransition;
        this.transitionRotationSpeed = transitionRotationSpeed;
        this.rotation = new Vector3(0, RotationDirectionFunctions.GetDegrees(rotation), 0);
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        while (Vector3.Distance(player.eulerAngles, rotation) >= 0.05f)
        {
            if (!smoothTransition)
            {
                player.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                player.rotation = Quaternion.RotateTowards(player.rotation, Quaternion.Euler(rotation), Time.deltaTime * transitionRotationSpeed);
            }
            yield return null;
        }
        player.rotation = Quaternion.Euler(rotation);
    }
}
