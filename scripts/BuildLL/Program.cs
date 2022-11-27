﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Bullseye.Targets;
using static BuildLL.Runtime;

namespace BuildLL
{
    class Program
    {
        private static string versionSetting = "git";
        private static string prefix = "/usr/local/";
        private static int parallel = -1;
        private static string glslangValidatorPath = null;
        private static bool buildDebug = false;
        public static void Options()
        {
            StringArg("--assemblyversion", x => versionSetting = x, "Set generated version");
            StringArg("--prefix", x => prefix = x, "Set cmake install prefix");
            IntArg("-j|--jobs", x => parallel = x, "Parallelism for native build step");
            StringArg("--glslangValidator", x => glslangValidatorPath = x);
            FlagArg("--debug", () => buildDebug = true, "Build natives with debug info");
        }
        
        static readonly string[] sdkProjects = {
            "src/lancer/lancer.csproj",
            "src/LLServer/LLServer.csproj",
            "src/thorn2lua/thorn2lua.csproj",
            "src/Editor/InterfaceEdit/InterfaceEdit.csproj",
            "src/Editor/LancerEdit/LancerEdit.csproj",
            "src/Editor/SystemViewer/SystemViewer.csproj",
            "src/Editor/ThnPlayer/ThnPlayer.csproj",
            "src/Editor/lleditscript/lleditscript.csproj",
            "src/Launcher/Launcher.csproj"
        };

        static readonly string[] engineProjects = {
            "src/lancer/lancer.csproj",
            "src/Launcher/Launcher.csproj",
            "src/LLServer/LLServer.csproj"
        };

        static void Clean(string rid)
        {
            Dotnet.Clean("LibreLancer.sln");
            RmDir("./obj/projs-" + rid);
            RmDir("./obj/projs-sdk-" + rid);
            RmDir("./bin/librelancer-" + rid);
            RmDir("./bin/librelancer-sdk-" + rid);
        }

       static  List<string> publishedProjects = new List<string>();

        static void FullBuild(string rid, bool sdk)
        {
            var projs = sdk ? sdkProjects : engineProjects;
            var objDir = "./obj/projs-";
            var binDir = sdk ? "./bin/librelancer-sdk-" : "./bin/librelancer-";
            var outdir = binDir + rid;
            foreach (var proj in projs)
            {
                var name = Path.GetFileName(proj);
                if (!publishedProjects.Contains(rid + ":" + proj)) {
                    CustomPublish.PatchedPublish(proj, objDir + rid + "/" + name, rid);
                    publishedProjects.Add(rid + ":" + proj);
                }
            }
            CustomPublish.Merge(objDir + rid, binDir + rid, rid,
                projs.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray());
            CopyFile("Credits.txt", outdir);
            if (IsWindows) {
                CopyFile("deps/openal-soft-license.txt", outdir);
                CopyFile("deps/openal-soft-sourceurl.txt", outdir);
            }
            CopyFile("LICENSE", outdir);
        }

        static string GetLinuxRid()
        {
            var uname = Bash("uname -m", false).Trim().ToLowerInvariant();
            if(uname.StartsWith("aarch64"))
                return "linux-arm64";
            if(uname.StartsWith("armv"))
                return "linux-arm";
            if(uname.StartsWith("x86_64"))
                return "linux-x64";
            return "linux-x86";
        }

        private static string VersionString;
        public static void Targets()
        {
            if(parallel > 0) Dotnet.CPUCount = parallel;
            /* webhook things */
            Target("default", DependsOn("BuildAll"));
            Target("BuildAll", DependsOn("BuildEngine", "BuildSdk"));
            
            Target("GenerateVersion", () =>
            {
                var version = versionSetting;
                if (version == "git")
                {
                    var lastSha = Git.ShaTip(".");
                    version = string.Format("{0}-git ({1})", lastSha.Substring(0, 7),
                        DateTime.Now.ToString("yyyyMMdd"));
                }
                string commonVersion =
                    $"<!-- This file is AutoGenerated -->\n<Project><PropertyGroup><InformationalVersion>{version}</InformationalVersion></PropertyGroup></Project>";
                Console.WriteLine($"Version: {version}");
                VersionString = version;
                if (!File.Exists("./src/CommonVersion.props") ||
                    File.ReadAllText("./src/CommonVersion.props") != commonVersion)
                {
                    File.WriteAllText("./src/CommonVersion.props", commonVersion);
                    Console.WriteLine("Updated version file ./src/CommonVersion.props");
                }
            });

            static string GetFileArgs(string dir, string glob)
            {
                return string.Join(" ", Directory.GetFiles(dir, glob).Select(x => Quote(x)));
            }
            Target("BuildShaders", () =>
            {
                string glv = "";
                if(glslangValidatorPath != null) glv = $"-g \"{glslangValidatorPath}\"";
                var args =  $"-b -t ShaderVariables -c ShaderVariables.Compile -x ShaderVariables.Log -n LibreLancer.Shaders -o ./src/LibreLancer/Shaders {glv} {GetFileArgs("./shaders/","*.glsl")}";
                
                Dotnet.Run("./shaders/ShaderProcessor/ShaderProcessor.csproj", args);
            });
            
            Target("BuildNatives", () =>
            {
                if (buildDebug) Console.WriteLine("Building natives with debug info");
                Directory.CreateDirectory("obj");
                Directory.CreateDirectory("bin/natives/x86");
                Directory.CreateDirectory("bin/natives/x64");
                string config = buildDebug ? "RelWithDebInfo" : "MinSizeRel";
                if (IsWindows)
                {
                    Directory.CreateDirectory("obj/x86");
                    Directory.CreateDirectory("obj/x64");
                    CopyDirContents("./deps/x64/", "./bin/natives/x64", false, "*.dll");
                    CopyDirContents("./deps/x86/", "./bin/natives/x86", false, "*.dll");
                    //build 32-bit
                    CMake.Run(".", new CMakeSettings() {
                        OutputPath = "obj/x86",
                        Generator = "Visual Studio 17 2022",
                        Platform = "Win32",
                        BuildType = config
                    });
                    MSBuild.Run("./obj/x86/librelancernatives.sln", $"/m /p:Configuration={config}", VSVersion.VS2022, MSBuildPlatform.x86);
                    CopyDirContents("./obj/x86/binaries/", "./bin/natives/x86", false, "*.dll");
                    if(buildDebug) CopyDirContents("./obj/x86/binaries/", "./bin/natives/x86", false, "*.pdb");
                    //build 64-bit
                    CMake.Run(".", new CMakeSettings() {
                        OutputPath = "obj/x64",
                        Generator = "Visual Studio 17 2022",
                        Platform = "x64",
                        BuildType = config
                    });
                    MSBuild.Run("./obj/x64/librelancernatives.sln", $"/m /p:Configuration={config}", VSVersion.VS2022, MSBuildPlatform.x64);
                    CopyDirContents("./obj/x64/binaries/", "./bin/natives/x64", false, "*.dll");
                    if (buildDebug) CopyDirContents("./obj/x64/binaries/", "./bin/natives/x64", false, "*.pdb");

                }
                else
                {
                    CMake.Run(".", new CMakeSettings()
                    {
                        OutputPath = "obj",
                        Options = new[] { "-DCMAKE_INSTALL_PREFIX=" + prefix },
                        BuildType = config
                    });
                    string args = "";
                    if (parallel > 0) args = "-j" + parallel;
                    RunCommand("make", args, "obj");
                    CopyDirContents("obj/binaries/", "./bin/natives/");
                }
            });

            Target("Clean", () =>
            {
                if (IsWindows)
                {
                    Clean("win7-x86");
                    Clean("win7-x64");
                }
                else
                    Clean(GetLinuxRid());
            });

            Target("Restore", () =>
            {
                Dotnet.Restore("LibreLancer.sln");
            });
            
            Target("BuildEngine", DependsOn("GenerateVersion", "BuildNatives", "Restore"), () =>
            {
                if(IsWindows) {
                    FullBuild("win7-x86", false);
                    FullBuild("win7-x64", false);
                } else
                    FullBuild(GetLinuxRid(), false);
            });
            Target("BuildDocumentation", DependsOn("GenerateVersion"), () =>
            {
                if (IsWindows)
                {
                    DocumentationBuilder.BuildDocs("./docs/", "./bin/librelancer-sdk-win7-x86/lib/Docs/", VersionString);
                    DocumentationBuilder.Copy("./bin/librelancer-sdk-win7-x86/lib/Docs/", "./bin/librelancer-sdk-win7-x64/lib/Docs/");
                }
                else
                {
                    DocumentationBuilder.BuildDocs("./docs/", $"./bin/librelancer-sdk-{GetLinuxRid()}/lib/Docs/", VersionString);
                }

            });
            Target("BuildSdk", DependsOn("GenerateVersion", "BuildDocumentation", "BuildNatives", "Restore"), () =>
            {
                if(IsWindows) {
                    FullBuild("win7-x86", true);
                    FullBuild("win7-x64", true);
                } else
                    FullBuild(GetLinuxRid(), true);
            });

            static void TarDirectory(string file, string dir)
            {
                Bash($"tar -I 'gzip -9' -cf {Quote(file)} -C {Quote(dir)} .", true);
            }

            Target("LinuxDaily", DependsOn("BuildEngine", "BuildSdk"), () =>
            {
                RmDir("packaging/packages/a");
                RmDir("packaging/packages/b");
                Directory.CreateDirectory("packaging/packages/a");
                Directory.CreateDirectory("packaging/packages/b");
                var lastCommit = Git.ShaTip(".");
                Console.WriteLine("Compressing");
                //Engine
                var name = "librelancer-" + lastCommit.Substring(0,7) + "-ubuntu-amd64";
                CopyDirContents("bin/librelancer-" + GetLinuxRid(), "packaging/packages/a/" + name, true);
                TarDirectory("packaging/packages/librelancer-daily-ubuntu-amd64.tar.gz", "packaging/packages/a");
                RmDir("packaging/packages/a");
                //Sdk
                name = "librelancer-sdk-" + lastCommit.Substring(0,7) + "-ubuntu-amd64";
                CopyDirContents("bin/librelancer-sdk-" + GetLinuxRid(), "packaging/packages/b/" + name, true);
                TarDirectory("packaging/packages/librelancer-sdk-daily-ubuntu-amd64.tar.gz", "packaging/packages/b");
                RmDir("packaging/packages/b");
                //Timestamp
                var unixTime = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                File.WriteAllText("packaging/packages/timestamp", unixTime.ToString());
            });
        }
    }
}
