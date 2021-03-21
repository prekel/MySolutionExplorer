// Copyright (c) 2017 Vladislav Prekel

namespace MySolutionExplorer.Core
{
	public interface IProjectRenamer 
	{
		int Number { get; set; }
		string Site { get; set; }
		string TaskName { get; set; }
		void Rename();
	}
}