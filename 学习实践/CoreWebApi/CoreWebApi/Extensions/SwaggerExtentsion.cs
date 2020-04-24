using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Extensions
{
    public class ApplyTagDescriptions : IDocumentFilter
    {
        public void Apply(OpenApiDocument apiDoc, DocumentFilterContext context)
        {
            // 反射加载并获取所有ApiController类型列表
            List<string> strApiTypes = new List<string>();
            Assembly ass = Assembly.GetExecutingAssembly();
            IEnumerable<Type> apiTypes = ass.GetTypes().Where(p => p.GetCustomAttribute<ApiControllerAttribute>() != null);
            foreach (Type t in apiTypes) strApiTypes.Add(t.FullName);

            List<OpenApiTag> tags = new List<OpenApiTag>();

            // 读取xml文件中的注释信息
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            XDocument doc = XDocument.Load(xmlPath);

            // 遍历并判断是否为ApiController
            var members = doc.Element("doc").Element("members").Elements("member");
            foreach (var m in members)
            {
                var name = m.Attribute("name").Value;
                if (!name.StartsWith("T:")) continue;
                if (!strApiTypes.Contains(name.Substring(2))) continue;

                string apiName = name.Substring(name.LastIndexOf(".") + 1).Replace("Controller", "");
                string apiDesc = m.Element("summary").Value.Trim();
                tags.Add(new OpenApiTag { Name = apiName, Description = apiDesc });
            }

            if (tags.Count > 0) apiDoc.Tags = tags;
        }
    }
}
