namespace PROG260_Week6
{
    /// <summary>
    /// Represents a file with basic properties such as path, directory, name, and extension.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Gets or sets the full path of the file.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Gets or sets the directory of the file.
        /// </summary>
        string Dir { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the extension of the file.
        /// </summary>
        string Extension { get; set; }
    }
}