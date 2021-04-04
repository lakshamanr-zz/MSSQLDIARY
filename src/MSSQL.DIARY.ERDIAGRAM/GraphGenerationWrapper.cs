using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MSSQL.DIARY.ERDIAGRAM
{
    public class GraphGenerationWrapper: IGraphGeneration
    {
        private readonly IGetStartProcessQuery startProcessQuery;

        private readonly IGetProcessStartInfoQuery getProcessStartInfoQuery;

        private readonly IRegisterLayoutPluginCommand registerLayoutPluginCommand;

        private Enums.RenderingEngine renderingEngine;

        private string _lstrGraphVizPath;

        public string IstrGraphVizPath
        {
            get => _lstrGraphVizPath ?? (IstrAssemblyDirectory + "/GraphViz");
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    string text = value.Replace("\\", "/");
                    _lstrGraphVizPath = (text.EndsWith("/") ? text.Substring(0, text.LastIndexOf('/')) : text);
                }
                else
                {
                    _lstrGraphVizPath = null;
                }
            }
        }

        public Enums.RenderingEngine RenderingEngine
        {
            get => renderingEngine;
            set => renderingEngine = value;
        }

        private string IstrConfigLocation => IstrGraphVizPath + "/config6";

        private bool IblnConfigExists => File.Exists(IstrConfigLocation);

        public static string IstrAssemblyDirectory
        {
            get
            {
                UriBuilder uriBuilder = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                string lstrtext = Uri.UnescapeDataString(uriBuilder.Path);
                return lstrtext.Substring(0, lstrtext.LastIndexOf('/'));
            }
        }

        private string LstrFilePath => IstrGraphVizPath + "/" + GetRenderingEngine(renderingEngine) + ".exe"; 

        public void SetGraphPath(string astrGraphVizPath)
        {
            _lstrGraphVizPath = astrGraphVizPath;
        }
        public GraphGenerationWrapper(IGetStartProcessQuery startProcessQuery, IGetProcessStartInfoQuery getProcessStartInfoQuery, IRegisterLayoutPluginCommand registerLayoutPluginCommand, string astrGraphVizPath)
        {
            this.startProcessQuery = startProcessQuery;
            this.getProcessStartInfoQuery = getProcessStartInfoQuery;
            this.registerLayoutPluginCommand = registerLayoutPluginCommand;
             _lstrGraphVizPath = astrGraphVizPath;
        }

        public byte[] GenerateGraph(string astrDotFile, Enums.GraphReturnType aenmReturnType)
        {
            if (!IblnConfigExists)
            {
                registerLayoutPluginCommand.Invoke(LstrFilePath, RenderingEngine);
            }
            string returnType2 = GetReturnType(aenmReturnType);
            ProcessStartInfo processStartInfo = GetProcessStartInfo(returnType2);
            using (Process process = startProcessQuery.Invoke(processStartInfo))
            {
                process.BeginErrorReadLine();
                using (StreamWriter streamWriter = process.StandardInput)
                {
                    streamWriter.WriteLine(astrDotFile);
                }
                using (StreamReader streamReader = process.StandardOutput)
                {
                    Stream baseStream = streamReader.BaseStream;
                    return ReadFully(baseStream);
                }
            }
        }

        private ProcessStartInfo GetProcessStartInfo(string astrReturnType)
        {
            return getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
            {
                FileName = LstrFilePath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Arguments = "-v -o -T" + astrReturnType,
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
            var dictionary = new Dictionary<Enums.GraphReturnType, string>
            {
                { Enums.GraphReturnType.Png, "png" },
                { Enums.GraphReturnType.Jpg, "jpg" },
                { Enums.GraphReturnType.Pdf, "pdf" },
                { Enums.GraphReturnType.Plain, "plain" },
                { Enums.GraphReturnType.PlainExt, "plain-ext" },
                { Enums.GraphReturnType.Svg, "svg" }
            };
            Dictionary<Enums.GraphReturnType, string> dictionary2 = dictionary;
            return dictionary2[returnType];
        }

        private string GetRenderingEngine(Enums.RenderingEngine renderingType)
        {
            Dictionary<Enums.RenderingEngine, string> dictionary = new Dictionary<Enums.RenderingEngine, string>
            {
                {Enums.RenderingEngine.Dot, "dot"},
                {Enums.RenderingEngine.Neato, "neato"},
                {Enums.RenderingEngine.Twopi, "twopi"},
                {Enums.RenderingEngine.Circo, "circo"},
                {Enums.RenderingEngine.Fdp, "fdp"},
                {Enums.RenderingEngine.Sfdp, "sfdp"},
                {Enums.RenderingEngine.Patchwork, "patchwork"},
                {Enums.RenderingEngine.Osage, "osage"}
            };
            Dictionary<Enums.RenderingEngine, string> dictionary2 = dictionary;
            return dictionary2[renderingType];
        }

    }
}
