using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;

    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        Manager.GameOver();
    }
}
