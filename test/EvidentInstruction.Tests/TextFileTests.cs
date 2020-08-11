using System;
using System.Diagnostics.CodeAnalysis;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Models;
using EvidentInstruction.Models.Directory.Interfaces;
using EvidentInstruction.Models.Profider.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace EvidentInstruction.Tests
{
    [ExcludeFromCodeCoverage]
    public class TextFileTests
    {
        [Fact]
        public void IsExist_NULLPath_ReturnTrue()
        {
            // Act
            var file = new TextFile()
            {
                Filename = Guid.NewGuid().ToString(),
                Path = null
            };
            var mockUserDir = new Mock<IDirectory>();
            var mockFileProvider = new Mock<IFileProvider>();
            var mockPathProvider = new Mock<IPathProvider>();

            mockUserDir.Setup(f => f.Get()).Returns(It.IsAny<string>());
            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);

            file.UserDirectory = mockUserDir.Object;
            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;

            // Arrange

            bool result = file.IsExist(file.Filename, file.Path);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsExist_CorrectNameAndPath_ReturnTrue()
        {
            // Act
            var file = new TextFile()
            {
                Filename = "tets.txt",
                Path = "Correct path"
            };

            var mockUserDir = new Mock<IDirectory>();
            var mockFileProvider = new Mock<IFileProvider>();
            var mockPathProvider = new Mock<IPathProvider>();

            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;
            
            // Arrange
            bool result = file.IsExist(file.Filename, file.Path);

            // Assert
            result.Should().BeTrue();
        }
        [Fact]
        public void IsExist_CorrectNameAndPathButFileDoesNotExist_ReturnFalse()
        {
            // Act
            var file = new TextFile()
            {
                Filename = "tets.txt",
                Path = "Correct path"
            };

            var mockUserDir = new Mock<IDirectory>();
            var mockFileProvider = new Mock<IFileProvider>();
            var mockPathProvider = new Mock<IPathProvider>();

            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;

            // Arrange
            bool result = file.IsExist(file.Filename, file.Path);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Create_NULLPath_ReturnTrue()
        {
            // Act
            var file = new TextFile()
            {
                Filename = "test.txt",
                Path = null,
                Content = "just some content"
            };

            var mockUserDirectoryProvider = new Mock<IDirectory>();
            var mockFileProvider = new Mock<IFileProvider>();

            mockUserDirectoryProvider.Setup(f => f.Get()).Returns("This is UserDirectory");
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            mockFileProvider.Setup(f => f.AppendAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            file.UserDirectory = mockUserDirectoryProvider.Object;
            file.FileProvider = mockFileProvider.Object;
            // Assert
            file.Create(file.Filename, file.Path, file.Content).Should().BeTrue();
        }

        [Fact]
        public void Create_CorrectNameAndPathAndContent_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Content = "sdjfihsidfusifhs",
                Path = "just path"
            };

            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            mockFileProvider.Setup(f => f.AppendAllText(file.Filename, file.Path, file.Content)).Returns(true);
            file.FileProvider = mockFileProvider.Object;
            bool result = file.Create(file.Filename, file.Path, file.Content);
            result.Should().BeTrue();
        }

        [Fact]
        public void Create_CorrectNameAndPathAndContent_IncorrectAppendAllText_ReturnFalse()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Content = "Its my file feature tests",
                Path = "just path"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            mockFileProvider.Setup(f => f.AppendAllText(file.Filename, file.Path, file.Content)).Returns(false);
            file.FileProvider = mockFileProvider.Object;
            bool result = file.Create(file.Filename, file.Path, file.Content);
            result.Should().BeFalse();
        }

        [Fact]
        public void Create_CorrectNameAndPathNullContent_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Content = null,
                Path = "just path"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            mockFileProvider.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            file.FileProvider = mockFileProvider.Object;
            bool result = file.Create(file.Filename, file.Path, file.Content);
            result.Should().BeTrue();
        }

        [Fact]
        public void Create_CorrectNameAndPathNullContent_IncorrectCreateMethod_ReturnFalse()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Content = null,
                Path = "just path"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            mockFileProvider.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            file.FileProvider = mockFileProvider.Object;
            bool result = file.Create(file.Filename, file.Path, file.Content);
            result.Should().BeFalse();
        }

        [Fact]
        public void Create_CorrectNameAndPathAndContent_PathIsDoesNotExist_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Content = "some content",
                Path = "just path"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            file.FileProvider = mockFileProvider.Object;
            bool result = file.Create(file.Filename, file.Path, file.Content);
            result.Should().BeTrue();
        }

        [Fact]
        public void Create_CorrectNameAndPathAndContent_PathIsExist_WriteAllTextReturnFalse_ReturnFalse()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Content = "some content",
                Path = "just path"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            file.FileProvider = mockFileProvider.Object;
            bool result = file.Create(file.Filename, file.Path, file.Content);
            result.Should().BeFalse();
        }

        [Fact]
        public void Create_CorrectNameAndPathAndContent_ExtensionIsNotTXT_ReturnFileExtensionException()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Content = "some content",
                Path = "just path"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(It.IsAny<string>())).Returns(false);
            file.FileProvider = mockFileProvider.Object;
            Action action = () => file.Create(file.Filename, file.Path, file.Content);
            action.Should().Throw<FileExtensionException>().WithMessage($"The file \"{file.Filename}\" is not a text file");
        }

        [Fact]
        public void Create_NULLNameCorrectPathAndContent_ReturnNoFileNameException()
        {
            var file = new TextFile()
            {
                Filename = null,
                Content = "some content",
                Path = "just path"
            };
            Action action = () => file.Create(file.Filename, file.Path, file.Content);
            action.Should().Throw<NoFileNameException>().WithMessage("CREATE: FileName is missing");
        }

        [Fact]
        public void Delete_NullFileName_ReturnNoFileNameException()
        {
            var file = new TextFile()
            {
                Filename = null,
                Path = "just path"
            };
            var mockPathProvider = new Mock<IPathProvider>();
            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            file.PathProvider = mockPathProvider.Object;
            Action action = () => file.Delete(file.Filename, file.Path);
            action.Should().Throw<NoFileNameException>().WithMessage("DELETE: FileName is missing");
        }

        [Fact]
        public void Delete_FileNameAndPathIsCorrect_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Path = "just path"
            };
            var mockPathProvider = new Mock<IPathProvider>();
            var mockFileProvider = new Mock<IFileProvider>();
            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Delete(It.IsAny<string>())).Returns(true);
            file.PathProvider = mockPathProvider.Object;
            file.FileProvider = mockFileProvider.Object;
            bool result = file.Delete(file.Filename, file.Path);
            result.Should().BeTrue();
        }

        [Fact]
        public void Delete_FileDoesNotExist_ReturnFileExistException()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Path = "just path"
            };
            var mockPathProvider = new Mock<IPathProvider>();
            var mockFileProvider = new Mock<IFileProvider>();
            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            file.PathProvider = mockPathProvider.Object;
            file.FileProvider = mockFileProvider.Object;
            Action action = () => file.Delete(file.Filename, file.Path);
            action.Should().Throw<FileExistException>();
        }

        [Fact]
        public void Delete_NULLPath_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Path = null
            };
            var mockUserDirectory = new Mock<IDirectory>();
            var mockPathProvider = new Mock<IPathProvider>();
            var mockFileProvider = new Mock<IFileProvider>();
            mockUserDirectory.Setup(f => f.Get()).Returns("This is User Directory");
            mockPathProvider.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.Delete(It.IsAny<string>())).Returns(true);
            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;
            file.UserDirectory = mockUserDirectory.Object;
            bool result = file.Delete(file.Filename, file.Path);
            result.Should().BeTrue();
        }

        [Fact]
        public void Delete_NULLPath_ReturnFileExistException()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Path = null
            };
            var mockFile = new Mock<IFileProvider>();
            var mockPath = new Mock<IPathProvider>();
            var mockUserDir = new Mock<IDirectory>();
            mockUserDir.Setup(f => f.Get()).Returns(It.IsAny<string>());
            mockPath.Setup(f => f.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            mockFile.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);
            file.UserDirectory = mockUserDir.Object;
            file.PathProvider = mockPath.Object;
            file.FileProvider = mockFile.Object;
            Action action = () => file.Delete(file.Filename, file.Path);
            action.Should().Throw<FileExistException>();

        }

        [Theory]
        [InlineData("filename", "path")]
        public void Delete_FileIsNotExist_ReturnFileExistException(string filename, string path)
        {
            // Act
            var file = new TextFile();

            var mockPathProvider = new Mock<IPathProvider>();
            var mockFileProvider = new Mock<IFileProvider>();

            mockPathProvider.Setup(p => p.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid().ToString());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);

            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;

            // Arrange 
            Action action = () => file.Delete(filename, path);

            // Assert
            action.Should().Throw<FileExistException>()
                .And.Message.Contains($"Файла \"{filename}\" в директории \"{path}\" не существует");
        }

        [Fact]
        public void Download_NULLFilename_ReturnNoFileNameException()
        {
            var file = new TextFile()
            {
                Filename = null,
                Path = "just path",
                Url = "just URL"
            };
            Action acvtion = () => file.DownloadFile(file.Url, file.Filename, file.Path);
            acvtion.Should().Throw<ArgumentException>().WithMessage("DOWNLOAD: FileName is missing");
        }

        [Fact]
        public void Download_CorrectFileNameAndURL_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Path = "just path",
                Url = "just URL"
            };
            var mockWebProvider = new Mock<IWebProvider>();
            var mockFileProvider = new Mock<IFileProvider>();
            mockWebProvider.Setup(f => f.Download(file.Url, file.Path, file.Filename)).Returns(true);
            mockFileProvider.Setup(f => f.CheckFileExtension(file.Filename)).Returns(true);
            file.FileProvider = mockFileProvider.Object;
            file.WebProvider = mockWebProvider.Object;
            bool result = file.DownloadFile(file.Url, file.Filename, file.Path);
            result.Should().BeTrue();
        }

        [Fact]
        public void Download_WebClientDownloadMethodError_ReturnFalse()
        {
            var file = new TextFile()
            {
                Filename = "test.txt",
                Path = "just path",
                Url = "just URL"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            var mockWebProvider = new Mock<IWebProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(file.Filename)).Returns(true);
            mockWebProvider.Setup(f => f.Download(file.Url, file.Path, file.Filename)).Returns(false);
            file.FileProvider = mockFileProvider.Object;
            file.WebProvider = mockWebProvider.Object;
            bool result = file.DownloadFile(file.Url, file.Filename, file.Path);
            result.Should().BeFalse();
        }

        [Fact]
        public void Download_IncorrectFileExtension_ReturnValidFileNameException()
        {

            var file = new TextFile()
            {
                Filename = "test.jpg",
                Path = "just path",
                Url = "just URL"
            };
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(f => f.CheckFileExtension(file.Filename)).Returns(false);
            file.FileProvider = mockFileProvider.Object;
            Action action = () => file.DownloadFile(file.Url, file.Filename, file.Path);
            action.Should().Throw<ValidFileNameException>()
                .WithMessage($"Check that the file \"{file.Filename}\" has a .txt extension");
        }

        [Fact]
        public void Download_NULLPathToSave_ReturnTrue()
        {
            var file = new TextFile()
            {
                Filename = "test.jpg",
                Path = null,
                Url = "just URL"
            };
            var mockUserDir = new Mock<IDirectory>();
            var mockFileProvider = new Mock<IFileProvider>();
            var mockWebProvider = new Mock<IWebProvider>();
            mockWebProvider.Setup(f=>f.Download(file.Url, file.Path, file.Filename)).Returns(true);
            mockUserDir.Setup(f => f.Get()).Returns(It.IsAny<string>());
            mockFileProvider.Setup(f => f.CheckFileExtension(file.Filename)).Returns(true);
            file.FileProvider = mockFileProvider.Object;
            file.UserDirectory = mockUserDir.Object;
            file.WebProvider = mockWebProvider.Object;
           bool result = file.DownloadFile(file.Url, file.Filename, file.Path);
           result.Should().BeTrue();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("<a></a>")]
        [InlineData("1")]
        public void GetContent_CorrectFileData_ReturnContent(string testString)
        {
            // Act
            var file = new TextFile();

            var mockPathProvider = new Mock<IPathProvider>();
            var mockFileProvider = new Mock<IFileProvider>();

            mockPathProvider.Setup(p => p.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid().ToString());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.ReadAllText(It.IsAny<string>(), It.IsAny<string>())).Returns(testString);

            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;

            // Arrange 
            var content = file.GetContent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            // Assert
            content.Should().Equals(testString);
        }


        [Theory]
        [InlineData("test")]
        [InlineData("<a></a>")]
        [InlineData("1")]
        public void GetContent_CorrectFileDataWithoutPath_ReturnContent(string testString)
        {
            // Act
            var file = new TextFile();

            var mockPathProvider = new Mock<IPathProvider>();
            var mockFileProvider = new Mock<IFileProvider>();

            mockPathProvider.Setup(p => p.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid().ToString());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(true);
            mockFileProvider.Setup(f => f.ReadAllText(It.IsAny<string>(), It.IsAny<string>())).Returns(testString);

            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;

            // Arrange 
            var content = file.GetContent(Guid.NewGuid().ToString(), null);

            // Assert
            content.Should().Equals(testString);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GetContent_EmptyFileName_ReturnNoFileNameException(string filename)
        {
            // Act
            var file = new TextFile();

            // Arrange 
            Action action = () => file.GetContent(filename, Guid.NewGuid().ToString());

            // Assert
            action.Should().Throw<NoFileNameException>()
                .WithMessage($"GET CONTENT: FileName is missing");
        }

        [Theory]
        [InlineData("filename", "path")]
        public void GetContent_FileIsNotExist_ReturnFileExistException(string filename, string path)
        {
            // Act
            var file = new TextFile();

            var mockPathProvider = new Mock<IPathProvider>();
            var mockFileProvider = new Mock<IFileProvider>();

            mockPathProvider.Setup(p => p.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid().ToString());
            mockFileProvider.Setup(f => f.Exist(It.IsAny<string>())).Returns(false);

            file.FileProvider = mockFileProvider.Object;
            file.PathProvider = mockPathProvider.Object;

            // Arrange 
            Action action = () => file.GetContent(filename, path);

            // Assert
            action.Should().Throw<FileExistException>()
                .And.Message.Contains($"Файла \"{filename}\" в директории \"{path}\" не существует");
        }

        [Fact]
        public void Dispose_ReturnNotThowException()
        {
            var file = new TextFile();
            Action action = () => file.Dispose();
            action.Should().NotThrow<Exception>();
        }
    }
}
