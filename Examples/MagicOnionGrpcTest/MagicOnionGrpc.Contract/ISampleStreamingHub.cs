using System.Threading.Tasks;
using MagicOnion;
using MessagePack;

namespace MagicOnionGrpc.Contract
{
    public interface ISampleStreamingHub : IStreamingHub<ISampleStreamingHub, ISampleStreamingReceiver>
    {
        Task JoinAsync(JoinRequest request);

        Task LeaveAsync();

        Task SendMessageAsync(string message);
    }

    public interface ISampleStreamingReceiver
    {
        void OnJoin(string name);
        void OnLeave(string name);
        void OnSendMessage(MessageResponse response);
    }


    [MessagePackObject]
    public class JoinRequest
    {
        [Key(0)]
        public string GroupName { get; set; }
        [Key(1)]
        public string UserName { get; set; }
    }

    [MessagePackObject]
    public class MessageResponse
    {
        [Key(0)]
        public string UserName { get; set; }
        [Key(1)]
        public string Message { get; set; }
    }
}