using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Drawing;
using System.Drawing.Imaging;
using Taka.App.Deliverer.Application.Services;

namespace Taka.App.Deliverer.Tests.Services
{
    public class LocalFileStorageServiceTests
    {
        private readonly LocalFileStorageService _storageService;
        private readonly IConfiguration _configuration;
        private readonly string _tempDirectory;

        public LocalFileStorageServiceTests()
        {
            _configuration = Substitute.For<IConfiguration>();
            _storageService = new LocalFileStorageService(_configuration);
            _tempDirectory = Path.GetTempPath();
        }

        [Fact]
        public async Task UploadFileAsync_ShouldSaveFileToConfiguredLocationAndReturnPath()
        {
            // Arrange
            string expectedLocalPath = _tempDirectory;
            string expectedFileName = "testfile.png";
            string expectedFullPath = Path.Combine(expectedLocalPath, expectedFileName);
            SetupUploadTest(expectedFileName);
            byte[] fileData = new byte[] { 0x1, 0x2, 0x3, 0x4 };

            _configuration["LocalStorage"].Returns(expectedLocalPath);

            // Act
            string resultPath = await _storageService.UploadFileAsync(fileData, expectedFileName);

            // Assert
            resultPath.Should().Be(expectedFullPath);
            File.Exists(resultPath).Should().BeTrue();
        }

        [Fact]
        public async Task UploadFileAsync_ShouldThrowApplicationException_IfLocalStorageConfigIsMissing()
        {
            // Arrange
            string expectedFileName = "nofile.png";
            byte[] fileData = new byte[] { 0x1, 0x2, 0x3, 0x4 };
            _configuration["LocalStorage"].Returns((string)null);

            // Act
            Func<Task> act = async () => await _storageService.UploadFileAsync(fileData, expectedFileName);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("LocalStorage not *");
        }

        [Fact]
        public async Task UploadFileAsync_ShouldCreateNewFileOnDisk()
        {
            // Arrange
            string testPath = Path.GetTempPath();
            string testFileName = "uniquefile.png";
            string fullPath = Path.Combine(testPath, testFileName);
            byte[] fileData = new byte[] { 0x1, 0x2, 0x3, 0x4 };

            _configuration["LocalStorage"].Returns(testPath);

            // Act
            string resultPath = await _storageService.UploadFileAsync(fileData, testFileName);

            // Assert
            resultPath.Should().Be(fullPath);
            File.Exists(resultPath).Should().BeTrue();

            // Cleanup
            File.Delete(resultPath);
        }

        private void SetupUploadTest(string fileName)
        {
            var imagePath = Path.Combine(_tempDirectory, fileName);

            if (!File.Exists(imagePath))
            {
                using (var bitmap = new Bitmap(100, 100))
                {
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.FillRectangle(Brushes.Azure, 0, 0, 100, 100);
                    }

                    bitmap.Save(imagePath, ImageFormat.Png);
                }
            }
        }

    }
}
