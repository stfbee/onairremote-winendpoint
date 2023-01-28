using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace On_Air_Endpoint;

public class CurrentStatus
{
    public bool on_air { get; set; }
}

public static class Program
{
    [DllImport("MicActivityChecker.dll", EntryPoint = "?is_microphone_recording@@YAHXZ",
        CallingConvention = CallingConvention.Cdecl)]
    private static extern bool is_microphone_recording();

    private const string Url = "http://+:4200/";

    private static async Task HandleIncomingConnections(HttpListener listener)
    {
        var runServer = true;

        while (runServer)
        {
            // Ждём подключения
            var ctx = await listener.GetContextAsync();

            // Если приходит пост на /shutdown, то останавливаем сервак
            if (ctx.Request is { HttpMethod: "POST", Url.AbsolutePath: "/shutdown" })
            {
                Console.WriteLine("Shutdown requested");
                runServer = false;
            }

            // Получаем статус микрофона
            var r = new CurrentStatus
            {
                on_air = is_microphone_recording(),
            };
            // Сериализуем его в жсон
            var json = JsonSerializer.Serialize(r);
            // Отвечаем клиенту
            var data = Encoding.UTF8.GetBytes(json);
            var resp = ctx.Response;
            resp.ContentType = "application/json";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            // Write out to the response stream (asynchronously), then close it
            await resp.OutputStream.WriteAsync(data);
            resp.Close();
        }
    }

    public static void Main()
    {
        // Создаем сервер
        var listener = new HttpListener();
        listener.Prefixes.Add(Url);
        listener.Start();
        Console.WriteLine("Listening on *:4200");

        // Ловим запросеки
        var listenTask = HandleIncomingConnections(listener);
        listenTask.GetAwaiter().GetResult();

        // Если сервер закончил слушать запросеки, то останавливаемся
        listener.Close();
    }
}