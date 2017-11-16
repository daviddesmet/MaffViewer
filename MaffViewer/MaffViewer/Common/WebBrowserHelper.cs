namespace MaffViewer.Common
{
    using System.Windows;
    using DotNetBrowser.WPF;

    public class WebBrowserHelper
    {
        public static readonly DependencyProperty UrlDataProperty = DependencyProperty.RegisterAttached("UrlData", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnUrlDataChanged));

        public static string GetUrlData(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(UrlDataProperty);
        }

        public static void SetUrlData(DependencyObject dependencyObject, string url)
        {
            dependencyObject.SetValue(UrlDataProperty, url);
        }

        private static void OnUrlDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = (WPFBrowserView)d;
            webBrowser.Browser.LoadURL((string)e.NewValue);
        }
    }
}
