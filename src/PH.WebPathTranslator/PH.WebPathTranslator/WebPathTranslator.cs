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
        /// Initializes a new instance of the <see cref="WebPathTranslator"/> class.
        /// </summary>
        /// <param name="webRootPath">The web root path <example>c:\temp\myDir\</example>.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty or whitespace. - webRootPath</exception>
        public WebPathTranslator([NotNull] string webRootPath, [CanBeNull] ILogger<WebPathTranslator> logger = null)
        {
            if (string.IsNullOrEmpty(webRootPath) || string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentException("Value cannot be null or empty or whitespace.", nameof(webRootPath));
            }

           

            _webRootPath = webRootPath;
            _logger      = logger;
            
            _logger?.LogTrace($"Web Root Path: '{_webRootPath}'");

        }

        #region Implementation of IWebPathTranslator

    

        /// <summary>Translate a Web-Relative path to FileSystem-Path</summary>
        /// <param name="webrelativePath">Relative path <example>~/SomeFolder/</example></param>
        /// <returns>File System Path</returns>
        /// <exception cref="ArgumentException">Value cannot be null or empty or whitespace. - webrelativePath</exception>
        [NotNull]
        public string ToFileSystemPath([NotNull] string webrelativePath)
        {
            string initial = webrelativePath;
            if (string.IsNullOrEmpty(webrelativePath) || string.IsNullOrWhiteSpace(webrelativePath))
            {
                throw new ArgumentException("Value cannot be null or empty or whitespace.", nameof(webrelativePath));
            }


            if (webrelativePath.StartsWith(Root, StringComparison.InvariantCulture))
            {
                webrelativePath = webrelativePath.Replace(Root, $"{_webRootPath}{Path.DirectorySeparatorChar}");
            }

            var pt = webrelativePath.Replace("//", "/").Replace("/", $"{Path.DirectorySeparatorChar}")
                                    .Replace($"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}",
                                             $"{Path.DirectorySeparatorChar}");
           
            _logger?.LogTrace($"Web-Path '{initial}' to '{pt}'");
            return pt;

        }

        

        /// <summary>Translate a <see cref="FileInfo"/> full path to web-relative</summary>
        /// <param name="file">FileInfo file</param>
        /// <returns>Web-Relative Path</returns>
        /// <exception cref="ArgumentNullException">file</exception>
        [NotNull]
        public string ToWebRelativePath([NotNull] FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return ToWeb(file.FullName);
        }


        /// <summary>Converts a <see cref="DirectoryInfo"/> full path to web-relative.</summary>
        /// <param name="directory">DirectoryInfo directory</param>
        /// <returns>Web-Relative Path</returns>
        /// <exception cref="ArgumentNullException">directory</exception>
        [NotNull]
        public string ToWebRelativePath([NotNull] DirectoryInfo directory)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

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
        /// Return a <see cref="FileInfo" /> from its web-relative position
        /// </summary>
        /// <param name="webrelativePath">Web-Relative path <example>~/SomeFolder/SomeFile.zip</example></param>
        /// <returns>FileInfo file</returns>
        /// <exception cref="ArgumentException">Value cannot be null or empty or whitespace. - webrelativePath</exception>
        [NotNull]
        public FileInfo GetFile([NotNull] string webrelativePath)
        {
            if (string.IsNullOrEmpty(webrelativePath) || string.IsNullOrWhiteSpace(webrelativePath))
            {
                throw new ArgumentException("Value cannot be null or empty or whitespace.", nameof(webrelativePath));
            }



            var fileInfo = new FileInfo(ToFileSystemPath(webrelativePath));
            _logger?.LogTrace($"GetFile '{webrelativePath}' return '{fileInfo.FullName}' [Exists: {fileInfo.Exists}]");
            return fileInfo;
        }

       

        /// <summary>
        /// Return a <see cref="DirectoryInfo" /> from its web-relative position
        /// </summary>
        /// <param name="webrelativePath">Web-Relative path <example>~/SomeFolder/</example></param>
        /// <returns>DirectoryInfo directory</returns>
        /// <exception cref="ArgumentException">Value cannot be null or empty or whitespace. - webrelativePath</exception>
        [NotNull]
        public DirectoryInfo GetDirectory([NotNull] string webrelativePath)
        {
            if (string.IsNullOrEmpty(webrelativePath) || string.IsNullOrWhiteSpace(webrelativePath))
            {
                throw new ArgumentException("Value cannot be null or empty or whitespace.", nameof(webrelativePath));
            }


            var dir = new DirectoryInfo(ToFileSystemPath(webrelativePath));
            _logger?.LogTrace($"GetDirectory '{webrelativePath}' return '{dir.FullName}' [Exists: {dir.Exists}]");
            return dir;
        }

        #endregion
    }
}