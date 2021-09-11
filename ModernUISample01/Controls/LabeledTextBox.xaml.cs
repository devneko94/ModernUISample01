using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernUISample01.Controls
{
    /// <summary>
    /// LabeledTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class LabeledTextBox : UserControl
    {
        #region プロパティ
        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public Brush LabelColor
        {
            get => (Brush)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
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
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            nameof(LabelText), typeof(string), typeof(LabeledTextBox), new FrameworkPropertyMetadata(string.Empty));


        public static readonly DependencyProperty LabelColorProperty = DependencyProperty.Register(
            nameof(LabelColor), typeof(Brush), typeof(LabeledTextBox), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));


        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(LabeledTextBox), new FrameworkPropertyMetadata(new CornerRadius(5), FrameworkPropertyMetadataOptions.AffectsArrange));


        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
            nameof(TextMargin), typeof(Thickness), typeof(LabeledTextBox), new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.AffectsMeasure));


        public static readonly DependencyProperty TextPaddingProperty = DependencyProperty.Register(
            nameof(TextPadding), typeof(Thickness), typeof(LabeledTextBox), new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.AffectsMeasure));


        public static readonly DependencyProperty MainTextProperty = DependencyProperty.Register(
            nameof(MainText), typeof(string), typeof(LabeledTextBox), 
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region コンストラクタ
        public LabeledTextBox()
        {
            InitializeComponent();
        }
        #endregion
    }
}
