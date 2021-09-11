using ModernUISample01.Common;

namespace ModernUISample01.MVVM.Main
{
    public class MainVM : ObservableBase
    {
        #region フィールド
        private string _message = "Modern UI Template";
        #endregion

        #region プロパティ
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
