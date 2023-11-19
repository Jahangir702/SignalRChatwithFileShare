using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.IO;

namespace R54_M13_Class_01_Work_01.Hubs
{
    public class MessageHub : Hub
    {
        static Dictionary<string, string> users = new Dictionary<string, string>();
        private readonly IWebHostEnvironment env;
        public MessageHub(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public async override Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("connection","You are connected");
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task JoinAsync(string username)
        {
            users.Add(Context.ConnectionId, username);
            await Clients.All.SendAsync("userlist", users.Values.ToList());
        }
        public async Task SendAllAsync(string msg)
        {
           // await Clients.All.SendAsync("message", msg);
          
           await Clients.All.SendAsync("message", new { sender = users[Context.ConnectionId], message=msg });
        }
        public async Task UploadAsync(FileData data)
        {
            string ext = Path.GetExtension(data.Filename);
            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+ext;
            string savePath = Path.Combine(this.env.WebRootPath, "Uploads", fileName);
            data.Content = data.Content.Substring(data.Content.LastIndexOf(',') + 1);
            string converted = data.Content.Replace('-', '+');
            converted = converted.Replace('_', '/');
            byte[] buffer = Convert.FromBase64String(converted);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
            await Clients.All.SendAsync("uploaded", new { sender = users[Context.ConnectionId], File=fileName, Type=ext });

        }
    }
    public class FileData
    {
        public string Filename { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
    

}
