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
	public class ProjectRenamer<T> : IDisposable, IProjectRenamer
		where T : Project, new()
	{
		private T OldProject { get; set; }
		private Project NewProject { get; set; }
		private Solution Solution { get; set; }

		public int Number { get => NewProject.Number; set => NewProject.Number = value; }
		public string Site { get => NewProject.Site; set => NewProject.Site = value; }
		public string TaskName { get => NewProject.TaskName; set => NewProject.TaskName = value; }

		public ProjectRenamer(T p)
		{
			OldProject = p;
			Solution = p.ParentSolution;
			NewProject = new T
			{
				Number = OldProject.Number,
				TaskName = OldProject.TaskName,
				Site = OldProject.Site,
				Lang = OldProject.Lang,
				ParentSolution = Solution,
				Path = Solution.Dir + MyEnum.Slash + OldProject.Name
			};
			Solution[Solution.IndexOf(OldProject)] = NewProject;
		}

		public void Rename()
		{
			if (OldProject.ParentSolution.Dir.GetDirectories().Any(u => u.Name == NewProject.Name)) return;
			NewProject.Dir = null;
			Directory.Move(OldProject.Path, NewProject.Path);
			NewProject.ReformAll();
		}

		public void Dispose()
		{
			OldProject.Delete();
		}
	}
}
