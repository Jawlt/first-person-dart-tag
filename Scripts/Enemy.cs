using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isTagged = false;

    public void Tag()
    {
        if (!isTagged)
        {
            isTagged = true;
            Debug.Log("Enemy is now tagged!");
        }
    }
}
