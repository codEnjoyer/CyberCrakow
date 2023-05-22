using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooting_range
{
    public class TargetManager : MonoBehaviour
    {
        [SerializeField] private List<Target> _targets;

        private void Start()
        {
            StartCoroutine(HittingAllTargets());
        }


        private IEnumerator HittingAllTargets()
        {
            yield return new WaitForSeconds(3);
            foreach (var target in _targets)
            {
                target.GetHit();
                yield return new WaitForSeconds(1);
            }
        }
    }
}