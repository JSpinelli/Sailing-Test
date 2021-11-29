#if CLAYXELS_URP

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using System.Collections.Generic;

#if UNITY_EDITOR 
using UnityEditor;
using System.Reflection;
#endif

using Clayxels;

public class MicroVoxelRenderPassURP : UnityEngine.Rendering.Universal.ScriptableRenderPass{
    public override void Execute(ScriptableRenderContext context, ref UnityEngine.Rendering.Universal.RenderingData renderingData){
        if(ClayContainer.globalDataNeedsInit){
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get();

        cmd.name = "clayxelsPrePass";

        this.ConfigureTarget(ClayContainer.getMicroVoxelRenderBuffers(), ClayContainer.getMicroVoxelDepthBuffer());
        cmd.ClearRenderTarget(true, true, Color.black, 1.0f);

        ClayContainer.drawMicroVoxelPrePass(cmd);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
        
    }
}

public class MicroVoxelRenderFeature : UnityEngine.Rendering.Universal.ScriptableRendererFeature{
    static public int dirtyAllLights = 0;

    MicroVoxelRenderPassURP m_ScriptablePass1 = null;
    
    public override void Create(){
        m_ScriptablePass1 = new MicroVoxelRenderPassURP();
        m_ScriptablePass1.renderPassEvent = UnityEngine.Rendering.Universal.RenderPassEvent.BeforeRenderingPrepasses;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(UnityEngine.Rendering.Universal.ScriptableRenderer renderer, ref UnityEngine.Rendering.Universal.RenderingData renderingData){
        if(ClayContainer.getNumMicrovoxelContainers() > 0){
            renderer.EnqueuePass(m_ScriptablePass1);

            #if UNITY_EDITOR
                // dirty all lights to re-establish proper render loop after this pass has executed
                if(MicroVoxelRenderFeature.dirtyAllLights > 0){
                    MicroVoxelRenderFeature.dirtyAllLights -= 1;

                    Light[] lights = UnityEngine.Object.FindObjectsOfType<Light>();
                    for(int i = 0; i < lights.Length; ++i){
                        lights[i].SetLightDirty();
                    }
                }
            #endif
        }
    }
}

#if UNITY_EDITOR // exclude from build

namespace Clayxels{
    [InitializeOnLoad]
    public class URPEditorInit{
        static URPEditorInit(){
            ClayContainer.renderPipelineInitCallback = URPEditorInit.init;
            init();
        }

        private static int GetDefaultRendererIndex(UniversalRenderPipelineAsset asset){
            return (int)typeof(UniversalRenderPipelineAsset).GetField("m_DefaultRendererIndex", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(asset);
        }

        public static ScriptableRendererData GetDefaultRenderer(){
            if (UniversalRenderPipeline.asset)
            {
                ScriptableRendererData[] rendererDataList = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset)
                        .GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance)
                        .GetValue(UniversalRenderPipeline.asset);
                int defaultRendererIndex = GetDefaultRendererIndex(UniversalRenderPipeline.asset);
         
                return rendererDataList[defaultRendererIndex];
            }
            else
            {
                Debug.LogError("Clayxels: No Universal Render Pipeline is currently active, please create your project from Unity Hub and select Universal when prompted.");
                return null;
            }
        }

        static void init(){
            UnityEngine.Rendering.Universal.ScriptableRendererData forwardPipeData = GetDefaultRenderer();

            List<UnityEngine.Rendering.Universal.ScriptableRendererFeature> passes = forwardPipeData.rendererFeatures;

            List<UnityEngine.Rendering.Universal.ScriptableRendererFeature> passesToRemove = new List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>();
            for(int i = 0; i < passes.Count; ++i){
                if(passes[i].GetType().Name.StartsWith("MicroVoxelRenderFeature")){
                    passesToRemove.Add(passes[i]);
                }
            }

            for(int i = 0; i < passesToRemove.Count; ++i){
                passes.Remove(passesToRemove[i]);
            }

            passesToRemove.Clear();

            string forwardPipeAssetPath = AssetDatabase.GetAssetPath(forwardPipeData);

            Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(forwardPipeAssetPath);
            for(int i = 0; i < subAssets.Length; ++i){
                if(subAssets[i] != null){
                    if(subAssets[i].GetType().Name.StartsWith("MicroVoxelRenderFeature")){
                        AssetDatabase.RemoveObjectFromAsset(subAssets[i]);
                    }
                }
            }

            forwardPipeData = GetDefaultRenderer();
            passes = forwardPipeData.rendererFeatures;
            
            MicroVoxelRenderFeature newRenderFeature = ScriptableObject.CreateInstance<MicroVoxelRenderFeature>();
            newRenderFeature.name = "MicroVoxelRenderFeature";
            AssetDatabase.AddObjectToAsset(newRenderFeature, forwardPipeData);

            // string guid;
            // long localId;
            // AssetDatabase.TryGetGUIDAndLocalFileIdentifier(newRenderFeature , out guid, out localId);

            // string renderFeatureAssetPath = AssetDatabase.GUIDToAssetPath(guid);
            
            // UnityEngine.Rendering.Universal.ScriptableRendererFeature renderFeatureAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.Universal.ScriptableRendererFeature>(renderFeatureAssetPath);

            passes.Add(newRenderFeature);

            // update the passes list with this hack
            MethodInfo dynMethod = forwardPipeData.GetType().GetMethod("OnValidate", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(forwardPipeData, new object[]{});

            // trick unity to re-render lights
            MicroVoxelRenderFeature.dirtyAllLights = 2;
        }
    }
}

#endif
#endif