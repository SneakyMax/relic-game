using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

namespace Assets.Scripts
{
    [UnityComponent]
    public class DynamicCamera : MonoBehaviour
    {
        [Header("Zoom Control")]
        [AssignedInUnity]
        public float MaxZoom;

        [AssignedInUnity]
        public float ZoomDistanceFactor;

        [AssignedInUnity]
        public float ZoomPushBack;

        [AssignedInUnity]
        public float AdditionalYDistanceModifier = 1;


        [AssignedInUnity, Space(10), Header("Adjusting Speed")]
        public float ZoomAdjustmentSpeed = 1f;

        [AssignedInUnity]
        public float MoveAdjustmentSpeed = 1f;

        private PlayerSpawner playerSpawner;
        private RelicSpawner relicSpawner;
        private new Camera camera;

        private float startOrthographicScale;
        private Vector3 startPosition;
        private float heightOffset;

        private Vector3 targetPosition;
        private float targetOrthographicSize;

        [Space(10), Header("Options")]
        public bool Enabled;

        [UnityMessage]
        public void Awake()
        {
            var controllersAndGui = GameObject.Find("ControllersAndGui");

            playerSpawner = controllersAndGui.GetComponentInChildren<PlayerSpawner>();
            relicSpawner = controllersAndGui.GetComponentInChildren<RelicSpawner>();
            camera = GetComponentInChildren<Camera>();
        }

        [UnityMessage]
        public void Start()
        {
            if(GameStateController.Instance.HasOption("DynamicCamera"))
                Enabled = GameStateController.Instance.GetOption<bool>("DynamicCamera");

            startOrthographicScale = camera.GetComponent<CameraFit>().GetOrthographicSizee();
            startPosition = transform.position;
            heightOffset = transform.position.y;
        }

        [UnityMessage]
        public void Update()
        {
            if (Enabled == false)
                return;

            UpdateTargets();

            MoveToTargets();
        }

        private void MoveToTargets()
        {
            DoSmallZoom();
            DoSmallMove();
        }

        private void DoSmallZoom()
        {
            var zoomAdjustConstantSpeed = ZoomAdjustmentSpeed * Time.deltaTime;
            Func<float, float> clamp = x => Mathf.Clamp(x, -zoomAdjustConstantSpeed, zoomAdjustConstantSpeed);

            var zoomDiff = camera.orthographicSize - targetOrthographicSize;
            camera.orthographicSize -= clamp(zoomDiff);
        }

        private void DoSmallMove()
        {
            var positionAdjustConstantSpeed = MoveAdjustmentSpeed * Time.deltaTime;
            Func<float, float> clamp = x => Mathf.Clamp(x, -positionAdjustConstantSpeed, positionAdjustConstantSpeed);

            var positionDiff = transform.position - targetPosition;
            var adjustment = new Vector3(clamp(positionDiff.x), clamp(positionDiff.y), clamp(positionDiff.z));

            transform.position -= adjustment;
        }

        private void UpdateTargets()
        {
            var spawnedPlayers = GetPointsOfInterest();

            if (spawnedPlayers.Count == 0)
            {
                targetPosition = startPosition;
                targetOrthographicSize = startOrthographicScale;
                return;
            }

            var centroid = GetCentroid(spawnedPlayers);
            var zoomAmount = GetZoomAmount(spawnedPlayers);

            targetOrthographicSize = startOrthographicScale / zoomAmount;
            targetPosition = new Vector3(centroid.x, centroid.y + heightOffset, startPosition.z);
        }

        private float GetZoomAmount(IList<Vector3> pointsOfInterest)
        {
            var maxDistance = 0f;
            foreach (var checkPosition in pointsOfInterest)
            {
                foreach (var otherPosition in pointsOfInterest)
                {
                    if (checkPosition == otherPosition)
                        continue;
                    
                    var x = otherPosition.x - checkPosition.x;
                    var y = (otherPosition.y - checkPosition.y) * camera.aspect * AdditionalYDistanceModifier;

                    var distance = Mathf.Sqrt(x * x + y * y);
                    if (distance > maxDistance)
                        maxDistance = distance;
                }
            }

            if (Math.Abs(maxDistance) < 0.001f)
                return MaxZoom;

            var zoomAmount = ZoomDistanceFactor / maxDistance + ZoomPushBack;
            zoomAmount = Math.Min(zoomAmount, MaxZoom);

            return Math.Max(1f, zoomAmount);
        }

        private IList<Vector3> GetPointsOfInterest()
        {
            var pointsOfInterest = new List<Vector3>();

            foreach (var player in playerSpawner.Players)
            {
                if (player.PlayerInstance != null)
                    pointsOfInterest.Add(player.PlayerInstance.transform.position);
            }

            if (relicSpawner.CurrentRelic != null)
            {
                pointsOfInterest.Add(relicSpawner.CurrentRelic.transform.position);
            }

            return pointsOfInterest;
        }

        private Vector3 GetCentroid(IList<Vector3> pointsOfInterest)
        {
            var sum = new Vector3();

            foreach (var position in pointsOfInterest)
                sum += position;

            return sum / pointsOfInterest.Count;
        }
    }
}