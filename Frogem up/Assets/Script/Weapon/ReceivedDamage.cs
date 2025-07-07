using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeCharacter { Player, Enemy }
public class ReceivedDamage : MonoBehaviour {

    [Header("Stats")]
    public int life = 10;
    public TypeCharacter canTakeDamageTo;

    [Header("Manager Damage")]
    public bool canPauseDamage = false;
    public bool canTakeDamage = true;
    public float delayToTakeDamage;

    [Header("Visual Effects")]
    public Material damageMaterial;
    public float flashDuration = 0.5f;
    public float flashSpeed = 0.1f;

    private float delayBase;
    private Renderer[] _renderers;
    private Material[][] _originalMaterials;
    private bool _isFlashing = false;

    private void Awake()
    {
        delayBase = delayToTakeDamage;
        SetupRenderers();
    }
    private void SetupRenderers()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        _originalMaterials = new Material[_renderers.Length][];
        for (int i = 0; i < _renderers.Length; i++)
        {
            _originalMaterials[i] = new Material[_renderers[i].materials.Length];
            for (int j = 0; j < _renderers[i].materials.Length; j++)
            {
                _originalMaterials[i][j] = _renderers[i].materials[j];
            }
        }

        if (damageMaterial == null) { damageMaterial = CreateDefaultDamageMaterial(); }
    }
    private Material CreateDefaultDamageMaterial()
    {
        Material redMaterial = new Material(Shader.Find("Standard"));
        redMaterial.color = Color.red;
        redMaterial.SetFloat("_Metallic", 0f);
        redMaterial.SetFloat("_Glossiness", 0.5f);
        return redMaterial;
    }
    private void TakeDamage(int damage)
    {
        if (canPauseDamage) canTakeDamage = false;

        if (!_isFlashing) { StartCoroutine(FlashDamageEffect()); }

        if (damage < life) { life -= damage; }
        else
        {
            life = 0;
            Debug.Log(name + " ha muerto.");
            gameObject.SetActive(false);
        }
    }
    private IEnumerator FlashDamageEffect()
    {
        _isFlashing = true;
        float elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            SetMaterialsToFlash(true);
            yield return new WaitForSeconds(flashSpeed);

            SetMaterialsToFlash(false);
            yield return new WaitForSeconds(flashSpeed);

            elapsedTime += flashSpeed * 2f;
        }

        SetMaterialsToFlash(false);
        _isFlashing = false;
    }
    private void SetMaterialsToFlash(bool useDamageMaterial)
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] == null) continue;

            Material[] materials = new Material[_renderers[i].materials.Length];

            for (int j = 0; j < materials.Length; j++)
            {
                materials[j] = useDamageMaterial ? damageMaterial : _originalMaterials[i][j];
            }

            _renderers[i].materials = materials;
        }
    }
    private void Update()
    {
        if (canPauseDamage)
        {
            if (!canTakeDamage)
            {
                delayToTakeDamage -= Time.deltaTime;
                if (delayToTakeDamage <= 0)
                {
                    delayToTakeDamage = delayBase;
                    canTakeDamage = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>())
        {
            Dictionary<string, int> data = other.GetComponent<Bullet>().data;
            string tag = "";
            foreach (string key in data.Keys)
            {
                tag = key;
                break;
            }
            if (other.CompareTag("Bullet"))
            {
                if (tag != canTakeDamageTo.ToString()) return;
                if (canTakeDamage) { TakeDamage(data[tag]); }
            }
        }
    }
    public void ForceFlashEffect() { if (!_isFlashing) { StartCoroutine(FlashDamageEffect()); } }
    public bool IsFlashing() { return _isFlashing; }
    public void SetCustomFlashDuration(float duration) { flashDuration = duration; }
    public void SetCustomFlashSpeed(float speed) { flashSpeed = speed; }
    private void OnDestroy()
    {
        if (damageMaterial != null && damageMaterial.name == "Standard (Instance)")
        {
            DestroyImmediate(damageMaterial);
        }
    }
}