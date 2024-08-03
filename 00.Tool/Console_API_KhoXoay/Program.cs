using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
class Program
{

    static async Task Main(string[] args)
    {

        Task api_listener = Task.Run(() => API_Listener("http://localhost:8083/"));
        Task robot_send = Task.Run(() => Robot_Send());
        

        await Task.WhenAll(api_listener, robot_send);
    }

    static async Task API_Listener(string url)
    {
        while (true)
        {
            try
            {

                using (HttpListener listener = new HttpListener())
                {
                    listener.Prefixes.Add(url);
                    listener.Start();
                    Console.WriteLine("Đang lắng nghe API tại: " + url);

                    while (true)
                    {
                        Console.WriteLine("Task_1");
                        HttpListenerContext context = await listener.GetContextAsync();
                        HttpListenerRequest request = context.Request;

                        string apiPath = request.Url.AbsolutePath.ToLower();

                        bool responseValue = false;

                        if (apiPath == "/api/original-detector")
                        {
                            responseValue = true;
                        }
                        else if (apiPath == "/api/call-location")
                        {
                            responseValue = true;
                        }
                        else if (apiPath == "/api/server-success-location")
                        {
                            responseValue = true;
                        }
                        else if (apiPath == "/check-connect")
                        {
                            responseValue = true;
                            Console.WriteLine("OK");
                        }

                        if (responseValue)
                        {
                            string responseString = responseValue.ToString();
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                            HttpListenerResponse response = context.Response;
                            response.ContentType = "text/plain";
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            response.OutputStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lắng nghe API: " + ex.Message);
            }
        }

    }
    static async Task Robot_Send()
    {
        while (true)
        {
            try
            {
                string url = "http://localhost:8082/api/robot-error/";

                using (HttpListener listener = new HttpListener())
                {
                    listener.Prefixes.Add(url);
                    listener.Start();


                    Console.WriteLine("Listening for requests...");

                    while (true)
                    {
                        HttpListenerContext context = await listener.GetContextAsync();
                        HttpListenerRequest request = context.Request;

                        // Xử lý yêu cầu từ máy khách tại đây
                        // ...

                        // Gửi phản hồi từ máy chủ
                        DateTime now = DateTime.Now;
                        string jsonData = "{ 'error': " + now + "}";
                        byte[] responseData = System.Text.Encoding.UTF8.GetBytes(jsonData);

                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        context.Response.ContentLength64 = responseData.Length;
                        context.Response.OutputStream.Write(responseData, 0, responseData.Length);
                        context.Response.OutputStream.Close();
                    }
                }
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
    }

}
