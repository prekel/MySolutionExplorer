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

		private void loadButton_Click(object sender, RoutedEventArgs e)
		{
			s = Solution.Load(dirfile.FullName);
			mainTable.ItemsSource = s;
			SaveFlag = true;
		}

		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			if (s == null)
			{
				s = new Solution { DirSolution = new FileInfo(dirfile.FullName) };
			}
			var lang = ((TextBlock) langList.SelectedValue).Text;
			if (lang == "cpp")
			{
				var p = new CppProject
				{
					ParentSolution = s,
					TaskName = nameText.Text,
					Site = siteText.Text,
					Number = int.Parse(numberText.Text),
					Lang = lang
				};
				p.Path = dir + MyEnum.Slash + p.Name;
				p.CreateFiles();
				s.Add(p);
			}
			else if (lang == "cs")
			{
				var p = new CSharpProject
				{
					ParentSolution = s,
					TaskName = nameText.Text,
					Site = siteText.Text,
					Number = int.Parse(numberText.Text),
					Lang = lang
				};
				p.Path = dir + MyEnum.Slash + p.Name;
				p.CreateFiles();
				s.Add(p);
			}
			mainTable.ItemsSource = null;
			mainTable.ItemsSource = s;
			SaveFlag = false;
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			s.Save();
			SaveFlag = true;
		}

		private void showButton_Click(object sender, RoutedEventArgs e)
		{
			var myDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "MySLN|*.mysln|XML|*.xml|Все файлы|*.*",
				CheckFileExists = true
			};
			if (myDialog.ShowDialog() == true)
			{
				dirfile = new FileInfo(myDialog.FileName);
				dir = dirfile.Directory;
			}
		}
	}
}