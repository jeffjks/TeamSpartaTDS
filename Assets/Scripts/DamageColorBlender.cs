using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColorBlender : MonoBehaviour
{
    public HasHitPoint m_HasHitPoint;
    public SpriteRenderer m_SpriteRenderer;
    public Material m_FlashMaterial;
    public float m_Duration = 0.25f;
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

    private Material _originalMaterial;

    private Coroutine _flashCoroutine;

    private void Start()
    {
        _originalMaterial = m_SpriteRenderer.sharedMaterial;
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
            _flashCoroutine = StartCoroutine(FlashRoutine());
        }
    }

    private void StopFlash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            m_SpriteRenderer.material = _originalMaterial;
            _flashCoroutine = null;
        }
        _isFlashing = false;
    }

    private IEnumerator FlashRoutine()
    {
        if (m_SpriteRenderer != null)
        {
            var elapsedTime = 0f;
            m_SpriteRenderer.material = m_FlashMaterial;
            while (elapsedTime < m_Duration)
            {
                var flashAmount = Mathf.Lerp(1f, 0f, elapsedTime / m_Duration);
                m_FlashMaterial.SetFloat("_FlashAmount", flashAmount);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            m_SpriteRenderer.material = _originalMaterial;
        }
        _flashCoroutine = null;
        _isFlashing = false;
        yield break;
    }
}
