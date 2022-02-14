using System;
using UnityEngine;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Experimental.Rendering;

[ExecuteInEditMode]
public class Portal : MonoBehaviour
{
    public Camera portalCamera;
    public GameObject bubble;
    public GameObject radiousObj;
    public float offset = 0.1f;

    public float _radius;
    private void Start()
    {
    }

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= UpdateCamera;
    }

    void UpdateCamera(ScriptableRenderContext src, Camera camera)
    {
        if ((camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView) &&
            camera.tag != "Portal Camera")
        {
            portalCamera.projectionMatrix = camera.projectionMatrix; // Match matrices

            var relativePosition = camera.transform.position;
            Vector3 offsetPos = relativePosition - bubble.transform.position;
            portalCamera.transform.position = bubble.transform.position + Vector3.ClampMagnitude(offsetPos, Vector3.Distance(bubble.transform.position, radiousObj.transform.position));            
            
            var relativeRotation = transform.InverseTransformDirection(camera.transform.forward);
            relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
            portalCamera.transform.forward = bubble.transform.TransformDirection(relativeRotation);
        }
    }

    // Calculates reflection matrix around the given plane
    private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
    {
        reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
        reflectionMat.m01 = (-2F * plane[0] * plane[1]);
        reflectionMat.m02 = (-2F * plane[0] * plane[2]);
        reflectionMat.m03 = (-2F * plane[3] * plane[0]);

        reflectionMat.m10 = (-2F * plane[1] * plane[0]);
        reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
        reflectionMat.m12 = (-2F * plane[1] * plane[2]);
        reflectionMat.m13 = (-2F * plane[3] * plane[1]);

        reflectionMat.m20 = (-2F * plane[2] * plane[0]);
        reflectionMat.m21 = (-2F * plane[2] * plane[1]);
        reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
        reflectionMat.m23 = (-2F * plane[3] * plane[2]);

        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

        if (otherRigidbody)
        {
            if (otherRigidbody.tag == "Player")
            {
            }


            // otherRigidbody.transform.position = pairPortal.transform.position + (pairPortal.transform.forward * offset);
            // otherRigidbody.transform.rotation = pairPortal.transform.rotation;
            //
            // otherRigidbody.velocity = otherRigidbody.velocity.magnitude * pairPortal.transform.forward;
        }
    }
}