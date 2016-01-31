using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float Time;

        public void Start()
        {
            Destroy(gameObject, Time);
        }
    }
}