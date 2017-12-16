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
using System.ComponentModel;

using MySolutionExplorer.Core;

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
			get => savef;
			set
			{
				savef = value;
				Title = (dirfile?.Name ?? "") + (value ? "" : "*") + " - " + MyEnum.AppName;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
			mainGrid.ColumnDefinitions[2].Width = new GridLength(0);
			if (App.SolutionFile != null)
			{
				dirfile = new FileInfo(App.SolutionFile);
				s = Solution.Load(dirfile.FullName);
				ReloadTable();
				SaveFlag = true;
			}
		}

		/// <summary>
		/// Обновляет таблицу
		/// </summary>
		public void ReloadTable()
		{
			SaveFlag = false;
			mainTable.ItemsSource = null;
			mainTable.ItemsSource = s;
			//mainTable.ContextMenu = new ContextMenu();
		}

		/// <summary>
		/// Создание пустого решения
		/// </summary>
		public void CreateEmptySolution()
		{
			s = new Solution(dirfile.FullName);
			SaveFlag = true;
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
		/// Нажатие на кнопку сохранения решения
		/// </summary>
		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			s.Save();
			SaveFlag = true;
		}

		/// <summary>
		/// Закрытие решения
		/// </summary>
		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
			s = null;
			SaveFlag = true;
			Title = MyEnum.AppName;
			dir = null;
			dirfile = null;
			mainTable.ItemsSource = null;
		}

		/// <summary>
		/// Импорт
		/// </summary>
		private void importButton_Click(object sender, RoutedEventArgs e)
		{
			if (s.ImportProjects() > 0) ReloadTable();
		}

		/// <summary>
		/// Открытие
		/// </summary>
		private void openbutton_Click(object sender, RoutedEventArgs e)
		{
			if (OpenSolutionDialog())
			{
				s = Solution.Load(dirfile.FullName);
				mainTable.ItemsSource = s;
				SaveFlag = true;
			}
		}

		/// <summary>
		/// Обзор
		/// </summary>
		/// <returns>Выбрал ли пользователь файл</returns>
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

		/// <summary>
		/// Выход
		/// </summary>
		private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Close();

		/// <summary>
		/// Создание пустого решения (нажатие на кнопку)
		/// </summary>
		private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (OpenSolutionDialog())
			{
				CreateEmptySolution();
			}
		}

		/// <summary>
		/// Открытие окна создания проекта
		/// </summary>
		private void CreateProjMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var w = new CreateProjectWindow(s);
			w.Show();
			w.Create += W_Create;
		}

		/// <summary>
		/// Действия при добавлении проекта
		/// </summary>
		private void W_Create(object sender, ProjectEventArgs e) => ReloadTable();

		private void syncButton_Click(object sender, RoutedEventArgs e)
		{
			var myDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "MySLN|*.mysln|XML|*.xml|Все файлы|*.*",
				CheckFileExists = false,
			};
			if (myDialog.ShowDialog() == true)
			{
				var dir = new FileInfo(myDialog.FileName).Directory;
				s.Sync(dir);
			}
		}

		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var selproj = (Project)mainTable.SelectedItem;
			selproj.Delete();
			ReloadTable();
		}

		private void ExludeMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var selproj = (Project)mainTable.SelectedItem;
			s.Remove(selproj);
			ReloadTable();
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if (SaveFlag == false)
			{
				var r = MessageBox.Show(this, "Сохранить?", MyEnum.AppName, MessageBoxButton.YesNoCancel);
				if (r == MessageBoxResult.Yes)
				{
					SaveFlag = true;
					s.Save();
				}
				if (r == MessageBoxResult.Cancel)
				{
					e.Cancel = true;
				}
			}
		}

		private GridViewColumnHeader listViewSortCol = null;
		private SortAdorner listViewSortAdorner = null;

		private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader column = (sender as GridViewColumnHeader);
			string sortBy = column.Tag.ToString();
			if (listViewSortCol != null)
			{
				AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
				mainTable.Items.SortDescriptions.Clear();
			}

			ListSortDirection newDir = ListSortDirection.Ascending;
			if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
				newDir = ListSortDirection.Descending;

			listViewSortCol = column;
			listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
			AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
			mainTable.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		public class SortAdorner : Adorner
		{
			private static Geometry ascGeometry =
					Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

			private static Geometry descGeometry =
					Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

			public ListSortDirection Direction { get; private set; }

			public SortAdorner(UIElement element, ListSortDirection dir) : base(element)
			{
				Direction = dir;
			}

			protected override void OnRender(DrawingContext drawingContext)
			{
				base.OnRender(drawingContext);

				if (AdornedElement.RenderSize.Width < 20)
					return;

				TranslateTransform transform = new TranslateTransform
						(
								AdornedElement.RenderSize.Width - 15,
								(AdornedElement.RenderSize.Height - 5) / 2
						);
				drawingContext.PushTransform(transform);

				Geometry geometry = ascGeometry;
				if (Direction == ListSortDirection.Descending)
					geometry = descGeometry;
				drawingContext.DrawGeometry(Brushes.Black, null, geometry);

				drawingContext.Pop();
			}

		}

		private void mainTable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void mainTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var si = (Project)((ListView)e.Source).SelectedItem;
			if (si == null) return;
			mainGrid.ColumnDefinitions[2].Width = new GridLength(135);
			taskNumber.Text = si.Number.ToString();
			taskName.Text = si.TaskName;
			taskSite.Text = si.Site;
			taskLang.Content = "Язык: " + si.Lang;
			//if (e.AddedItems.Count == 1)
			//{
			//	//infoTree.ItemsSource = e.AddedItems;
			//}
		}

		private void mainTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var si = ((ListView)e.Source).SelectedItem;
			if (si == null) return;
			((ListView)e.Source).SelectedItem = null;
			ReloadTable();
			mainGrid.ColumnDefinitions[2].Width = new GridLength(0);
		}
	}
}