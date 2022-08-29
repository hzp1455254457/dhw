/*************************************************************************
 *  Copyright © 2017-2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  MouseTranslate.cs
 *  Description  :  Mouse pointer drag to translate gameobject.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  4/9/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.UCommon.Generic;
using UnityEngine;

namespace MGS.UCamera
{
    /// <summary>
    /// Mouse pointer drag to translate gameobject.
    /// </summary>
    [AddComponentMenu("MGS/UCamera/MouseTranslate")]
    public class MouseTranslate : MonoBehaviour
    {
        //这个变量用来记录单指双指的变换
        private bool m_IsSingleFinger;

        //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
        private Vector2 oldPosition1;
        private Vector2 oldPosition2;

        //摄像机距离
        public float distance = 10.0f;

        //缩放系数
        public float scaleFactor = 1f;

        public float maxDistance = 30f;
        public float minDistance = 5f;


        private Camera m_Camera;
        private Vector3 m_CameraOffset;

        private Vector3 currentVelocity = Vector3.zero;

        #region Field and Property
        /// <summary>
        /// Target camera for translate direction.
        /// </summary>
        [Tooltip("Target camera for translate direction.")]
        public Transform targetCamera;

        /// <summary>
        /// Settings of mouse button and pointer.
        /// </summary>
        [Tooltip("Settings of mouse button and pointer.")]
        public MouseSettings mouseSettings = new MouseSettings(0, 0.1f, 0);

        /// <summary>
        /// Settings of move area.
        /// </summary>
        [Tooltip("Settings of move area.")]
        public PlaneArea areaSettings;

        /// <summary>
        /// Damper for move.
        /// </summary>
        [Tooltip("Damper for move.")]
        [Range(0, 10)]
        public float damper = 1;

        /// <summary>
        /// Current offset base area center.
        /// </summary>
        public Vector3 CurrentOffset { protected set; get; }

        /// <summary>
        /// Target offset base area center.
        /// </summary>
        protected Vector3 targetOffset;
        #endregion

        #region Protected Method
        /// <summary>
        /// Component awake.
        /// </summary>
        protected virtual void Awake()
        {
           
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            m_Camera = this.GetComponent<Camera>();
            m_CameraOffset = m_Camera.transform.position;
            areaSettings = new PlaneArea(GameObject.Find("大场景").transform, UICameraControl.Size,0);
            CurrentOffset = targetOffset = transform.position - areaSettings.center.position;
        }

        /// <summary>
        /// Component update.
        /// </summary>
        void Update()
        {
            if (ClickManager.IsPointerOverUIObject()) return;
            TranslateByMouse();
            return;
            ////判断触摸数量为单点触摸
            //if (Input.touchCount == 1)
            //{
            //    TranslateByMouse();
            //}
            if (Input.touchCount > 1)
            {
                //当从单指触摸进入多指触摸的时候,记录一下触摸的位置
                //保证计算缩放都是从两指手指触碰开始的
                if (m_IsSingleFinger)
                {
                    oldPosition1 = Input.GetTouch(0).position;
                    oldPosition2 = Input.GetTouch(1).position;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    ScaleCamera();
                }

                m_IsSingleFinger = false;
            }
            else
            {
                TranslateByMouse();
            }
        }

        /// <summary>
        /// Translate this gameobject by mouse.
        /// </summary>
        protected void TranslateByMouse()
        {
            if (Input.GetMouseButton(mouseSettings.mouseButtonID))
            {
                //Mouse pointer.
                var mouseX = Input.GetAxis("Mouse X") * mouseSettings.pointerSensitivity;
                //var mouseY = Input.GetAxis("Mouse Y") * mouseSettings.pointerSensitivity;

                //Deal with offset base direction of target camera.
                targetOffset -= targetCamera.right * mouseX;
                //targetOffset -= Vector3.Cross(targetCamera.right, Vector3.up) * mouseY;

                //Range limit.
                targetOffset.x = Mathf.Clamp(targetOffset.x, -areaSettings.width, areaSettings.width);
                //targetOffset.z = Mathf.Clamp(targetOffset.z, -areaSettings.length, areaSettings.length);
            }

            //Lerp and update transform position.
            CurrentOffset = Vector3.Lerp(CurrentOffset, targetOffset, damper * Time.deltaTime);
            transform.position = areaSettings.center.position + CurrentOffset;
        }


        private void ScaleCamera()
        {
            //计算出当前两点触摸点的位置
            var tempPosition1 = Input.GetTouch(0).position;
            var tempPosition2 = Input.GetTouch(1).position;


            float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
            float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

            //计算上次和这次双指触摸之间的距离差距
            //然后去更改摄像机的距离
            distance -= (currentTouchDistance - lastTouchDistance) * scaleFactor * Time.deltaTime;


            //把距离限制住在min和max之间
            distance = Mathf.Clamp(distance, minDistance, maxDistance);


            //备份上一次触摸点的位置，用于对比
            oldPosition1 = tempPosition1;
            oldPosition2 = tempPosition2;
            DoScaleCamera();
        }

        private void DoScaleCamera()
        {
            var position = m_CameraOffset + m_Camera.transform.forward * -distance;
            //m_Camera.transform.position = position;

            var targetPos = new Vector3(position.x, m_Camera.transform.position.y, m_Camera.transform.position.z);
            m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, targetPos, ref currentVelocity, 0.01f);
        }

        #endregion
    }
}