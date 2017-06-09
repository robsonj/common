using JetBrains.Annotations;
using Nuke.Common.Tools;
using Nuke.Core;
using Nuke.Core.Execution;
using Nuke.Core.Tooling;
using Nuke.Core.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Nuke.Common.Tools.GitVersion
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [IconClass("podium")]
    public static partial class GitVersionTasks
    {
        static partial void PreProcess (GitVersionSettings gitVersionSettings);
        static partial void PostProcess (GitVersionSettings gitVersionSettings);
        /// <summary>
        /// <p>GitVersion is a tool to help you achieve Semantic Versioning on your project.</p>
        /// <p>For more details, visit the <a href="http://gitversion.readthedocs.io/en/stable/">official website</a>.</p>
        /// </summary>
        public static void GitVersion (Configure<GitVersionSettings> gitVersionSettingsConfigure = null, ProcessSettings processSettings = null)
        {
            gitVersionSettingsConfigure = gitVersionSettingsConfigure ?? (x => x);
            var gitVersionSettings = new GitVersionSettings();
            gitVersionSettings = gitVersionSettingsConfigure(gitVersionSettings);
            PreProcess(gitVersionSettings);
            var process = ProcessManager.Instance.StartProcess(gitVersionSettings, processSettings);
            process.AssertZeroExitCode();
            PostProcess(gitVersionSettings);
        }
    }
    /// <summary>
    /// <p>GitVersion is a tool to help you achieve Semantic Versioning on your project.</p>
    /// <p>For more details, visit the <a href="http://gitversion.readthedocs.io/en/stable/">official website</a>.</p>
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class GitVersionSettings : ToolSettings
    {
        /// <inheritdoc />
        public override string ToolPath => base.ToolPath ?? NuGetPackageResolver.GetToolPath($"GitVersion.CommandLine", $"GitVersion.exe");
        public virtual bool UpdateAssemblyInfo { get; internal set; }
        protected override Arguments GetArgumentsInternal()
        {
            return base.GetArgumentsInternal()
              .Add("/updateassemblyinfo", UpdateAssemblyInfo);
        }
    }
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class GitVersionSettingsExtensions
    {
        [Pure]
        public static GitVersionSettings SetUpdateAssemblyInfo(this GitVersionSettings gitVersionSettings, bool updateAssemblyInfo)
        {
            gitVersionSettings = gitVersionSettings.NewInstance();
            gitVersionSettings.UpdateAssemblyInfo = updateAssemblyInfo;
            return gitVersionSettings;
        }
        [Pure]
        public static GitVersionSettings EnableUpdateAssemblyInfo(this GitVersionSettings gitVersionSettings)
        {
            gitVersionSettings = gitVersionSettings.NewInstance();
            gitVersionSettings.UpdateAssemblyInfo = true;
            return gitVersionSettings;
        }
        [Pure]
        public static GitVersionSettings DisableUpdateAssemblyInfo(this GitVersionSettings gitVersionSettings)
        {
            gitVersionSettings = gitVersionSettings.NewInstance();
            gitVersionSettings.UpdateAssemblyInfo = false;
            return gitVersionSettings;
        }
        [Pure]
        public static GitVersionSettings ToggleUpdateAssemblyInfo(this GitVersionSettings gitVersionSettings)
        {
            gitVersionSettings = gitVersionSettings.NewInstance();
            gitVersionSettings.UpdateAssemblyInfo = !gitVersionSettings.UpdateAssemblyInfo;
            return gitVersionSettings;
        }
    }
}