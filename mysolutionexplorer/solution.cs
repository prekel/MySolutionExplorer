// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MySolutionExplorer
{
	/// <summary>
	/// Решение (группа проектов)
	/// </summary>
	[Serializable]
	[XmlRoot("Solution")]
	[XmlInclude(typeof(Project))]
	[XmlInclude(typeof(VSProject))]
	[XmlInclude(typeof(CppProject))]
	[XmlInclude(typeof(CSharpProject))]
	public class Solution : List<Project>
	{
		/// <summary>
		/// Файл решения
		/// </summary>
		[XmlIgnore] public FileInfo DirSolution { get; set; }

		/// <summary>
		/// Директория решения
		/// </summary>
		[XmlIgnore]
		public DirectoryInfo Dir
		{
			get { return DirSolution.Directory; }
		}

		public Solution()
		{
		}

		public Solution(string path)
		{
			DirSolution = new FileInfo(path);
		}

		/// <summary>
		/// Кандидат к удалению
		/// </summary>
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

		//public void ImportProjects()
		//{
		//	foreach (var i in Dir.GetDirectories())
		//	{
		//		if (i.Name.Contains("cpp"))
		//		{
		//			var regex = new System.Text.RegularExpressions.Regex("{a-z}+ {0-9}[4]. {a-z}+");
		//			var p = new CppProject
		//			{
		//				ParentSolution = this,
		//				TaskName = nameText.Text,
		//				Site = siteText.Text,
		//				Number = int.Parse(numberText.Text),
		//				Lang = lang
		//			};
		//			p.Path = dir + MyEnum.Slash + p.Name;
		//			p.CreateFiles();
		//			s.Add(p);
		//		}
		//	}
		//}

		/// <summary>
		/// Добавляет проект в решение
		/// </summary>
		/// <param name="item">Проект</param>
		public new void Add(Project item)
		{
			base.Add(item);
			item.ParentSolution = this;
		}

		/// <summary>
		/// Удаляет папку с собраным мусором
		/// </summary>
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

		/// <summary>
		/// Загружает решение
		/// </summary>
		/// <param name="path">Путь до файла</param>
		/// <returns>Решение</returns>
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

		/// <summary>
		/// Сохраняет решение 
		/// </summary>
		public void Save()
		{
			var serializer = new XmlSerializer(typeof(Solution));

			using (var w = new StreamWriter(DirSolution.FullName))
			{
				serializer.Serialize(w, this);
			}
		}

		/// <summary>
		/// Кандидат к удалению
		/// </summary>
		public void CreateProject(Project proj)
		{
			Add(proj);
		}

		public int ImportProjects()
		{
			var c = Count;
			foreach (var i in Dir.GetDirectories())
			{
				foreach (var j in this)
				{
					if (j.Path == i.FullName)
					{
						goto cntn;
					}
				}
				if (i.Name.Contains(MyEnum.CSharpSuff))
				{
					var p = new CSharpProject
					{
						ParentSolution = this,
						Name = i.Name,
						Lang = "cs",
						Dir = i
					};
					Add(p);
				}
				if (i.Name.Contains(MyEnum.CppSuff))
				{
					var p = new CppProject
					{
						ParentSolution = this,
						Name = i.Name,
						Lang = "cpp",
						Dir = i
					};
					Add(p);
				}
				cntn:;
			}
			return Count - c;
		}

		/// <summary>
		/// Копирует папку
		/// </summary>
		/// <param name="sourceDirName">Исходная папка</param>
		/// <param name="destDirName">Папка назначения</param>
		/// <param name="copySubDirs">Копировать ли подкаталоги</param>
		/// <copyright>Microsoft Corporation msdn.microsoft.com/ru-ru/library/bb762914</copyright>
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			var dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			var dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			var files = dir.GetFiles();
			foreach (var file in files)
			{
				var temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (var subdir in dirs)
				{
					var temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, true);
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