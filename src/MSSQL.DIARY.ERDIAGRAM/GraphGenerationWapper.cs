using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.DIARY.ERDIAGRAM
{
   public class GraphGenerationWapper: IGraphGeneration
    {
        private const string ProcessFolder = "GraphViz";

        private const string ConfigFile = "config6";

        private readonly IGetStartProcessQuery startProcessQuery;

        private readonly IGetProcessStartInfoQuery getProcessStartInfoQuery;

        private readonly IRegisterLayoutPluginCommand registerLayoutPlugincommand;

        private Enums.RenderingEngine renderingEngine;

        private string graphvizPath;

        public string GraphvizPath
        {
            get
            {
                return graphvizPath ?? (AssemblyDirectory + "/GraphViz");
            }
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    string text = value.Replace("\\", "/");
                    graphvizPath = (text.EndsWith("/") ? text.Substring(0, text.LastIndexOf('/')) : text);
                }
                else
                {
                    graphvizPath = null;
                }
            }
        }

        public Enums.RenderingEngine RenderingEngine
        {
            get
            {
                return renderingEngine;
            }
            set
            {
                renderingEngine = value;
            }
        }

        private string ConfigLocation => GraphvizPath + "/config6";

        private bool ConfigExists => File.Exists(ConfigLocation);

        private static string AssemblyDirectory
        {
            get
            {
                UriBuilder uriBuilder = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                string text = Uri.UnescapeDataString(uriBuilder.Path);
                return text.Substring(0, text.LastIndexOf('/'));
            }
        }

        private string FilePath => GraphvizPath + "/" + GetRenderingEngine(renderingEngine) + ".exe";
        public GraphGenerationWapper() 
        {
        }

        public void SetGraphPath(string istrgraphvizPath)
        {
            graphvizPath = istrgraphvizPath;
        }
        public GraphGenerationWapper(
            IGetStartProcessQuery startProcessQuery, 
            IGetProcessStartInfoQuery getProcessStartInfoQuery, 
            IRegisterLayoutPluginCommand registerLayoutPlugincommand,
            string istrgraphvizPath)
        {
            this.startProcessQuery = startProcessQuery;
            this.getProcessStartInfoQuery = getProcessStartInfoQuery;
            this.registerLayoutPlugincommand = registerLayoutPlugincommand;
             graphvizPath = istrgraphvizPath;
        }

        public byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType)
        {
            if (!ConfigExists)
            {
                registerLayoutPlugincommand.Invoke(FilePath, RenderingEngine);
            }
            string returnType2 = GetReturnType(returnType);
            ProcessStartInfo processStartInfo = GetProcessStartInfo(returnType2);
            using (Process process = startProcessQuery.Invoke(processStartInfo))
            {
                process.BeginErrorReadLine();
                using (StreamWriter streamWriter = process.StandardInput)
                {
                    streamWriter.WriteLine(dotFile);
                }
                using (StreamReader streamReader = process.StandardOutput)
                {
                    Stream baseStream = streamReader.BaseStream;
                    return ReadFully(baseStream);
                }
            }
        }

        private ProcessStartInfo GetProcessStartInfo(string returnType)
        {
            return getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
            {
                FileName = FilePath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Arguments = "-v -o -T" + returnType,
                CreateNoWindow = true
            });
        }

        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                input.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private string GetReturnType(Enums.GraphReturnType returnType)
        {
            Dictionary<Enums.GraphReturnType, string> dictionary = new Dictionary<Enums.GraphReturnType, string>();
            dictionary.Add(Enums.GraphReturnType.Png, "png");
            dictionary.Add(Enums.GraphReturnType.Jpg, "jpg");
            dictionary.Add(Enums.GraphReturnType.Pdf, "pdf");
            dictionary.Add(Enums.GraphReturnType.Plain, "plain");
            dictionary.Add(Enums.GraphReturnType.PlainExt, "plain-ext");
            dictionary.Add(Enums.GraphReturnType.Svg, "svg");
            Dictionary<Enums.GraphReturnType, string> dictionary2 = dictionary;
            return dictionary2[returnType];
        }

        private string GetRenderingEngine(Enums.RenderingEngine renderingType)
        {
            Dictionary<Enums.RenderingEngine, string> dictionary = new Dictionary<Enums.RenderingEngine, string>();
            dictionary.Add(Enums.RenderingEngine.Dot, "dot");
            dictionary.Add(Enums.RenderingEngine.Neato, "neato");
            dictionary.Add(Enums.RenderingEngine.Twopi, "twopi");
            dictionary.Add(Enums.RenderingEngine.Circo, "circo");
            dictionary.Add(Enums.RenderingEngine.Fdp, "fdp");
            dictionary.Add(Enums.RenderingEngine.Sfdp, "sfdp");
            dictionary.Add(Enums.RenderingEngine.Patchwork, "patchwork");
            dictionary.Add(Enums.RenderingEngine.Osage, "osage");
            Dictionary<Enums.RenderingEngine, string> dictionary2 = dictionary;
            return dictionary2[renderingType];
        }

    }
}
