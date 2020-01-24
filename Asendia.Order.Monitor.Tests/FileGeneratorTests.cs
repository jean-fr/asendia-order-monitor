using Moq;
using NLog;
using NUnit.Framework;
using System.IO;

namespace Asendia.Order.Monitor.Tests
{
    public class FileGeneratorTests
    {
        private IFileGenerator _fileGenerator;
        private string _rootDir = @"C:\OrderXMLGen";
        private string _sourceDir;
        private string _outputDir;
        private const string _testFileName = "Orders.csv";

        [OneTimeSetUp]
        public void Setup()
        {
            this._sourceDir = Path.Combine(this._rootDir, "source");
            this._outputDir = Path.Combine(this._rootDir, "output");

            if (!Directory.Exists(this._sourceDir))
            {
                Directory.CreateDirectory(this._sourceDir);
            }
            if (!Directory.Exists(this._outputDir))
            {
                Directory.CreateDirectory(this._outputDir);
            }

            this._fileGenerator = new FileGenerator(new Mock<ILogger>().Object);
        }

        [OneTimeTearDown]
        public void CleanUp() {

            if (Directory.Exists(this._rootDir))
            {
                Directory.Delete(this._rootDir, true);
            }          
        }

        [Test]
        public void Generate_XML_File_Missing_Directories()
        {
            var options = new OrderOptions();
            var result = this._fileGenerator.Execute(options);
            Assert.AreEqual(-1, result);
        }
        [Test]
        public void Generate_XML_File_Missing_Source_Directory()
        {
            var options = new OrderOptions { Output = this._outputDir };
            var result = this._fileGenerator.Execute(options);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Generate_XML_File_Source_Directory_Not_Exist()
        {
            var options = new OrderOptions { Source = $"{this._sourceDir}wrong" };
            var result = this._fileGenerator.Execute(options);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Generate_XML_File_Output_Directory_Not_Exist()
        {
            var options = new OrderOptions { Output = $"{this._outputDir}wrong" };
            var result = this._fileGenerator.Execute(options);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Generate_XML_File_Missing_Output_Directory()
        {
            var options = new OrderOptions { Source = this._sourceDir };
            var result = this._fileGenerator.Execute(options);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Generate_XML_File_Missing_CSV_Files()
        {
            var options = new OrderOptions { Source = this._sourceDir, Output = this._outputDir };
            var result = this._fileGenerator.Execute(options);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Generate_XML_File_Success()
        {
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Data", _testFileName), Path.Combine(this._sourceDir, _testFileName));
            var options = new OrderOptions { Source = this._sourceDir, Output = this._outputDir };
            var result = this._fileGenerator.Execute(options);
            Assert.AreEqual(0, result);
            Assert.IsTrue(File.Exists(Path.Combine(this._outputDir, "Orders.xml")));
        }
    }
}