namespace MaffViewer.Common
{
    using System.IO;
    using System.IO.Compression;

    public static class ZipArchiveExtensions
    {
        public enum OverwriteMethod
        {
            Always,
            IfNewer,
            Never
        }

        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, OverwriteMethod overwrite)
        {
            if (overwrite == OverwriteMethod.Never)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }

            foreach (var file in archive.Entries)
            {
                var destinationFileName = Path.Combine(destinationDirectoryName, file.FullName);
                var directory = Path.GetDirectoryName(destinationFileName);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                switch (overwrite)
                {
                    case OverwriteMethod.Always:
                        if (!string.IsNullOrWhiteSpace(file.Name))
                            file.ExtractToFile(destinationFileName, true);
                        break;
                    case OverwriteMethod.IfNewer:
                        if (!File.Exists(destinationFileName) || File.GetLastWriteTime(destinationFileName) < file.LastWriteTime)
                            file.ExtractToFile(destinationFileName, true);
                        break;
                    case OverwriteMethod.Never:
                        if (!File.Exists(destinationFileName))
                            file.ExtractToFile(destinationFileName);
                        break;
                }
            }
        }
    }
}
