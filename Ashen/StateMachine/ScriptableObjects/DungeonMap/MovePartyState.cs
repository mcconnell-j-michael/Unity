using System.Collections;
using UnityEngine;

public class MovePartyState : I_GameState
{
    private Transform player;
    private Vector3 destination;
    private bool smoothTransition;
    private float transitionSpeed;
    
    public void Initialize(Transform player, bool smoothTransition, float transitionSpeed, Vector2Int destination)
    {
        this.player = player;
        this.smoothTransition = smoothTransition;
        this.transitionSpeed = transitionSpeed;
        this.destination = new Vector3(destination.x, player.position.y, destination.y);
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        while ((Vector3.Distance(player.position, destination) >= 0.05f))
        {
            if (!smoothTransition)
            {
                player.position = destination;
            }
            else
            {
                player.position = Vector3.MoveTowards(player.position, destination, Time.deltaTime * transitionSpeed);
            }
            yield return null;
        }
        player.position = destination;
    }
}
