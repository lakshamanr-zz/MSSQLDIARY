using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;


namespace MSSQL.DIARY.ERDIAGRAM
{
    class Program
    {
        static void Main(string[] args)
        { 

            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery); 
            var wrapper = new GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);

            byte[] output = wrapper.GenerateGraph("", Enums.GraphReturnType.Svg);
            File.WriteAllBytes("Foo.svg", output.ToArray());
        }
    }
}
