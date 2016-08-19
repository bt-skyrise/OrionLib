var target = Argument("target", "Default");
var slnPath = "../Source/Orion.Lib.sln";
var nugerPackagesOutputPath = "../Nuget";

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore(slnPath);
});

Task("Clean")
    .Does(() =>
{
    CleanDirectories("../Source/**/bin");
    CleanDirectories("../Source/**/obj");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    MSBuild(slnPath, settings => settings.SetConfiguration("Release"));
});


Task("Create-NuGet-Package")
    .IsDependentOn("Build")
    .Does(() =>
{       
    CreateDirectory(nugerPackagesOutputPath);
    
    NuGetPack("../Source/OrionLib/OrionLib.csproj", new NuGetPackSettings 
    {
        OutputDirectory = nugerPackagesOutputPath,
        Version = EnvironmentVariable("APPVEYOR_BUILD_VERSION"),
        IncludeReferencedProjects = true,
        ArgumentCustomization = args => args.Append("-Prop Configuration=Release")
    });    
});

Task("Push-NuGet-Package")
    .IsDependentOn("Create-NuGet-Package")
    .Does(() =>
{
    var allNugetPackages = GetFiles(nugerPackagesOutputPath + "/*.nupkg");

    foreach(var nugetPackage in allNugetPackages)
    {
        NuGetPush(nugetPackage, new NuGetPushSettings
        {
            Source = "https://nuget.org/",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        });
    } 
});


Task("Default")
	.IsDependentOn("Push-NuGet-Package")
    .Does(() =>
{
    Information("Orion.Lib building finished.");
});

RunTarget(target);