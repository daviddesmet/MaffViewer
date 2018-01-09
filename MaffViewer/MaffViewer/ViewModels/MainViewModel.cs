namespace MaffViewer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml.Linq;

    using Common;
    using Models;
    using Views;

    using Dragablz;

    /// <summary>
    /// The Main ViewModel.
    /// </summary>
    /// <seealso cref="MaffViewer.ViewModels.ViewModelBase" />
    public class MainViewModel : ViewModelBase
    {
        private readonly IInterTabClient _interTabClient;
        private readonly ObservableCollection<TabContent> _tabContents = new ObservableCollection<TabContent>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            _interTabClient = new DefaultInterTabClient();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>MainViewModel.</returns>
        public static MainViewModel Initialize()
        {
            var result = new MainViewModel();

            result.TabContents.Add(new TabContent("New Tab", new WebPage()));
            return result;
        }

        /// <summary>
        /// Gets the tab contents.
        /// </summary>
        /// <value>The tab contents.</value>
        public ObservableCollection<TabContent> TabContents => _tabContents;

        /// <summary>
        /// Gets or sets the selected tab.
        /// </summary>
        /// <value>The selected tab.</value>
        public TabContent SelectedTab
        {
            get => GetProperty<TabContent>();
            set => SetProperty(value);
        }

        /// <summary>
        /// Gets the InterTabClient.
        /// </summary>
        /// <value>The InterTabClient.</value>
        public IInterTabClient InterTabClient => _interTabClient;

        /// <summary>
        /// Gets the new item factory.
        /// </summary>
        /// <value>The new item factory.</value>
        public static Func<object> NewItemFactory => () => new TabContent("New Tab", new WebPage());

        /// <summary>
        /// Gets the preview drop command.
        /// </summary>
        /// <value>The preview drop command.</value>
        public ICommand PreviewDropCommand => new RelayCommand(HandlePreviewDrop);

        /// <summary>
        /// Handles the preview drop.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void HandlePreviewDrop(object obj)
        {
            var ido = obj as IDataObject;
            if (ido is null)
                return;

            if (!ido.GetFormats().Contains(DataFormats.FileDrop))
                return;

            var data = ido.GetData(DataFormats.FileDrop) as string[];
            if (!Path.GetExtension(data[0]).Equals(".maff"))
                return;

            // TODO: Handle exceptions
            using (var archive = ZipFile.Open(data[0], ZipArchiveMode.Read))
            {
                var db = archive.Entries.FirstOrDefault(e => e.FullName.Contains("index.rdf"));

                var rdf = XDocument.Load(db.Open());
                XNamespace ns = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
                var maffNodes = rdf.Descendants(ns + "Description").Elements();

                var title = maffNodes.FirstOrDefault(n => n.Name.LocalName == "title").FirstAttribute.Value;
                var url = maffNodes.FirstOrDefault(n => n.Name.LocalName == "originalurl").FirstAttribute.Value;
                var archiveTime = maffNodes.FirstOrDefault(n => n.Name.LocalName == "archivetime").FirstAttribute.Value;
                var indexFileName = maffNodes.FirstOrDefault(n => n.Name.LocalName == "indexfilename").FirstAttribute.Value;

                var temp = Path.Combine(Path.GetTempPath(), archive.Entries[0].FullName);
                archive.ExtractToDirectory(Path.GetTempPath(), ZipArchiveExtensions.OverwriteMethod.Always);

                var tab = new TabContent(title, new WebPage() { DataContext = new PageViewModel() { OriginalUrl = url, ArchiveTime = archiveTime, UrlData = Path.Combine(temp, indexFileName) } });
                TabContents.Add(tab);

                SelectedTab = tab;
            }
        }
    }
}
