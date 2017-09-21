// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using RoboSharp;

namespace MySolutionExplorer.Core
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
    [XmlInclude(typeof(PyProject))]
    [XmlInclude(typeof(XmlProject))]
    [XmlInclude(typeof(IprProject))]
    [XmlInclude(typeof(JavaProject))]
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
        public DirectoryInfo Dir => DirSolution.Directory;

        public Solution()
        {
        }

        public Solution(string path) => DirSolution = new FileInfo(path);

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

        public void CreateProject(Langs lang, string task, string site, string number, DirectoryInfo dir)
        {
            Project p;
            switch (lang)
            {
                case Langs.Cpp:
                    p = new CppProject() { Lang = MyEnum.CppNoDot };
                    break;
                case Langs.CSharp:
                    p = new CSharpProject() { Lang = MyEnum.CSharpNoDot };
                    break;
                case Langs.Py:
                    p = new PyProject() { Lang = MyEnum.PythonNoDot };
                    break;
                case Langs.Java:
                    p = new JavaProject() { Lang = MyEnum.JavaNoDot };
                    break;
                default:
                    return;
            }
            p.ParentSolution = this;
            p.TaskName = task;
            p.Site = site;
            p.Number = int.Parse(number);
            p.Path = dir + MyEnum.Slash + p.Name;
            p.CreateFiles();
            Add(p);
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
                if (i.Name.Contains(MyEnum.PySuff))
                {
                    var p = new PyProject
                    {
                        ParentSolution = this,
                        Name = i.Name,
                        Lang = "py",
                        Dir = i
                    };
                    Add(p);
                }
                if (i.Name.Contains(MyEnum.JavaSuff))
                {
                    var p = new JavaProject
                    {
                        ParentSolution = this,
                        Name = i.Name,
                        Lang = "java",
                        Dir = i
                    };
                    Add(p);
                }
                cntn:;
            }
            return Count - c;
        }

		public static void Sync()
		{

		}

		public void Sync(DirectoryInfo dir2)
		{
			var dir1 = Dir;
			var sync = new RoboCommand();
			sync.CopyOptions.Source = dir1.FullName;
			sync.CopyOptions.Destination = dir2.FullName;
			sync.CopyOptions.CopySubdirectories = true;
			sync.CopyOptions.UseUnbufferedIo = true;
			sync.Start();
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