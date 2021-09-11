using ModernUISample01.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using ClosedXML.Excel;

namespace ModernUISample01.MVVM.Main
{
    public class MainVM : ObservableBase
    {
        #region 定数
        public enum OutputFormatOption
        {
            Excel,
            CSV
        }

        private const int TEXT_START_INDEX = 3;
        #endregion

        #region フィールド
        private string _appTitle = "ModernUISample01";

        private string _targetFolderPath
            = string.IsNullOrEmpty(Properties.Settings.Default.TargetFolderPath) ? @"C:\" : Properties.Settings.Default.TargetFolderPath;

        private string _outputFolderPath
            = string.IsNullOrEmpty(Properties.Settings.Default.OutputFolderPath) ? @"C:\" : Properties.Settings.Default.OutputFolderPath;

        private string _fileSearchPattern
            = string.IsNullOrEmpty(Properties.Settings.Default.FileSearchPattern) ? "*.txt" : Properties.Settings.Default.FileSearchPattern;

        private SearchOption _folderSearchOption = SearchOption.TopDirectoryOnly;

        private OutputFormatOption _outputFileFormat = OutputFormatOption.CSV;

        private List<MappedModel> _mappedDataList = new();

        private string _statusBarText = string.Empty;
        #endregion

        #region プロパティ
        public string AppTitle
        {
            get => _appTitle;
            set
            {
                if (_appTitle != value)
                {
                    _appTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TargetFolderPath
        {
            get => _targetFolderPath;
            set
            {
                if (_targetFolderPath != value)
                {
                    _targetFolderPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string OutputFolderPath
        {
            get => _outputFolderPath;
            set
            {
                if (_outputFolderPath != value)
                {
                    _outputFolderPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FileSearchPattern
        {
            get => _fileSearchPattern;
            set
            {
                if (_fileSearchPattern != value)
                {
                    _fileSearchPattern = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SearchTopDirectoryOnlyOption
        {
            get => FolderSearchOption == SearchOption.TopDirectoryOnly;
            set => FolderSearchOption = SearchOption.TopDirectoryOnly;
        }

        public bool SearchAllDirectoriesOption
        {
            get => FolderSearchOption == SearchOption.AllDirectories;
            set => FolderSearchOption = SearchOption.AllDirectories;
        }

        public SearchOption FolderSearchOption
        {
            get => _folderSearchOption;
            set
            {
                if (_folderSearchOption != value)
                {
                    if (!Enum.IsDefined(typeof(SearchOption), value))
                    {
                        ArgumentOutOfRangeException ex = new("OutOfSearchOptionRange", "SearchOptionの定義範囲外の値です。");
                        throw ex;
                    }

                    _folderSearchOption = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SearchTopDirectoryOnlyOption));
                    OnPropertyChanged(nameof(SearchAllDirectoriesOption));
                }
            }
        }

        public bool OutputExcelOption
        {
            get => OutputFileFormat == OutputFormatOption.Excel;
            set => OutputFileFormat = OutputFormatOption.Excel;
        }

        public bool OutputCSVOption
        {
            get => OutputFileFormat == OutputFormatOption.CSV;
            set => OutputFileFormat = OutputFormatOption.CSV;
        }

        public OutputFormatOption OutputFileFormat
        {
            get => _outputFileFormat;
            set
            {
                if (_outputFileFormat != value)
                {
                    if (!Enum.IsDefined(typeof(OutputFormatOption), value))
                    {
                        ArgumentOutOfRangeException ex = new("OutOfOutputFormatRange", "OutputFormatの定義範囲外の値です。");
                        throw ex;
                    }

                    _outputFileFormat = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(OutputExcelOption));
                    OnPropertyChanged(nameof(OutputCSVOption));
                }
            }
        }

        public DelegateCommand<string> ReferenceCommand { get; private set; }

        public DelegateCommand RunCommand { get; private set; }

        public string StatusBarText
        {
            get => _statusBarText;
            set
            {
                if (_statusBarText != value)
                {
                    _statusBarText = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region コンストラクタ
        public MainVM()
        {
            ReferenceCommand = new DelegateCommand<string>(ReferenceButton_Clicked);
            RunCommand = new DelegateCommand(RunButton_Clicked);
        }
        #endregion

        #region イベントハンドラ
        private void ReferenceButton_Clicked(string commandParameter)
        {
            using CommonOpenFileDialog openFileDialog = new()
            {
                Title = "フォルダを選択してください",
                IsFolderPicker = true,
            };
            switch (commandParameter)
            {
                case "1":
                    openFileDialog.InitialDirectory = TargetFolderPath;
                    break;
                case "2":
                    openFileDialog.InitialDirectory = OutputFolderPath;
                    break;
                default:
                    break;
            }

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                switch (commandParameter)
                {
                    case "1":
                        TargetFolderPath = openFileDialog.FileName;
                        break;
                    case "2":
                        OutputFolderPath = openFileDialog.FileName;
                        break;
                    default:
                        break;
                }
            }
        }

        private async void RunButton_Clicked()
        {
            int cnt = 0;
            _mappedDataList = new();

            SetAppSettings();

            StatusBarText = "マッピング中...";

            string[] filePaths = GetFilePaths();
            foreach (string filePath in filePaths)
            {
                MappedModel mappedData = await GetMappedDataAsync(filePath);

                if (mappedData != null)
                {
                    _mappedDataList.Add(mappedData);
                }

                StatusBarText = $"マッピング中... {++cnt}/{filePaths.Length} 件";
            }

            try
            {
                switch (OutputFileFormat)
                {
                    case OutputFormatOption.CSV:
                        await OutputCSVFileAsync();
                        StatusBarText = $"完了 ({Path.Combine(OutputFolderPath, "MappedData.csv")})";
                        break;
                    case OutputFormatOption.Excel:
                        await OutputExcelFileAsync();
                        StatusBarText = $"完了 ({Path.Combine(OutputFolderPath, "MappedData.xlsx")})";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                StatusBarText = "ファイル出力に失敗しました。";
            }
        }
        #endregion

        #region クラスメソッド
        private static string GetFirstLine(string filePath)
        {
            using StreamReader reader = new(filePath, Encoding.GetEncoding("Shift-JIS"));
            return reader.ReadLine() ?? string.Empty;
        }

        private static async Task<MappedModel> GetMappedDataAsync(string filePath)
        {
            string text = GetFirstLine(filePath);
            MappedModel model = new();

            await Task.Run(() =>
            {
                try
                {
                    string tableNameJP = text[TEXT_START_INDEX..text.IndexOf("(", StringComparison.InvariantCulture)];
                    string tableName = text.Substring(text.IndexOf("(", StringComparison.InvariantCulture) + 1,
                        text.IndexOf(")", StringComparison.InvariantCulture) - text.IndexOf("(", StringComparison.InvariantCulture) - 1);
                    string groupName = tableName.Substring(0, 3);

                    model.TableNameJP = tableNameJP;
                    model.TableName = tableName;
                    model.GroupName = groupName;
                    model.FilePath = filePath;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + filePath);
                    model = null;
                }
            });

            return model;
        }
        #endregion

        #region プライベートメソッド
        private void SetAppSettings()
        {
            Properties.Settings.Default.TargetFolderPath = this.TargetFolderPath;
            Properties.Settings.Default.OutputFolderPath = this.OutputFolderPath;
            Properties.Settings.Default.FileSearchPattern = this.FileSearchPattern;
            Properties.Settings.Default.Save();
        }

        private string[] GetFilePaths()
        {
            string[] filePaths = Directory.GetFiles(TargetFolderPath, FileSearchPattern, FolderSearchOption);
            return filePaths;
        }

        private async Task OutputCSVFileAsync()
        {
            int cnt = 0;
            StatusBarText = $"CSVファイル出力中...";

            try
            {
                using StreamWriter writer = new(Path.Combine(OutputFolderPath, "MappedData.csv"), false, Encoding.GetEncoding("Shift-JIS"));
                string[] header = _mappedDataList.FirstOrDefault()?.GetPropertyNames() ?? Array.Empty<string>();

                await writer.WriteLineAsync(string.Join(',', header));

                foreach (MappedModel data in _mappedDataList)
                {
                    string line = string.Join(',', data.GetValuesArray());
                    await writer.WriteLineAsync(line);
                    StatusBarText = $"CSVファイル出力中... {++cnt / _mappedDataList.Count} %";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task OutputExcelFileAsync()
        {
            StatusBarText = $"Excelファイル出力中...";

            await Task.Run(() =>
            {
                try
                {
                    XLWorkbook xlBook = new();
                    IXLWorksheet xlSheet = xlBook.Worksheets.Add("Sheet1");

                    object[][] dataMatrix = GetDataMatrix();

                    _ = xlSheet.Cell(1, 1).InsertData(dataMatrix);

                    xlBook.SaveAs(Path.Combine(OutputFolderPath, "MappedData.xlsx"));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            });
        }

        private object[][] GetDataMatrix()
        {
            try
            {
                List<IEnumerable<object>> matrix = new();
                string[] clmNames = _mappedDataList.FirstOrDefault()?.GetPropertyNames() ?? Array.Empty<string>();

                matrix.Add(clmNames);

                foreach (MappedModel data in _mappedDataList)
                {
                    List<object> rowData = new();

                    foreach (string clmName in clmNames)
                    {
                        PropertyInfo propertyInfo = data.GetType().GetProperty(clmName);
                        rowData.Add(propertyInfo?.GetValue(data));
                    }

                    matrix.Add(rowData);
                }
                return matrix.Select(x => x.ToArray()).ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
