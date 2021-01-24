using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private GameObject _health;

    public void SetHP(float hpNormalized)
    {
        _health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float currentHP = _health.transform.localScale.x;
        float changeAmount = currentHP - newHP;

        while(currentHP - newHP >Mathf.Epsilon)
        {
            currentHP -= changeAmount * Time.deltaTime;
            _health.transform.localScale = new Vector3(currentHP, 1f);
            yield return null;
        }
        _health.transform.localScale = new Vector3(newHP, 1f);

    }
}
