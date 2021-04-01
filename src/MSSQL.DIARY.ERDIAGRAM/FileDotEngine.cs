using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace MSSQL.DIARY.ERDIAGRAM
{
    public static class FileDotEngine
    {
        public static byte[] Svg (string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGenerationWrapper(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");
            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Svg);
        }
        public static byte[] Pdf(string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGenerationWrapper(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");
            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Pdf);
        }
        public static byte[] Png(string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGenerationWrapper(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");
            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Png);
        }
        public static byte[] Jpg(string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGenerationWrapper(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");
            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Jpg);
        }
    }
}
