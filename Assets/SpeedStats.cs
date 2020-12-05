using UnityEngine;
using UnityEngine.UI;

public class SpeedStats : MonoBehaviour
{
    public PlayerController player;
    public Text speedText;


    // Update is called once per frame
    void Update()
    {
        speedText.text = "Speed: " + player.getWalkSpeed();
        print("Speed: " + player.getWalkSpeed());
    }
}
