using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooting_range
{
    public class PolygonTargetManager : MonoBehaviour
    {
        [SerializeField] private List<PolygonTarget> _targets;
        [SerializeField] private bool _hitAllTargetsAtStart = true;
        [SerializeField] private float _secondsToWaitBeforeHit = 3f;
        [SerializeField] private float _secondsBetweenHits = 1f;
        [SerializeField] private float _secondsToWaitBeforeRecover = 5f;
        [SerializeField] private float _secondsBetweenRecovers = .5f;
        private void Start()
        {
            if (_hitAllTargetsAtStart)
                StartCoroutine(HitAllTargets());
        }


        private IEnumerator HitAllTargets()
        {
            yield return new WaitForSeconds(_secondsToWaitBeforeHit);
            foreach (var target in _targets)
            {
                target.GetHit();
                yield return new WaitForSeconds(_secondsBetweenHits);
            }
            StartCoroutine(RecoverAllTargets());
        }
        
        private IEnumerator RecoverAllTargets()
        {
            yield return new WaitForSeconds(_secondsToWaitBeforeRecover);
            foreach (var target in _targets)
            {
                target.Recover();
                yield return new WaitForSeconds(_secondsBetweenRecovers);
            }
        }
    }
}