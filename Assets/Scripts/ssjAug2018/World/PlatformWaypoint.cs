using UnityEngine;

public class PlatformWaypoint : MonoBehaviour {

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
