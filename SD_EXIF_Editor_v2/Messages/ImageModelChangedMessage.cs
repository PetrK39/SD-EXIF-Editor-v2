using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SD_EXIF_Editor_v2.Messages
{
    public class ImageModelChangedMessage(bool value) : ValueChangedMessage<bool>(value) { }
}
