using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DamageColorBlender : MonoBehaviour
{
    public HasHitPoint m_HasHitPoint;
    public Material m_FlashMaterial;
    public float m_Duration = 0.25f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _originalMaterials;
    protected MaterialPropertyBlock[] _materialPropertyBlocks;
    private Coroutine _flashCoroutine;
    private bool _isFlashing = false;

    private void OnEnable()
    {
        m_HasHitPoint.OnTakeDamage += StartFlash;
    }

    private void OnDisable()
    {
        m_HasHitPoint.OnTakeDamage -= StartFlash;
        StopFlash();
    }

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _materialPropertyBlocks = new MaterialPropertyBlock[_spriteRenderers.Length];
        _originalMaterials = new Material[_spriteRenderers.Length];

        for (var i = 0; i < _spriteRenderers.Length; ++i)
        {
            _materialPropertyBlocks[i] = new MaterialPropertyBlock();
            _originalMaterials[i] = _spriteRenderers[i].sharedMaterial;
        }
    }

    private void StartFlash()
    {
        if (_isFlashing)
        {
            return;
        }
        _isFlashing = true;

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }
        if (gameObject.activeSelf)
        {
            _flashCoroutine = StartCoroutine(WhiteFlash());
        }
    }

    private void StopFlash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            SetSpriteMaterial(_originalMaterials);
            _flashCoroutine = null;
        }
        _isFlashing = false;
    }

    private IEnumerator WhiteFlash()
    {
        if (_spriteRenderers != null)
        {
            var elapsedTime = 0f;
            
            SetSpriteMaterial(m_FlashMaterial);
            while (elapsedTime < m_Duration)
            {
                var flashAmount = Mathf.Lerp(1f, 0f, elapsedTime / m_Duration);

                for (var i = 0; i < _spriteRenderers.Length; ++i)
                {
                    _spriteRenderers[i].GetPropertyBlock(_materialPropertyBlocks[i]);
                    _materialPropertyBlocks[i].SetFloat("_FlashAmount", flashAmount);
                    _spriteRenderers[i].SetPropertyBlock(_materialPropertyBlocks[i]);
                }
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            SetSpriteMaterial(_originalMaterials);
        }
        _flashCoroutine = null;
        _isFlashing = false;
    }

    private void SetSpriteMaterial(Material newMaterial)
    {
        for (var i = 0; i < _spriteRenderers.Length; ++i)
        {
            _spriteRenderers[i].material = newMaterial;
        }
    }

    private void SetSpriteMaterial(Material[] newMaterials)
    {
        for (var i = 0; i < _spriteRenderers.Length; ++i)
        {
            _spriteRenderers[i].material = newMaterials[i];
        }
    }
}
