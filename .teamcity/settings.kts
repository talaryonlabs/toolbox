import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.PullRequests
import jetbrains.buildServer.configs.kotlin.buildFeatures.commitStatusPublisher
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildFeatures.pullRequests
import jetbrains.buildServer.configs.kotlin.buildSteps.Qodana
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetBuild
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetNugetPush
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetPack
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetRestore
import jetbrains.buildServer.configs.kotlin.buildSteps.qodana
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.triggers.vcs
import jetbrains.buildServer.configs.kotlin.vcs.GitVcsRoot

/*
The settings script is an entry point for defining a TeamCity
project hierarchy. The script should contain a single call to the
project() function with a Project instance or an init function as
an argument.

VcsRoots, BuildTypes, Templates, and subprojects can be
registered inside the project using the vcsRoot(), buildType(),
template(), and subProject() methods respectively.

To debug settings scripts in command-line, run the

    mvnDebug org.jetbrains.teamcity:teamcity-configs-maven-plugin:generate

command and attach your debugger to the port 8000.

To debug in IntelliJ Idea, open the 'Maven Projects' tool window (View
-> Tool Windows -> Maven Projects), find the generate task node
(Plugins -> teamcity-configs -> teamcity-configs:generate), the
'Debug' option is available in the context menu for the task.
*/

version = "2025.11"

project {

    vcsRoot(HttpsGithubComTalaryonlabsStackmgrRefsHeadsMain)
    vcsRoot(HttpsGithubComTalaryonlabsWebkitRefsHeadsDev)

    buildType(Toolbox)
    buildType(CodeQuality)
    buildType(ToolboxHosting)
    buildType(ToolboxServices)
    buildTypesOrder = arrayListOf(CodeQuality, Toolbox, ToolboxHosting, ToolboxServices)
}

object CodeQuality : BuildType({
    name = "Code Quality"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        qodana {
            id = "Qodana"
            enabled = false
            linter = dotNet {
                version = Qodana.DotNetVersion.LATEST
            }
            inspectionProfile = default()
            additionalQodanaArguments = "--project-dir src/Toolbox"
            cloudToken = "credentialsJSON:cdec7c6d-0d4f-423d-910f-301ce7e7f3ce"
        }
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
        commitStatusPublisher {
            vcsRootExtId = "${DslContext.settingsRoot.id}"
            publisher = github {
                githubUrl = "https://api.github.com"
                authType = vcsRoot()
            }
        }
        pullRequests {
            vcsRootExtId = "${DslContext.settingsRoot.id}"
            provider = github {
                authType = vcsRoot()
                filterAuthorRole = PullRequests.GitHubRoleFilter.MEMBER
            }
        }
    }

    requirements {
        contains("teamcity.agent.name", "build-ferociousbyte-dev")
    }
})

object Toolbox : BuildType({
    name = "Toolbox"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        script {
            name = "Get Version Number"
            id = "Get_Version_Number"
            workingDir = "src/Toolbox"
            scriptContent = """
                export version="${'$'}(cat Toolbox.csproj | grep -Eo '<Version>[0-9.\-]+</Version>' | grep -Eo '[0-9.\-]+')"
                echo "##teamcity[buildNumber '${'$'}version']"
            """.trimIndent()
        }
        dotnetRestore {
            name = "Restore Packages"
            id = "Restore_Packages"
            projects = "src/Toolbox/Toolbox.csproj"
            sources = "https://nuget.pkg.talaryon.dev/v3/index.json"
        }
        dotnetBuild {
            name = "Build"
            id = "dotnet"
            projects = "src/Toolbox/Toolbox.csproj"
        }
        dotnetPack {
            name = "Pack NuGet Package"
            id = "Pack_NuGet_Package"
            projects = "src/Toolbox/Toolbox.csproj"
            outputDir = "publish"
        }
        dotnetNugetPush {
            name = "Push NuGet Package"
            id = "Push_NuGet_Package"
            packages = "publish/*Toolbox*.nupkg"
            serverUrl = "https://nuget.pkg.talaryon.dev/v3/index.json"
            apiKey = "credentialsJSON:56baad1f-80c9-4e5e-8ad3-d684ac95dfb8"
        }
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
        commitStatusPublisher {
            enabled = false
            vcsRootExtId = "${HttpsGithubComTalaryonlabsStackmgrRefsHeadsMain.id}"
            publisher = github {
                githubUrl = "https://api.github.com"
                authType = personalToken {
                    token = "credentialsJSON:f395c44f-e583-4547-91ab-c3d8e4d49d97"
                }
            }
        }
    }

    requirements {
        contains("teamcity.agent.name", "build-ferociousbyte-dev")
    }
})

object ToolboxHosting : BuildType({
    name = "Toolbox.Hosting"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        script {
            name = "Get Version Number"
            id = "Get_Version_Number"
            workingDir = "src/Toolbox.Hosting"
            scriptContent = """
                export version="${'$'}(cat Toolbox.Hosting.csproj | grep -Eo '<Version>[0-9.\-]+</Version>' | grep -Eo '[0-9.\-]+')"
                echo "##teamcity[buildNumber '${'$'}version']"
            """.trimIndent()
        }
        dotnetRestore {
            name = "Restore Packages"
            id = "Restore_Packages"
            projects = "src/Toolbox.Hosting/Toolbox.Hosting.csproj"
            sources = "https://nuget.pkg.talaryon.dev/v3/index.json"
        }
        dotnetBuild {
            name = "Build"
            id = "dotnet"
            projects = "src/Toolbox.Hosting/Toolbox.Hosting.csproj"
        }
        dotnetPack {
            name = "Pack NuGet Package"
            id = "Pack_NuGet_Package"
            projects = "src/Toolbox.Hosting/Toolbox.Hosting.csproj"
            outputDir = "publish"
        }
        dotnetNugetPush {
            name = "Push NuGet Package"
            id = "Push_NuGet_Package"
            packages = "publish/*Toolbox.Hosting*.nupkg"
            serverUrl = "https://nuget.pkg.talaryon.dev/v3/index.json"
            apiKey = "credentialsJSON:56baad1f-80c9-4e5e-8ad3-d684ac95dfb8"
        }
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
        commitStatusPublisher {
            enabled = false
            vcsRootExtId = "${HttpsGithubComTalaryonlabsStackmgrRefsHeadsMain.id}"
            publisher = github {
                githubUrl = "https://api.github.com"
                authType = personalToken {
                    token = "credentialsJSON:f395c44f-e583-4547-91ab-c3d8e4d49d97"
                }
            }
        }
    }

    requirements {
        contains("teamcity.agent.name", "build-ferociousbyte-dev")
    }
})

object ToolboxServices : BuildType({
    name = "Toolbox.Services"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        script {
            name = "Get Version Number"
            id = "Get_Version_Number"
            workingDir = "src/Toolbox.Services"
            scriptContent = """
                export version="${'$'}(cat Toolbox.Services.csproj | grep -Eo '<Version>[0-9.\-]+</Version>' | grep -Eo '[0-9.\-]+')"
                echo "##teamcity[buildNumber '${'$'}version']"
            """.trimIndent()
        }
        dotnetRestore {
            name = "Restore Packages"
            id = "Restore_Packages"
            projects = "src/Toolbox.Hosting/Toolbox.Hosting.csproj"
            sources = "https://nuget.pkg.talaryon.dev/v3/index.json"
        }
        dotnetBuild {
            name = "Build"
            id = "dotnet"
            projects = "src/Toolbox.Hosting/Toolbox.Hosting.csproj"
        }
        dotnetPack {
            name = "Pack NuGet Package"
            id = "Pack_NuGet_Package"
            projects = "src/Toolbox.Hosting/Toolbox.Hosting.csproj"
            outputDir = "publish"
        }
        dotnetNugetPush {
            name = "Push NuGet Package"
            id = "Push_NuGet_Package"
            packages = "publish/*Toolbox.Hosting*.nupkg"
            serverUrl = "https://nuget.pkg.talaryon.dev/v3/index.json"
            apiKey = "credentialsJSON:56baad1f-80c9-4e5e-8ad3-d684ac95dfb8"
        }
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
        commitStatusPublisher {
            enabled = false
            vcsRootExtId = "${HttpsGithubComTalaryonlabsStackmgrRefsHeadsMain.id}"
            publisher = github {
                githubUrl = "https://api.github.com"
                authType = personalToken {
                    token = "credentialsJSON:f395c44f-e583-4547-91ab-c3d8e4d49d97"
                }
            }
        }
    }

    requirements {
        contains("teamcity.agent.name", "build-ferociousbyte-dev")
    }
})

object HttpsGithubComTalaryonlabsStackmgrRefsHeadsMain : GitVcsRoot({
    name = "https://github.com/talaryonlabs/stackmgr#refs/heads/main"
    url = "https://github.com/talaryonlabs/stackmgr"
    branch = "refs/heads/main"
    branchSpec = "refs/heads/*"
    authMethod = password {
        userName = "ferociousbyte"
        password = "credentialsJSON:d420966c-62b3-43f3-a5de-67e5abe6916a"
    }
})

object HttpsGithubComTalaryonlabsWebkitRefsHeadsDev : GitVcsRoot({
    name = "https://github.com/talaryonlabs/webkit#refs/heads/dev"
    url = "https://github.com/talaryonlabs/webkit"
    branch = "refs/heads/dev"
    branchSpec = "refs/heads/*"
    authMethod = password {
        userName = "ferociousbyte"
        password = "credentialsJSON:6a79183f-823b-402a-95f9-6dfe2623f133"
    }
})
