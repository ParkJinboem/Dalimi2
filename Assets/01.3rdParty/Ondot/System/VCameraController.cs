using System;
using Cinemachine;
using OnDot.Util;
using UnityEngine;

namespace OnDot.System
{
    public class VCameraController : Singleton<VCameraController>
    {
        [SerializeField] public CinemachineVirtualCamera[] vCams = new CinemachineVirtualCamera[2];

        public CinemachineBrain cinemachineBrain;
        public PolygonCollider2D polygonCollider2D;

        private float initOrthographicSize;
        public float InitOrthographicSize
        {
            get { return initOrthographicSize; }
        }

        public float OrthographicSize
        {
            get
            {
                return cinemachineBrain.OutputCamera.orthographicSize;
            }
            set
            {
                CinemachineVirtualCamera cinemachineVirtualCamera = (CinemachineVirtualCamera)cinemachineBrain.ActiveVirtualCamera;
                cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(value, 9, Mathf.Min(CameraConfiner.x, CameraConfiner.y));
            }
        }

        public Vector2 CameraConfiner
        {
            get
            {
                Vector2 size = polygonCollider2D.bounds.size;
                return new Vector2(size.x / ((float)Screen.width / Screen.height) / 2f, size.y / 2f);
            }
        }

        private void Start()
        {
            initOrthographicSize = OrthographicSize;
        }

        /// <summary>
        /// 메인 카메라 활성화
        /// </summary>
        public void ActiveMainCamera()
        {
            for (int i = 0; i < vCams.Length; i++)
            {
                vCams[i].gameObject.SetActive(false);
            }
            vCams[0].gameObject.SetActive(true);
        }

        /// <summary>
        /// 특정 카메라 활성화
        /// </summary>
        /// <param name="vCamId"></param>
        /// <param name="follow"></param>
        public void ActiveCamera(int vCamId, Transform follow)
        {
            //if (vCams[vCamId].Follow == follow)
            //{
            //    vCams[vCamId].Follow = null;
            //    ActiveMainCamera();
            //    return;
            //}

            for (int i = 0; i < vCams.Length; i++)
            {
                vCams[i].gameObject.SetActive(false);
            }
            vCams[vCamId].Follow = follow;
            vCams[vCamId].gameObject.SetActive(true);
        }
    }
}