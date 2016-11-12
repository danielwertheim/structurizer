#tool "nuget:?package=NUnit.ConsoleRunner"

#load "./buildconfig.cake"

var config = BuildConfig.Create(Context, BuildSystem);
var slns = config.SrcDir + "*.sln";

Task("Default")
    .IsDependentOn("InitOutDir")
    .IsDependentOn("NuGet-Restore")
    .IsDependentOn("AssemblyVersion")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTests");

Task("CI")
    .IsDependentOn("Default")
    .IsDependentOn("NuGet-Pack");
  
Task("InitOutDir")
    .Does(() => {
        EnsureDirectoryExists(config.OutDir);
        CleanDirectory(config.OutDir);
    });

Task("NuGet-Restore")
    .Does(() => NuGetRestore(GetFiles(slns)));

Task("AssemblyVersion").Does(() => {
    var file = config.SrcDir + "GlobalAssemblyVersion.cs";
    var info = ParseAssemblyInfo(file);
    CreateAssemblyInfo(file, new AssemblyInfoSettings {
        Version = config.BuildVersion,
        InformationalVersion = config.SemVer
    });
});
    
Task("Build").Does(() => {
    foreach(var sln in GetFiles(slns)) {
        MSBuild(sln, new MSBuildSettings {
            Verbosity = Verbosity.Minimal,
            ToolVersion = MSBuildToolVersion.VS2015,
            Configuration = config.BuildProfile,
            PlatformTarget = PlatformTarget.MSIL
        }.WithTarget("Rebuild"));
    }
});

Task("UnitTests").Does(() =>
{
    NUnit3(config.SrcDir + "**/*.UnitTests/bin/" + config.BuildProfile + "/*.UnitTests.dll", new NUnit3Settings {
        NoResults = true,
        NoHeader = true,
        TeamCity = config.IsTeamCityBuild
    });
});

Task("NuGet-Pack").Does(() => {
    NuGetPack(GetFiles(config.SrcDir + "*.nuspec"), new NuGetPackSettings {
        Version = config.SemVer,
        BasePath = config.SrcDir,
        OutputDirectory = config.OutDir,
        Properties = new Dictionary<string, string>
        {
            {"Configuration", config.BuildProfile}
        }
    });
});

RunTarget(config.Target);