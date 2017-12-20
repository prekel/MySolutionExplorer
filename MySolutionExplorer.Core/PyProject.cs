// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer.Core
{
	public class PyProject : Project
	{
		protected override void FindProjectFiles()
		{
			//throw new NotImplementedException();
		}

		public override void CreateFiles()
		{
			CreateFiles(MyEnum.TemplatePyProj);

			//FindProjectFiles();

			//CreateProjects();

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplatePy);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			//LoadProjects();
			//ReformVSProjects();

			FindFiles();
			//FindProjectFiles();
		}

        /// <summary>
        /// Кандидат к удалению
        /// </summary>
		[Obsolete]
		public static void Create(Solution s, string task, string site, string number, DirectoryInfo dir)
		{
			var p = new PyProject
			{
				ParentSolution = s,
				TaskName = task,
				Site = site,
				Number = int.Parse(number),
				Lang = "py"
			};
			p.Path = dir + MyEnum.Slash + p.Name;
			p.CreateFiles();
			s.Add(p);
		}

		public override void ReformAll()
		{
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);
		}
	}
}