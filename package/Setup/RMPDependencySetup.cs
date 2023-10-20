using System.Collections.Generic;
using Foundry.Core.Editor;
using Foundry.Core.Setup;
using UnityEditor;
using UnityEngine;

namespace Foundry.Avatar.ReadyPlayerMe.Setup
{
    public class RMPDependencySetup: IModuleSetupTasks
    {
        bool packagesInstalled;

        public RMPDependencySetup()
        {
            List<string> requiredPackages = new(new[]
            {
                "com.readyplayerme.core",
                "com.readyplayerme.avatarloader"
            });
            packagesInstalled = PackageManagerUtil.ArePackagesInstalled(requiredPackages);
        }
        
        public IModuleSetupTasks.State GetTaskState()
        {
            return packagesInstalled ? IModuleSetupTasks.State.Completed : IModuleSetupTasks.State.UncompletedRequiredTasks;
        }

        public List<SetupTaskList> GetTasks()
        {
            if (packagesInstalled)
                return new();
            SetupTask addRPMTask = new SetupTask();
            addRPMTask.name = "Ready Player Me SDK";
            addRPMTask.action = new SetupAction
            {
                name = "Install",
                callback = InstallPackage
            };
            
            var installDependenciesTaskList = new SetupTaskList("Dependencies");
            installDependenciesTaskList.Add(addRPMTask);

            return new List<SetupTaskList>{ installDependenciesTaskList };
        }

        public string ModuleName()
        {
            return "Ready Player Me Avatars for Foundry";
        }

        public string ModuleSource()
        {
            return "com.cyberhub.foundry.avatar.readyplayerme";
        }

        static void InstallPackage()
        {
            Debug.Log("Installing ready player me.");
            PackageManagerUtil.AddPackage("com.atteneder.gltfast", "https://github.com/atteneder/glTFast.git#v5.0.0");
            PackageManagerUtil.AddPackage("com.readyplayerme.core", "https://github.com/readyplayerme/rpm-unity-sdk-core.git#v1.3.0");
            PackageManagerUtil.AddPackage("com.readyplayerme.avatarloader",
                "https://github.com/readyplayerme/rpm-unity-sdk-avatar-loader.git#v1.3.0");
            PackageManagerUtil.Apply();
        }
    }
}
