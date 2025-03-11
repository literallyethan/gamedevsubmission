using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    // when collided, activate/deactivate all objects referenced
    public GameObject[] activatables;
    bool state = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Button Hit!");
        foreach (GameObject item in activatables)
        {
            item.SetActive(state = !state);
        }
    }
}
