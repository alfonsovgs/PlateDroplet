using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace PlateDroplet.Fixtures
{
    public class LoadDataAttribute : DataAttribute
    {
        private readonly int _threshold;
        private readonly int _ruleGroup;
        private const string FileName = "PlateDropletInfo.json";
        private const string Section = "PlateDropletInfo";
        private const string Child = "DropletInfo";

        public LoadDataAttribute(object o, int threshold, int ruleGroup)
        {
            _threshold = threshold;
            _ruleGroup = ruleGroup;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if(testMethod == null) throw new ArgumentNullException(nameof(testMethod));

            var path = Path.IsPathRooted(FileName)
                ? FileName
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), FileName);

            if(!File.Exists(path))
            {
                throw new ArgumentException($"File not found: {path}");
            }

            if(string.IsNullOrEmpty(Section))
            {
                throw new ArgumentException($"Section not found: {Section}");
            }

            var fileData = File.ReadAllText(FileName);
            var allData = JObject.Parse(fileData);
            var data = allData[Section]?[Child];

            return new List<object[]>
            {
                new[]
                {
                    data.ToObject(testMethod.GetParameters().First().ParameterType),
                    _threshold,
                    _ruleGroup,
                }
            };
        }
    }
}
