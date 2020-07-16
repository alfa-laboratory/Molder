using System;
using System.Collections.Generic;
using System.Text;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Models;
using EvidentInstruction.Models.inerfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace EvidentInstruction.Tests
{
    public class TextFileTests
    {
        //FileProvider AppendAllText return true
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
        //FileProvider AppendAllText return false;
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

        //FileProvider Create return true
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

        //FileProvider Create return false
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
        //FileProvider WriteAllText return true
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

        //FileProvider WriteAllText return false
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
        // FileProvider CheckFileExtension return false
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
            action.Should().Throw<FileExtensionException>().WithMessage("Файл " + file.Filename + " не является текстовым файлом");
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
            action.Should().Throw<NoFileNameException>().WithMessage("Имя файла отсутствует");
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
            action.Should().Throw<NoFileNameException>().WithMessage("Имя файла отсутствует");
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
    }
}
