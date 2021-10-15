#nullable enable
using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoSchedule.Core.Test
{
    static class ProjectSourcePath
    {
        const string MyRelativePath = nameof(ProjectSourcePath) + ".cs";
        static string? _lazyValue;

        public static string Value
        {
            get
            {
                _lazyValue ??= CalculatePath();
                return _lazyValue;
            }
        }

        static string GetSourceFilePathName([CallerFilePath] string? callerFilePath = null)
            => callerFilePath ?? "";

        static string CalculatePath()
        {
            string pathName = GetSourceFilePathName();
            Assert.IsTrue(pathName.EndsWith(MyRelativePath, StringComparison.Ordinal));
            return pathName[..^MyRelativePath.Length];
        }
    }
}
