using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MySolutionExplorer
{
	[Serializable]
	[XmlRoot("Solution")]
	[XmlInclude(typeof(Project))]
	[XmlInclude(typeof(CppProject))]
	public class Solution : List<Project>
	{
		[XmlIgnore]
		public FileInfo DirSolution;
		[XmlIgnore]
		public DirectoryInfo Dir { get { return DirSolution.Directory; } }

		public Solution()
		{

		}

		public Solution(string path)
		{
			DirSolution = new FileInfo(path);
		}

		public void FindProjects()
		{
			foreach (var i in Dir.GetDirectories())
			{
				if (i.Name.Contains("[cpp]"))
				{
					Add(new CppProject(i.FullName));
				}
			}
		}

		public new void Add(Project item)
		{
			base.Add(item);
			item.ParentSolution = this;
		}

		public void DeleteTrash()
		{
			try
			{
				Directory.Delete(Dir + MyEnum.Trash, true);
			}
			catch
			{
				//ignored
			}
		}

		public static Solution Load(string path)
		{
			var serializer = new XmlSerializer(typeof(Solution));
			Solution ret;

			using (var r = new StreamReader(path))
			{
				ret = (Solution)serializer.Deserialize(r);
			}

			ret.DirSolution = new FileInfo(path);
			return ret;
		}

		public void Save()
		{
			var serializer = new XmlSerializer(typeof(Solution));

			using (var w = new StreamWriter(DirSolution.FullName))
			{
				serializer.Serialize(w, this);
			}
		}

		public void CreateProject(Project proj)
		{
			Add(proj);

		}

		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}

		public static FileInfo RenameFile(FileInfo file, string name)
		{
			File.Move(file.FullName, file.DirectoryName + MyEnum.Slash + name);
			return new FileInfo(file.DirectoryName + MyEnum.Slash + name);
		}
	}
}