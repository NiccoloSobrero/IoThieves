namespace Art.Materials
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NeonLight : MonoBehaviour
    {
        [SerializeField] private float baseIntensity = 1.5f;
        [SerializeField] private float maxFlashIntensity = 4f; // picchi forti
        [SerializeField] private List<Renderer> renderers = new();
        [SerializeField] private Color color;

        private Coroutine _flickerCoroutine;

        void Start()
        {
            foreach (Renderer r in renderers)
            {
                r.material.EnableKeyword("_EMISSION");
            }
            _flickerCoroutine = StartCoroutine(ContinuousFlicker());
        }

        private IEnumerator ContinuousFlicker()
        {
            while (true)
            {
                // 70% casi: flicker normale (intensità variabile)
                float flickerInt = Random.Range(0.3f, baseIntensity);
                SetEmission(color * flickerInt);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.25f)); // on breve

                // 20% casi: flash ultra-forte
                if (Random.value < 0.2f)
                {
                    SetEmission(color * maxFlashIntensity);
                    yield return new WaitForSeconds(Random.Range(0.08f, 0.18f));
                }

                // 10% casi: spegnimento breve (blackout)
                if (Random.value < 0.1f)
                {
                    SetEmission(Color.black);
                    yield return new WaitForSeconds(Random.Range(0.03f, 0.1f));
                }

                // Nessuna pausa lunga: ciclo continuo!
            }
        }

        private void SetEmission(Color emittedColor)
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", emittedColor);
            }
        }

        void OnDestroy()
        {
            if (_flickerCoroutine != null)
                StopCoroutine(_flickerCoroutine);
        }
    }

}