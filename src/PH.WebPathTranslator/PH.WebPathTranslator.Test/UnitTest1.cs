using System;
using System.IO;
using PH.ITestOutputHelperExtensions;
using Xunit;
using Xunit.Abstractions;

namespace PH.WebPathTranslator.Test
{
    public class UnitTest1
    {
        protected ITestOutputHelper Output;

        public UnitTest1(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public void TestWithNullLogger()
        {
            var svc = new WebPathTranslator(@"c:\temp");

            var result = svc.ToFileSystemPath("~/Folder1/Folder2");

            Assert.Equal(@"c:\temp\Folder1\Folder2", result);
        }

        [Fact]
        public void Test_ToFileSystemPath()
        {
            var svc = Provide(@"c:\temp");

            var result = svc.ToFileSystemPath("~/Folder1/Folder2");

            Assert.Equal(@"c:\temp\Folder1\Folder2", result);
        }


        [Fact]
        public void Test_GetDirectory()
        {
            var svc = Provide(@"c:\temp");

            var result = svc.GetDirectory("~/Folder1/Folder2");

            var webRelativePath = svc.ToWebRelativePath(result);

            Assert.Equal(@"c:\temp\Folder1\Folder2", result.FullName);
            Assert.Equal(@"~/Folder1/Folder2", webRelativePath);
        }

        [Fact]
        public void TestExceptions()
        {
            var       realSvc = Provide(@"c:\temp");

            Exception ex0     = null;
            try
            {
                var svc = Provide("");
            }
            catch (Exception e)
            {
                ex0 = e;
            }

            Exception ex1 = null;
            try
            {
                var fake = realSvc.GetDirectory("");
            }
            catch (Exception e)
            {
                ex1 = e;
            }

            Exception ex2 = null;
            try
            {
                var fake = realSvc.GetFile("");
            }
            catch (Exception e)
            {
                ex2 = e;
            }
            Exception ex3 = null;
            try
            {
                var fake = realSvc.ToFileSystemPath("");
            }
            catch (Exception e)
            {
                ex3 = e;
            }

            Exception ex4 = null;
            try
            {
                FileInfo f    = null;
                var      fake = realSvc.ToWebRelativePath(f);
            }
            catch (Exception e)
            {
                ex4 = e;
            }

            Exception ex5 = null;
            try
            {
                DirectoryInfo d    = null;
                var      fake = realSvc.ToWebRelativePath(d);
            }
            catch (Exception e)
            {
                ex5 = e;
            }
            Assert.NotNull(ex0);
            Assert.NotNull(ex1);
            Assert.NotNull(ex2);
            Assert.NotNull(ex3);
            Assert.NotNull(ex4);
            Assert.NotNull(ex5);
        }

        [Fact]
        public void TestGetFiles()
        {
            var svc = Provide(@"c:\temp");

            var dir = svc.GetDirectory("~/Folder1/Folder2");
            if (!dir.Exists)
            {
                dir.Create();
            }

            System.IO.File.WriteAllText($"{dir.FullName}{Path.DirectorySeparatorChar}fake.txt", "some content");

            var file = svc.GetFile("~/Folder1/Folder2/fake.txt");

            var webPath = svc.ToWebRelativePath(file);
            var toFs    = svc.ToFileSystemPath(webPath);

            Assert.True(file.Exists);
            Assert.Equal("~/Folder1/Folder2/fake.txt", webPath);
            Assert.Equal($"{dir.FullName}{Path.DirectorySeparatorChar}fake.txt", toFs);


        }


        private IWebPathTranslator Provide(string rootPath)
        {
            var logger = Output.GetTestOutputLogger<WebPathTranslator>();
            return new WebPathTranslator(rootPath, logger);
        }


    }
}
