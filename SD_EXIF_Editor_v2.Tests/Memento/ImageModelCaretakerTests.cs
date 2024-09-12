using SD_EXIF_Editor_v2.Memento;
using SD_EXIF_Editor_v2.Model;

namespace SD_EXIF_Editor_v2.Tests.Memento
{
    public class ImageModelCaretakerTests
    {
        [Fact]
        public void Constructor_ShouldInitializeUndoStackWithInitialState()
        {
            // Arrange
            var imageModel = new ImageModel { RawMetadata = "initialState" };

            // Act
            var caretaker = new ImageModelCaretaker(imageModel);

            // Assert
            Assert.False(caretaker.CanUndo);
            Assert.False(caretaker.CanRedo);
            Assert.False(caretaker.HasChanged);
        }

        [Fact]
        public void OnPropertyChanging_ShouldPushMementoToUndoStack()
        {
            // Arrange
            var imageModel = new ImageModel { RawMetadata = "initialState" };
            var caretaker = new ImageModelCaretaker(imageModel);

            // Act
            imageModel.RawMetadata = "newState";

            // Assert
            Assert.True(caretaker.CanUndo);
            Assert.False(caretaker.CanRedo);
            Assert.True(caretaker.HasChanged);
        }

        [Fact]
        public void Undo_ShouldRestorePreviousState()
        {
            // Arrange
            var imageModel = new ImageModel { RawMetadata = "initialState" };
            var caretaker = new ImageModelCaretaker(imageModel);

            imageModel.RawMetadata = "newState";

            // Act
            caretaker.Undo();

            // Assert
            Assert.Equal("initialState", imageModel.RawMetadata);
            Assert.False(caretaker.CanUndo);
            Assert.True(caretaker.CanRedo);
            Assert.False(caretaker.HasChanged);
        }

        [Fact]
        public void Redo_ShouldRestoreNextState()
        {
            // Arrange
            var imageModel = new ImageModel { RawMetadata = "initialState" };
            var caretaker = new ImageModelCaretaker(imageModel);

            imageModel.RawMetadata = "newState";
            caretaker.Undo();

            // Act
            caretaker.Redo();

            // Assert
            Assert.Equal("newState", imageModel.RawMetadata);
            Assert.True(caretaker.CanUndo);
            Assert.False(caretaker.CanRedo);
            Assert.True(caretaker.HasChanged);
        }

        [Fact]
        public void CanUndo_ShouldReturnTrueWhenUndoStackHasMoreThanOneMemento()
        {
            // Arrange
            var imageModel = new ImageModel { RawMetadata = "initialState" };
            var caretaker = new ImageModelCaretaker(imageModel);

            imageModel.RawMetadata = "newState";

            // Act
            var canUndo = caretaker.CanUndo;

            // Assert
            Assert.True(canUndo);
        }

        [Fact]
        public void CanRedo_ShouldReturnTrueWhenRedoStackHasMementos()
        {
            // Arrange
            var imageModel = new ImageModel { RawMetadata = "initialState" };
            var caretaker = new ImageModelCaretaker(imageModel);

            imageModel.RawMetadata = "newState";
            caretaker.Undo();

            // Act
            var canRedo = caretaker.CanRedo;

            // Assert
            Assert.True(canRedo);
        }

        [Fact]
        public void HasChanged_ShouldReturnTrueWhenRawMetadataIsDifferentFromLastMemento()
        {
            // Arrange
            var imageModel = new ImageModel { RawMetadata = "initialState" };
            var caretaker = new ImageModelCaretaker(imageModel);

            // Act
            imageModel.RawMetadata = "newState";

            // Assert
            Assert.True(caretaker.HasChanged);
        }
    }
}
