using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModernUISample01.Common
{
    /// <summary>
    /// 変更通知ベースクラス
    /// </summary>
    public class ObservableBase : INotifyPropertyChanged
    {
        #region イベント
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region パブリックメソッド
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
