using System;
using System.Linq;
using UnityEngine;

namespace Tridi.AI
{
    [Serializable]
    public class VisionSense
    {
        [SerializeField] private Transform center;
        [SerializeField] private float distance;
        [SerializeField] private string ignoreTag;
        [SerializeField] private LayerMask layers;
        
        public Transform Center => center;
        public float Distance => distance; 
        
        public T See<T>(string tag, LayerMask searchLayer) where T : MonoBehaviour
        {
            var hits = Physics2D.CircleCastAll(center.position, distance, Vector2.right, 0.01f, searchLayer);

            return (from hit in hits.Where(hit => hit.collider.gameObject.CompareTag(tag))
                let comp = hit.collider.GetComponent<T>()
                where comp
                let check = Physics2D.LinecastAll(center.position, hit.point, layers)
                let obstacle = check.FirstOrDefault(x => !x.collider.gameObject.CompareTag(ignoreTag))
                where obstacle.collider == hit.collider
                select comp).FirstOrDefault();
        }
    }
}