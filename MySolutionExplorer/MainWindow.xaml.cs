// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Solution s;
		DirectoryInfo dir;
		FileInfo dirfile;
		private bool savef;

		/// <summary>
		/// Флаг сохранения, при изменении меняет заголовок
		/// </summary>
		public bool SaveFlag
		{
			get { return savef; }
			set
			{
				savef = value;
				Title = (dirfile == null ? "" : dirfile.Name) + (value ? "" : "*") + " - " + MyEnum.AppName;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		public void CreateEmptySolution()
		{
			s = new Solution(dirfile.FullName);
			SaveFlag = true;
		}

		public void CreateCppProject()
		{
			CppProject.Create(s, nameText.Text, siteText.Text, numberText.Text, dir);
		}

		public void CreateCSharpProject()
		{
			CSharpProject.Create(s, nameText.Text, siteText.Text, numberText.Text, dir);
		}

		public void ReloadTable()
		{
			mainTable.ItemsSource = null;
			mainTable.ItemsSource = s;
		}

		/// <summary>
		/// Нажатие на кнопку загрузки
		/// </summary>
		private void loadButton_Click(object sender, RoutedEventArgs e)
		{
			s = Solution.Load(dirfile.FullName);
			mainTable.ItemsSource = s;
			SaveFlag = true;
		}

		/// <summary>
		/// Нажатие на кнопку создания проекта
		/// </summary>
		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			if (s == null)
			{
				CreateEmptySolution();
				return;
			}
			var lang = ((TextBlock)langList.SelectedValue).Text;
			if (lang == "cpp")
			{
				CreateCppProject();
			}
			else if (lang == "cs")
			{
				CreateCSharpProject();
			}
			ReloadTable();
			SaveFlag = false;
		}

		/// <summary>
		/// Нажатие на кнопку сохранения решения
		/// </summary>
		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			s.Save();
			SaveFlag = true;
		}

		/// <summary>
		/// Нажатие на кнопку обзора
		/// </summary>
		private void showButton_Click(object sender, RoutedEventArgs e)
		{
			OpenSolutionDialog();
		}

		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
			s = null;
			SaveFlag = true;
			Title = MyEnum.AppName;
			dir = null;
			dirfile = null;
			mainTable.ItemsSource = null;
		}

		private void importButton_Click(object sender, RoutedEventArgs e)
		{
			s.ImportProjects();
			SaveFlag = false;
			ReloadTable();
		}

		private void openbutton_Click(object sender, RoutedEventArgs e)
		{
			if (OpenSolutionDialog())
			{
				s = Solution.Load(dirfile.FullName);
				mainTable.ItemsSource = s;
				SaveFlag = true; 
			}
		}

		public bool OpenSolutionDialog()
		{
			var myDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "MySLN|*.mysln|XML|*.xml|Все файлы|*.*",
				CheckFileExists = false,
			};
			if (myDialog.ShowDialog() == true)
			{
				dirfile = new FileInfo(myDialog.FileName);
				dir = dirfile.Directory;
				return true;
			}
			return false;
		}

		private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (OpenSolutionDialog())
			{
				CreateEmptySolution();
			}
		}
	}
}