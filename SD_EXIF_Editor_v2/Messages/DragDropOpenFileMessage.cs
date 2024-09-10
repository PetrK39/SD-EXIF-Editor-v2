using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace SD_EXIF_Editor_v2.Messages
{
    public class DragDropOpenFileMessage(Uri value) : ValueChangedMessage<Uri>(value) { }
}
