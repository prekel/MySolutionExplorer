// Copyright (c) 2017 Vladislav Prekel

namespace MySolutionExplorer.Core
{
	public interface IProjectRenamer
	{
		Project NewProject { get; set; }
		void Rename();
	}
}