using UnityEngine;

public class SticktoPlayer : MonoBehaviour
{
    public string pickupType;
    public float smoothSpeed;
    Transform targetPos;

    // Update is called once per frame
    void Update()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            switch (pickupType)
            {
                case "heal":
                    targetPos = GameObject.Find("HealSpawn").transform;
                    break;
                case "life":
                    targetPos = GameObject.Find("LifeSpawn").transform;
                    break;
            }
            
            transform.position = targetPos.position;
        }
    }
}
