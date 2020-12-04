using System;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using MagicOnionGrpc.Contract;

namespace MagicOnionGrpc.Server.GrpcServices
{
    public class SampleStreamingHub : StreamingHubBase<ISampleStreamingHub, ISampleStreamingReceiver>, ISampleStreamingHub
    {
        private IGroup _group;
        private string _name;

        public async Task JoinAsync(JoinRequest request)
        {
            _group = await Group.AddAsync(request.GroupName);
            _name = request.UserName;
            Broadcast(_group).OnJoin(request.UserName);
            Console.WriteLine($"{_group.GroupName} {_name} have joined");
            Console.WriteLine($"{_group.GroupName} have total {await _group.GetMemberCountAsync()} members");
        }

        public async Task LeaveAsync()
        {
            Broadcast(_group).OnLeave(_name);
            await _group.RemoveAsync(Context);
            Console.WriteLine($"{_group.GroupName} {_name} just have left");
            Console.WriteLine($"{_group.GroupName} have total {await _group.GetMemberCountAsync()} members");
        }

        public async Task SendMessageAsync(string message)
        {
            var response = new MessageResponse()
            {
                UserName = _name,
                Message = message
            };
            Broadcast(_group).OnSendMessage(response);
            await Task.CompletedTask;
        }
    }
}
