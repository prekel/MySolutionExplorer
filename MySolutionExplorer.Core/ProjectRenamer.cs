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
		public Project NewProject { get; set; }
		
		public ProjectRenamer(T p)
		{
			OldProject = p;
			NewProject = new T
			{
				Number = OldProject.Number,
				TaskName = OldProject.TaskName,
				Site = OldProject.Site,
				Lang = OldProject.Lang,
				ParentSolution = OldProject.ParentSolution,
				Path = OldProject.ParentSolution.Dir + MyEnum.Slash + OldProject.Name
			};
		}

		public void Rename()
		{
			if (OldProject.ParentSolution.Dir.GetDirectories().Any(u => u.Name == NewProject.Name)) return;
			NewProject.Dir = null;
			Directory.CreateDirectory(NewProject.Dir.FullName);
			Directory.Move(OldProject.Path, NewProject.Path);
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
