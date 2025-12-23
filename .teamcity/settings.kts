import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildSteps.Qodana
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetPublish
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetTest
import jetbrains.buildServer.configs.kotlin.buildSteps.qodana

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

    buildType(Build)
}

object Build : BuildType({
    name = "Build"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        qodana {
            id = "Qodana"
            linter = dotNet {
                version = Qodana.DotNetVersion.LATEST
            }
            inspectionProfile = default()
            cloudToken = "credentialsJSON:cdec7c6d-0d4f-423d-910f-301ce7e7f3ce"
        }
        dotnetPublish {
            id = "dotnet"
            enabled = false
            projects = "src/Toolbox/Toolbox.csproj"
            sdk = "9"
        }
        dotnetTest {
            id = "dotnet_1"
            enabled = false
            projects = "tests/Toolbox.Tests/Toolbox.Tests.csproj"
            sdk = "9"
        }
    }

    features {
        perfmon {
        }
    }

    requirements {
        exists("container.engine")
        contains("teamcity.agent.hostname", ".build.ferociousbyte.dev")
    }
})
