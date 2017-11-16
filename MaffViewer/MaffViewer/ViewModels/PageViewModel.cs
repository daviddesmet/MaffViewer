namespace MaffViewer.ViewModels
{
    /// <summary>
    /// The Page ViewModel.
    /// </summary>
    /// <seealso cref="MaffViewer.ViewModels.ViewModelBase" />
    public class PageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the original URL.
        /// </summary>
        /// <value>The original URL.</value>
        public string OriginalUrl
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        /// <summary>
        /// Gets or sets the archive time.
        /// </summary>
        /// <value>The archive time.</value>
        public string ArchiveTime
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        /// <summary>
        /// Gets or sets the URL data.
        /// </summary>
        /// <value>The URL data.</value>
        public string UrlData
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }
}
