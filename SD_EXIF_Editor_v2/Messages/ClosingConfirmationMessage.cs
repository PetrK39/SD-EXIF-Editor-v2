using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Messages
{
    public class ClosingConfirmationMessage : RequestMessage<bool>
    {
        public TaskCompletionSource<bool> ResponseCompletionSource { get; } = new TaskCompletionSource<bool>();
    }
}
