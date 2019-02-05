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

            Assert.Equal(@"c:\temp\Folder1\Folder2", result.FullName);
        }






        private IWebPathTranslator Provide(string rootPath)
        {
            return new WebPathTranslator(rootPath, new FakeLogger(Output));
        }


    }
}
