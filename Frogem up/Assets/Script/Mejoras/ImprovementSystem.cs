using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ImprovementSystem : MonoBehaviour {

    public Improvement[] improvements;

    [Header("Items")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    [Space]
    public Image[] items;
    public Image selectItem;

    [Header("Weapon")]
    public GameObject sectorWeapon;
    public TextMeshProUGUI[] valueWeapon;
    public TextMeshProUGUI[] subWeapon;
    public Slider[] sliderWeapon;

    [Header("Stats")]
    public GameObject sectorStats;
    public TextMeshProUGUI[] valueStats;
    public TextMeshProUGUI[] subStats;
    public Slider[] sliderStats;

    private List<int> ints = new List<int>();
    private List<int> itemList = new List<int>();
    private int selectedValue = -1;

    private WaveSystem waveSystem;
    private WeaponObject weaponObj;
    private StatsManager statsManager;

    private void Awake()
    {
        waveSystem = GetComponentInParent<WaveSystem>();
        statsManager = FindObjectOfType<StatsManager>();
        weaponObj = FindObjectOfType<WeaponObject>();
    }
    private void OnEnable()
    {
        for(int i = 0; i < improvements.Length; i++) { ints.Add(i); }
        selectedValue = -1;
    }
    public void Roll()
    {
        itemList.Clear();
        itemList = CompleteItems();

        for(int i = 0; i < itemList.Count; i++)
        {
            items[i].sprite = improvements[itemList[i]].icon;
        }

        selectedValue = -1;

        Highlight(0);
    }
    public void Highlight(int pos)
    {
        if(selectedValue == pos)
        {
            SelectedOption(pos);
            return;
        }

        selectedValue = pos;

        nameText.text = improvements[itemList[pos]].nameItem;
        descText.text = improvements[itemList[pos]].descItem;

        if (improvements[itemList[pos]].isWeapon)
        {
            sectorWeapon.gameObject.SetActive(true);
            sectorStats.gameObject.SetActive(false);

            Weapon weapon = improvements[itemList[pos]].weaponView;

            valueWeapon[0].text = weapon.bulletSpeed.ToString();
            valueWeapon[1].text = weapon.dispersion.ToString();
            valueWeapon[2].text = weapon.delayShot.ToString();
            valueWeapon[3].text = weapon.bulletAmount.ToString();
            valueWeapon[4].text = weapon.bulletPerClic.ToString();
            valueWeapon[5].text = weapon.timeToReload.ToString();
            valueWeapon[6].text = weapon.damage.ToString();

            subWeapon[0].text = "Bullet Speed";
            subWeapon[1].text = "Dispersion";
            subWeapon[2].text = "Delay Shot";
            subWeapon[3].text = "Bullet Amount";
            subWeapon[4].text = "Bullets for Burst";
            subWeapon[5].text = "Time to Reload";
            subWeapon[6].text = "Damage";

            sliderWeapon[0].value = weapon.bulletSpeed;
            sliderWeapon[1].value = weapon.dispersion;
            sliderWeapon[2].value = weapon.delayShot;
            sliderWeapon[3].value = weapon.bulletAmount;
            sliderWeapon[4].value = weapon.bulletPerClic;
            sliderWeapon[5].value = weapon.timeToReload;
            sliderWeapon[6].value = weapon.damage;
        }
        else
        {
            sectorWeapon.gameObject.SetActive(false);
            sectorStats.gameObject.SetActive(true);

            Improvement item = improvements[itemList[pos]];

            valueStats[0].text = "+" + item.newValue.ToString();
            subStats[0].text = item.modifyStats.ToString();
            sliderStats[0].value = item.newValue;
        }
    }
    private List<int> CompleteItems()
    {
        List<int> baseList = ints;
        List<int> newItems = new List<int>();

        for(int i = 0; i < 3; i++)
        {
            int rnd = Random.Range(0, baseList.Count);
            newItems.Add(baseList[rnd]);
            baseList.RemoveAt(rnd);
        }

        return newItems;
    }
    private void SelectedOption(int pos)
    {
        if (improvements[itemList[pos]].isWeapon)
        {
            weaponObj.ChangeWeapon(improvements[itemList[pos]].weaponView);
        }
        else
        {
            switch (improvements[itemList[pos]].modifyStats)
            {
                case Stats.life:
                    statsManager.life += improvements[itemList[pos]].newValue;
                    break;
                case Stats.speed: 
                    statsManager.speed += improvements[itemList[pos]].newValue;
                    break;
                case Stats.shield:
                    statsManager.shield += improvements[itemList[pos]].newValue;
                    break;

            }
        }

        waveSystem.LaunchWave();
    }
}