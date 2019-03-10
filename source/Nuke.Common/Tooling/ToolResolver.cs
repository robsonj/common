// Copyright 2019 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Nuke.Common.Tooling
{
    [PublicAPI]
    public static class ToolResolver
    {
        public static Tool GetPackageTool(string packageId, string packageExecutable, string framework)
        {
            var toolPath = ToolPathResolver.GetPackageExecutable(packageId, packageExecutable, framework);
            return new ToolExecutor(toolPath).Execute;
        }

        [CanBeNull]
        public static Tool TryGetEnvironmentTool(string name)
        {
            var toolPath = ToolPathResolver.TryGetEnvironmentExecutable($"{name.ToUpperInvariant()}_EXE");
            if (toolPath == null)
                return null;

            return new ToolExecutor(toolPath).Execute;
        }

        public static Tool GetLocalTool(string relativePath)
        {
            var toolPath = Path.Combine(NukeBuild.RootDirectory, relativePath);
            ControlFlow.Assert(File.Exists(toolPath), $"File.Exists({toolPath})");
            return new ToolExecutor(toolPath).Execute;
        }

        public static Tool GetPathTool(string name)
        {
            var toolPath = ToolPathResolver.GetPathExecutable(name);
            return new ToolExecutor(toolPath).Execute;
        }
    }
}
