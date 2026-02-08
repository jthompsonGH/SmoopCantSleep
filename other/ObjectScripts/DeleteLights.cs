using UnityEngine;


public class DeleteLights : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("fancyLighting", 1) == 0)
        {
            GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        }

        this.enabled = false;
    }
}
