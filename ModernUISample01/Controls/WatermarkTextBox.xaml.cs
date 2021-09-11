using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernUISample01.Controls
{
    /// <summary>
    /// WatermarkTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class WatermarkTextBox : UserControl
    {
        #region プロパティ
        public string WatermarkText
        {
            get => (string)GetValue(WatermarkTextProperty);
            set => SetValue(WatermarkTextProperty, value);
        }

        public Brush WatermarkColor
        {
            get => (Brush)GetValue(WatermarkColorProperty);
            set => SetValue(WatermarkColorProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Thickness TextMargin
        {
            get => (Thickness)GetValue(TextMarginProperty);
            set => SetValue(TextMarginProperty, value);
        }

        public Thickness TextPadding
        {
            get => (Thickness)GetValue(TextPaddingProperty);
            set => SetValue(TextPaddingProperty, value);
        }

        public string MainText
        {
            get => (string)GetValue(MainTextProperty);
            set => SetValue(MainTextProperty, value);
        }
        #endregion

        #region 依存プロパティ
        public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.Register(
            nameof(WatermarkText), typeof(string), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(string.Empty));


        public static readonly DependencyProperty WatermarkColorProperty = DependencyProperty.Register(
            nameof(WatermarkColor), typeof(Brush), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.AffectsRender));


        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(new CornerRadius(5), FrameworkPropertyMetadataOptions.AffectsArrange));


        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
            nameof(TextMargin), typeof(Thickness), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.AffectsMeasure));


        public static readonly DependencyProperty TextPaddingProperty = DependencyProperty.Register(
            nameof(TextPadding), typeof(Thickness), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.AffectsMeasure));


        public static readonly DependencyProperty MainTextProperty = DependencyProperty.Register(
            nameof(MainText), typeof(string), typeof(WatermarkTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region コンストラクタ
        public WatermarkTextBox()
        {
            InitializeComponent();
        }
        #endregion
    }
}
