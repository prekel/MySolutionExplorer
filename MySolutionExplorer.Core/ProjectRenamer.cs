// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolutionExplorer.Core
{
	public class ProjectRenamer<T> : IDisposable
		where T : CSharpProject, new()
	{
		private T OldProject { get; set; }
		public T NewProject { get; set; }
		
		public ProjectRenamer(T p)
		{
			OldProject = p;
			NewProject = new T();
			NewProject.Number = OldProject.Number;
			NewProject.TaskName = OldProject.TaskName;
			NewProject.Site = OldProject.Site;
			NewProject.Lang = OldProject.Lang;
			NewProject.ParentSolution = OldProject.ParentSolution;
			NewProject.Path = OldProject.ParentSolution.Dir + MyEnum.Slash + OldProject.Name;
		}

		public void Rename()
		{
			NewProject.TaskName = "123";
			if (OldProject.ParentSolution.Dir.GetDirectories().Any(u => u.Name == NewProject.Name)) return;
			//var old =
			//Directory.CreateDirectory(Dir.FullName);
			//Directory.Move()
			//var np = new Project();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
