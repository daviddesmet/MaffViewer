namespace MaffViewer.Models
{
    //using System.ComponentModel;

    public class TabContent// : INotifyPropertyChanged
    {
        private readonly string _header;
        private readonly object _content;

        public TabContent(string header, object content)
        {
            _header = header;
            _content = content;
        }

        public string Header => _header;

        public object Content => _content;

        //private string _header;

        //public string Header
        //{
        //    get => _header;
        //    set => this.MutateVerbose(ref _header, value, args => PropertyChanged?.Invoke(this, args));
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
