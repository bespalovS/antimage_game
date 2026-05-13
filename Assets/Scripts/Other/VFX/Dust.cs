using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField] private GameObject dustPrefab;
    [SerializeField] private Player player;

    public void SpawnDust()
    {
        Vector2 dir = player.GetMovementVector();

        Vector3 offset = -dir.normalized;
        Vector3 spawnPos = player.transform.position + offset;

        GameObject dust = Instantiate(dustPrefab, spawnPos, Quaternion.identity);

        Animator anim = dust.GetComponent<Animator>();

        int direction = GetDirection(dir);
        anim.SetInteger("Direction", direction);
        anim.SetTrigger("PlayDust");

        Destroy(dust, 1f);
    }

    int GetDirection(Vector2 move)
    {
        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            return move.x > 0 ? 3 : 2;
        else
            return move.y > 0 ? 0 : 1;
    }

}
