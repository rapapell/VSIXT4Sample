using EnvDTE;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VSIXT4Sample
{
  public class CodeGenerator
  {
    public void RunTemplate(IServiceProvider a_xServiceProvider)
    {
      Project xProject = null;

      DTE xDTE = (DTE)a_xServiceProvider.GetService(typeof(DTE));
      ITextTemplating xT4 = (ITextTemplating)a_xServiceProvider.GetService(typeof(STextTemplating));
      string sTemplateFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "T4", "Template.tt");
      string sOutputTemplateFile = xT4.ProcessTemplate(sTemplateFilePath, File.ReadAllText(sTemplateFilePath));

      Array activeSolutionProjects = xDTE.ActiveSolutionProjects as Array;
      if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
      {
        xProject = activeSolutionProjects.GetValue(0) as Project;
      }

      if(Directory.Exists("GeneratedCode") == false)
      {
        Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(xProject.FullName), "GeneratedCode"));
      }

      string sInformationOutputFilePath = Path.Combine(Path.GetDirectoryName(xProject.FullName), "GeneratedCode", "Class.generated.cs");

      File.WriteAllText(sInformationOutputFilePath, sOutputTemplateFile);
    }
  }
}
