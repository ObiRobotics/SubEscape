using UnityEngine;

public class RewardBadges : MonoBehaviour
{
    public ExperimentState expState;
    public GameObject[] badges;
    public Light particleLight;
    //public ParticleSystem particleBadge;
    public AudioSource[] effectSound;
    public float effectDuration = 0.5f;
    public int[] badgeScores = new int[4] {500,1500,3000,6500};

    private bool[] activated = new bool[4] { false, false, false, false };

    private void Start()
    {
        int i = 0;
        foreach (GameObject badge in badges)
        {
            badge.SetActive(false);
            activated[i] = false;
            i++;
        }
        particleLight.enabled = false;
        //particleBadge.Stop();
        effectSound[0].Stop();
        effectSound[1].Stop();
    }

    private void Update()
    {
        if (expState.score >= badgeScores[0] & !activated[0]) // 500
        {
            ActivateEffects();
            Invoke("DeactivateEffect", effectDuration);
            badges[0].SetActive(true);
            activated[0] = true;
        }
        if (expState.score >= badgeScores[1] & !activated[1]) // 1500
        {
            ActivateEffects();
            Invoke("DeactivateEffect", effectDuration);
            badges[1].SetActive(true);
            activated[1] = true;
        }
        if (expState.score >= badgeScores[2] & !activated[2]) // 3000
        {
            ActivateEffects();
            Invoke("DeactivateEffect", effectDuration);
            badges[2].SetActive(true);
            activated[2] = true;
        }
        if (expState.score >= badgeScores[3] & !activated[3]) // 6500
        {
            ActivateEffects();
            Invoke("DeactivateEffect", effectDuration);
            badges[3].SetActive(true);
            activated[3] = true;
        }
    }

    private void ActivateEffects()
    {
        particleLight.enabled = true;
        //particleBadge.Play();
        effectSound[0].Play();
        effectSound[1].Play();
    }

    private void DeactivateEffect()
    {
        particleLight.enabled = false;
        //particleBadge.Stop();
        effectSound[0].Stop();
        effectSound[1].Stop();
    }
}