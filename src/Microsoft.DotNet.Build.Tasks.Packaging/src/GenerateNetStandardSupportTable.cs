// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GenerateNetStandardSupportTable : PackagingTask
    {
        [Required]
        public ITaskItem[] Reports
        {
            get;
            set;
        }

        [Required]
        public string DocFilePath
        {
            get;
            set;
        }


        public override bool Execute()
        {
            if (Reports == null || Reports.Length == 0)
            {
                Log.LogError("Reports argument must be specified");
                return false;
            }

            if (String.IsNullOrEmpty(DocFilePath))
            {
                Log.LogError("DocFilePath argument must be specified");
                return false;
            }

            string docDir = Path.GetDirectoryName(DocFilePath);
            if (!Directory.Exists(docDir))
            {
                Directory.CreateDirectory(docDir);
            }

            HashSet<NuGetFramework> knownGenerations = new HashSet<NuGetFramework>();
            foreach (var report in Reports.Select(r => r.GetMetadata("FullPath")))
            {
                SupportRow row = new SupportRow();
                row.Name = Path.GetFileNameWithoutExtension(report);
                row.SuportedFramework = new Dictionary<NuGetFramework, Version>();

                using (var file = File.OpenText(report))
                using (var reader = new JsonTextReader(file))
                {
                    var doc = JObject.Load(reader);
                    var versions = doc["versions"];

                    foreach(var property in versions)
                    {
                    }

                }

            }

            return !Log.HasLoggedErrors;
        }

        private struct SupportRow
        {
            public string Name;
            public Dictionary<NuGetFramework, Version> SuportedFramework;
        }
    }
}
