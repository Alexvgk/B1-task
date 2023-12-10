using Desktop_task.Repositories;
using Desktop_task.Services.DataDb;
using Desktop_task.Services.ParsingServices;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;

using System.Windows.Input;

namespace Desktop_task.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand OpenFileCommand { get; set; }
        public ICommand ParseExcelCommand { get; set; }
        public ICommand LoadFileDataCommand { get; set; }

        private DataTable? _excelData;
        private ObservableCollection<Model.File>? _loadedFiles;

        public ObservableCollection<Model.File> LoadedFiles
        {
            get { return _loadedFiles; }
            set
            {
                _loadedFiles = value;
                OnPropertyChanged();
            }
        }


        public bool IsTotal { get; set; }

        private string? _selectedFilePath;

        public string SelectedFilePath
        {
            get { return _selectedFilePath; }
            set
            {
                if (_selectedFilePath != value)
                {
                    _selectedFilePath = value;
                    OnPropertyChanged(nameof(SelectedFilePath));
                }
            }
        }


        private Model.File _selectedFile;
        public Model.File SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
                SetData();
            }
        }

        public DataTable ExcelData
        {
            get { return _excelData; }
            set
            {
                if (_excelData != value)
                {
                    _excelData = value;
                    OnPropertyChanged(nameof(ExcelData));
                }
            }
        }

        public MainViewModel()
        {
            OpenFileCommand = new DelegateCommand(OpenFile);
            ParseExcelCommand = new DelegateCommand(ParseExcel);
            LoadFileDataCommand = new DelegateCommand<Model.File>(LoadFileData);
            InitializeAsync();
        }


        private async void InitializeAsync()
        {
            FileRepo repo = new FileRepo(new FinanceDbContext());
            LoadedFiles = new ObservableCollection<Model.File>(await repo.GetAllAsync());
        }
        private async void SetData()
        {
            ExcelData = await ParsingServices.TryParseToDataTable(_selectedFile);
        }

        private void LoadFileData(Model.File obj)
        {
            LoadedFiles.Add(obj);
        }

        private async void ParseExcel()
        {
            ParsingServices.TryParse(_selectedFilePath);
            FileRepo repo = new FileRepo(new FinanceDbContext());
            LoadedFiles = new ObservableCollection<Model.File>(await repo.GetAllAsync());
        }


        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFilePath = openFileDialog.FileName;
            }
        }
    }
}
