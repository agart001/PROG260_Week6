
namespace PROG260_Week6
{
    /// <summary>
    /// Represents an input file with properties for path, directory, name, extension, and a delimiter.
    /// Implements the IFile interface.
    /// </summary>
    public class InputFile : IFile
    {
        /// <summary>
        /// Gets or sets the full path of the input file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the directory of the input file.
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// Gets or sets the name of the input file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the extension of the input file.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Default constructor for the InputFile class.
        /// </summary>
        public InputFile()
        {
            // Empty constructor
        }

        /// <summary>
        /// Parameterized constructor for the InputFile class, allowing initialization of key properties.
        /// </summary>
        /// <param name="path">The full path of the input file.</param>
        /// <param name="name">The name of the input file.</param>
        /// <param name="extension">The extension of the input file.</param>
        public InputFile(string path, string name, string extension)
        {
            // Set properties based on input parameters
            Path = path;
            Dir = Path.Substring(0, Path.LastIndexOf('\\'));
            Name = name.Substring(0, name.LastIndexOf('.'));
            Extension = extension;
        }
    }
}