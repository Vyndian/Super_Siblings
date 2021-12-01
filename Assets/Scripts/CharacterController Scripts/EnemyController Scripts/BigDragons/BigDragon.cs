using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class BigDragon : MonoBehaviour
{
    [SerializeField] private GameObject fireBreath;
    [SerializeField] private float fireBreathCooldown = 5.0f;
    private bool fireBreath_isCharging = false;

    private void Update()
    {
        if (fireBreath.activeSelf == false && !fireBreath_isCharging)
        {
            StartCoroutine(FireBreathDelay());
        }
    }

    IEnumerator FireBreathDelay()
    {
        fireBreath_isCharging = true;
        float timer = 0.0f;
        while (timer < fireBreathCooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        fireBreath.SetActive(true);
        fireBreath_isCharging = false;
    }
}
