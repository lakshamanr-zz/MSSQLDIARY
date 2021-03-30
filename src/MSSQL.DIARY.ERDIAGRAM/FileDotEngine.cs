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
        public static byte[] SVG (string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var Genwapper = new GraphGenerationWapper(); 
            var wrapper = new GraphGenerationWapper(
                getStartProcessQuery,
                getProcessStartInfoQuery, 
                registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");

            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Svg);
        }
        public static byte[] PDF(string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var Genwapper = new GraphGenerationWapper();
            var wrapper = new GraphGenerationWapper(
                getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");

            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Pdf);
        }
        public static byte[] PNG(string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var Genwapper = new GraphGenerationWapper();
            var wrapper = new GraphGenerationWapper(
                getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");

            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Png);
        }
        public static byte[] JPG(string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var Genwapper = new GraphGenerationWapper();
            var wrapper = new GraphGenerationWapper(
                getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand, "C:\\Program Files (x86)\\Graphviz2.38\\bin");

            return wrapper.GenerateGraph(dot, Enums.GraphReturnType.Jpg);
        }
    }
}
