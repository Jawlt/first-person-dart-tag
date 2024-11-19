using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isTagged = false;

    public void Tag()
    {
        if (!isTagged)
        {
            isTagged = true;
            // Perform any actions you want when the enemy is tagged
            Debug.Log("Enemy is now tagged!");
            // Update any UI or effects as needed
        }
    }
}
