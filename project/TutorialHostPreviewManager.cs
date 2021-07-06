using GameData;
using Gameplay.Accessory;
using UnityEngine;
using DynamicShadowProjector;

namespace UI.Views
{
    public class TutorialHostPreviewManager : MonoBehaviour
    {
        private GameObject _spawnedMember;

        [Header("References")] [SerializeField]
        private Camera _characterCamera;

        [SerializeField] private Transform _charParent;
        [SerializeField] private DrawTargetObject _drawTargetScript;

        [Space] [Header("Settings")]
        [SerializeField] private float _distanceFromCameraToCharacter;
        [SerializeField] private Vector3 _textureRenderViewPosition;
        [SerializeField] private Vector3 _cameraPositionOffset;
        [SerializeField] private Vector3 _cameraLookAtOffset;
        [SerializeField] private RuntimeAnimatorController _tutorialAnimController;
       
        public GameObject SpawnedMember => _spawnedMember;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize(BandMemberData bandMemberData)
        {
            transform.position = _textureRenderViewPosition;
            if (bandMemberData.CharacterPrefab == null)
            {
                return;
            }

            _spawnedMember = Instantiate(bandMemberData.CharacterPrefab, _charParent);
            _drawTargetScript.SetCommandBufferDirty();

            if (_spawnedMember == null)
            {
                return;
            }
            _spawnedMember.GetComponent<Animator>().runtimeAnimatorController = _tutorialAnimController as RuntimeAnimatorController;
            _spawnedMember.transform.localPosition = Vector3.zero;
            _spawnedMember.transform.eulerAngles = Vector3.zero;

            var accessoryController = _spawnedMember.GetComponent<BandMemberAccessoryController>();

            ChangerLayerAndLight();
        }

        private void ChangerLayerAndLight()
        {
            foreach (Transform obj in _spawnedMember.GetComponentsInChildren<Transform>())
            {
                obj.gameObject.layer = 18;

                Renderer renderer = gameObject.GetComponent<Renderer>();

                if (renderer != null)
                {
                    renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.CustomProvided;
                }
            }
        }
    }
}