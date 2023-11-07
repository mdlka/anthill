using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace YellowSquad.BuildCI
{
    public class Build
    {
        private static BuildOptions DevelopmentBuildOptions => 
            BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.EnableDeepProfilingSupport;

        [MenuItem("Build/Build WebGL Test Run")]
        public static void BuildWebGLNoCompress()
        {
            BuildWebGL(WebGLCompressionFormat.Disabled, BuildOptions.AutoRunPlayer);
        }

        [MenuItem("Build/Build WebGL Test Development Run")]
        public static void BuildWebGLNoCompressDevelopment()
        {
            BuildWebGL(WebGLCompressionFormat.Disabled, BuildOptions.AutoRunPlayer | DevelopmentBuildOptions);
        }

        [MenuItem("Build/Build WebGL Brotli")]
        public static void BuildWebGLBrotliCompress()
        {
            BuildWebGL(WebGLCompressionFormat.Brotli, BuildOptions.None);
        }

        private static void BuildWebGL(WebGLCompressionFormat compressionFormat, BuildOptions buildOptions)
        {
            var oldCompressionFormat = PlayerSettings.WebGL.compressionFormat;
            PlayerSettings.WebGL.compressionFormat = compressionFormat;
            
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Game/Scenes/Main.unity" },
                locationPathName = "WebGLBuild",
                target = BuildTarget.WebGL,
                options = buildOptions
            };
            
            ExecuteBuild(buildPlayerOptions);
            
            PlayerSettings.WebGL.compressionFormat = oldCompressionFormat;
        }
        
        private static void ExecuteBuild(BuildPlayerOptions options)
        {
            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log("Build succeeded: " + summary.totalSize / 1024f / 1024f + " mb" + "\nOutput path: " + summary.outputPath);
                    break;
                case BuildResult.Failed:
                    Debug.Log("Build failed");
                    break;
                case BuildResult.Unknown:
                    Debug.Log("Unknown");
                    break;
                case BuildResult.Cancelled:
                    Debug.Log("Build cancelled");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}