using System.IO;
using JetBrains.Annotations;

namespace PH.WebPathTranslator
{
    /// <summary>
    /// Translate a Path From/To Web-Relative
    ///
    /// Useful for using web-related paths within services
    ///
    /// <seealso cref="FileInfo"/>
    /// <seealso cref="DirectoryInfo"/>
    /// </summary>
    public interface IWebPathTranslator
    {
        /// <summary>
        /// Translate a Web-Relative path to FileSystem-Path
        /// </summary>
        /// <param name="webrelativePath">Relative path <example>~/SomeFolder/</example></param>
        /// <returns>File System Path</returns>
        string ToFileSystemPath([NotNull] string webrelativePath);

        /// <summary>
        /// Translate a <see cref="FileInfo"/> full path to web-relative
        /// </summary>
        /// <param name="file">FileInfo file</param>
        /// <returns>Web-Relative Path</returns>
        string ToWebRelativePath([NotNull] FileInfo file);

        /// <summary>
        /// Translate a <see cref="DirectoryInfo"/> full path to web-relative
        /// </summary>
        /// <param name="directory">DirectoryInfo directory</param>
        /// <returns>Web-Relative Path</returns>
        string ToWebRelativePath([NotNull] DirectoryInfo directory);


        /// <summary>
        /// Return a <see cref="FileInfo"/> from its web-relative position
        /// </summary>
        /// <param name="webrelativePath">Web-Relative path <example>~/SomeFolder/SomeFile.zip</example></param>
        /// <returns>FileInfo file</returns>
        FileInfo GetFile([NotNull] string webrelativePath);

        /// <summary>
        /// Return a <see cref="DirectoryInfo"/> from its web-relative position
        /// </summary>
        /// <param name="webrelativePath">Web-Relative path <example>~/SomeFolder/</example></param>
        /// <returns>DirectoryInfo directory</returns>
        DirectoryInfo GetDirectory([NotNull] string webrelativePath);



    }
}
