using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class LocalServer : MonoBehaviour
{
    private Thread HTTPListenerThread;
    private static HttpListener httpListener = new HttpListener();
    public string PORT_HOST = "http://127.0.0.1:8084/";
    public bool EnableLocalDebug = false;
    void Start()
    {
        // Start HTTP Server background thread 		
        HTTPListenerThread = new Thread(new ThreadStart(ListenTraces));
        HTTPListenerThread.IsBackground = true;
        HTTPListenerThread.Start();

    }


    public void ListenTraces()
    {
        httpListener.Prefixes.Add(PORT_HOST);
        try
        {
            httpListener.Start();
        }
        catch (HttpListenerException hlex)
        {
            Debug.Log("Can't start the agent to listen transaction" + hlex);
            return;
        }
        //Debug.Log("Now ready to receive traces...");
        while (true)
        {
            if (httpListener.IsListening)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                ProcessRequest(request);
                ProcessResponse(response);
            }
        }
    }

    private void ProcessRequest(HttpListenerRequest request)
    {
        var body = WebUtility.UrlDecode(new StreamReader(request.InputStream).ReadToEnd());
        // Qudamah Quboa
        if (EnableLocalDebug)
        {
            Debug.Log("QServer accepted msg:" + body.Substring(body.IndexOf("&doc=") + 5));
        }
    }

    private void ProcessResponse(HttpListenerResponse response)
    {
        byte[] b = Encoding.UTF8.GetBytes("ACK");
        response.StatusCode = 200;
        response.KeepAlive = false;
        response.ContentLength64 = b.Length;

        var output = response.OutputStream;
        output.Write(b, 0, b.Length);
        response.Close();
        output.Close();
    }
}