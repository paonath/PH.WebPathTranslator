using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

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
    public class WebPathTranslator : IWebPathTranslator
    {
        private readonly string _webRootPath;
        private const string Root = "~/";
        private readonly ILogger<WebPathTranslator> _logger;

        /// <summary>
        /// Initialize a new instance of <see cref="WebPathTranslator"/>
        ///
        /// <exception cref="ArgumentException">Throw ArgumentException if Web-Root Path is null</exception>
        /// </summary>
        /// <param name="webRootPath">Web-Root Path <example>c:\temp\myDir\</example></param>
        /// <param name="logger">Logger</param>
        public WebPathTranslator([NotNull] string webRootPath, [CanBeNull] ILogger<WebPathTranslator> logger = null)
        {
            if (string.IsNullOrEmpty(webRootPath))
                throw new ArgumentException("Value cannot be null or empty.", nameof(webRootPath));
            if (string.IsNullOrWhiteSpace(webRootPath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(webRootPath));

            _webRootPath = webRootPath;
            _logger      = logger;
            
            _logger?.LogTrace($"Web Root Path: '{_webRootPath}'");

        }

        #region Implementation of IWebPathTranslator

         
        /// <summary>
        /// Translate a Web-Relative path to FileSystem-Path
        /// </summary>
        /// <param name="webrelativePath">Relative path <example>~/SomeFolder/</example></param>
        /// <returns>File System Path</returns>
        [NotNull]
        public string ToFileSystemPath([NotNull] string webrelativePath)
        {
            if (string.IsNullOrEmpty(webrelativePath))
                throw new ArgumentException("Value cannot be null or empty.", nameof(webrelativePath));
            if (string.IsNullOrWhiteSpace(webrelativePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(webrelativePath));


            if (webrelativePath.StartsWith(Root, StringComparison.InvariantCulture))
                webrelativePath = webrelativePath.Replace(Root, $"{_webRootPath}{Path.DirectorySeparatorChar}");

            var pt = webrelativePath.Replace("//", "/").Replace("/", $"{Path.DirectorySeparatorChar}")
                                    .Replace($"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}",
                                             $"{Path.DirectorySeparatorChar}");
           
            _logger?.LogTrace($"Web-Path '{webrelativePath}' to '{pt}'");
            return pt;

        }

        /// <summary>
        /// Translate a <see cref="FileInfo"/> full path to web-relative
        /// </summary>
        /// <param name="file">FileInfo file</param>
        /// <returns>Web-Relative Path</returns>
        [NotNull]
        public string ToWebRelativePath([NotNull] FileInfo file)
        {
            if (file is null) 
                throw new ArgumentNullException(nameof(file));

            return ToWeb(file.FullName);
        }

        /// <summary>
        /// Translate a <see cref="DirectoryInfo"/> full path to web-relative
        /// </summary>
        /// <param name="directory">DirectoryInfo directory</param>
        /// <returns>Web-Relative Path</returns>
        [NotNull]
        public string ToWebRelativePath([NotNull] DirectoryInfo directory)
        {
            if (directory is null) 
                throw new ArgumentNullException(nameof(directory));

            return ToWeb(directory.FullName);
        }



        [NotNull]
        private string ToWeb([NotNull] string fullPath)
        {

            var ff = fullPath.Replace(_webRootPath, Root);

            var r = ff.Replace($"{Path.DirectorySeparatorChar}", "/").Replace("//", "/");

            _logger?.LogTrace($"Path '{fullPath}' to Web-Path '{r}'");
            return r;
        }

        /// <summary>
        /// Return a <see cref="FileInfo"/> from its web-relative position
        /// </summary>
        /// <param name="webrelativePath">Web-Relative path <example>~/SomeFolder/SomeFile.zip</example></param>
        /// <returns>FileInfo file</returns>
        [NotNull]
        public FileInfo GetFile([NotNull] string webrelativePath)
        {
            if (string.IsNullOrEmpty(webrelativePath))
                throw new ArgumentException("Value cannot be null or empty.", nameof(webrelativePath));
            if (string.IsNullOrWhiteSpace(webrelativePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(webrelativePath));


            return new FileInfo(ToFileSystemPath(webrelativePath));
        }

        /// <summary>
        /// Return a <see cref="DirectoryInfo"/> from its web-relative position
        /// </summary>
        /// <param name="webrelativePath">Web-Relative path <example>~/SomeFolder/</example></param>
        /// <returns>DirectoryInfo directory</returns>
        [NotNull]
        public DirectoryInfo GetDirectory([NotNull] string webrelativePath)
        {
            if (string.IsNullOrEmpty(webrelativePath))
                throw new ArgumentException("Value cannot be null or empty.", nameof(webrelativePath));
            if (string.IsNullOrWhiteSpace(webrelativePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(webrelativePath));

            return new DirectoryInfo(ToFileSystemPath(webrelativePath));
        }

        #endregion
    }
}