using UnityEngine;

namespace WinterUniverse
{
    public class WorldCameraManager : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _followSpeed = 5f;

        public void OnLateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, WorldManager.StaticInstance.Player.transform.position + _offset, _followSpeed * Time.deltaTime);
        }
    }
}