using UnityEngine;

/// <summary>
/// Keeps the AudioListener on the player. The camera follows from about 11 metres away, and a listener that far
/// back would make every sound seem far away, so the camera's own listener was removed and this one sits at the player.
/// </summary>
public class ListenerOnPlayer : MonoBehaviour
{
    Transform target;

    void Start()
    {
        var player = FindAnyObjectByType<PlayerController>();
        if (player != null) target = player.transform;
    }

    void LateUpdate()
    {
        if (target != null) transform.position = target.position;
    }
}
