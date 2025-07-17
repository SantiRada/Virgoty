using UnityEngine;
using TMPro;

public enum Stats { life, speed, shield }
public class StatsManager : MonoBehaviour {

    public float life {  get; set; }
    public float maxLife { get; set; }
    public float shield {  get; set; }
    public float maxShield {  get; set; }
    public float speed {  get; set; }

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI shieldText;

    private void Update()
    {
        lifeText.text = life.ToString() + " / " + maxLife.ToString();
        shieldText.text = shield.ToString() + " / " + maxShield.ToString();
    }
}
