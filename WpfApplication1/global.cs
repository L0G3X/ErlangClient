using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;

public static class Global
{
    public enum Type
    {
        Join = 1,
        Login,
        Logout,
        GetPoint,
        SendPoint,
    };

    private static string GetURL(Type type)
    {
#if DEBUG
        var url = "http://localhost:6064/";
#else
        var url = "http://localhost:6064/";
#endif
        switch (type)
        {
            case Type.Join:
                return $"{url}user/join";
            case Type.Login:
                return $"{url}user/login";
            case Type.Logout:
                return $"{url}user/logout";
            case Type.GetPoint:
                return $"{url}get/point";
            case Type.SendPoint:
                return $"{url}send/point";
        }
        return "ERROR";
    }

    public static string SendData(string data, Type type)
    {
        if (GetURL(type).Equals("ERROR"))
        {
            return "ERROR";
        }

        if (!type.Equals(Type.Join))
            data += $"&guid={guid}";

        var sendData = Encoding.UTF8.GetBytes(data);
        var httpWebRequest = WebRequest.Create(GetURL(type)) as HttpWebRequest;
        httpWebRequest.ContentType = "application/x-www-form-urlencoded;    charset=UTF-8";
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentLength = sendData.Length;

        using (var requestStream = httpWebRequest.GetRequestStream())
        {
            requestStream.Write(sendData, 0, sendData.Length);
        }

        using (var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
        {
            return ParseResponse(streamReader.ReadToEnd(), type);
        }
    }

    private static string ParseResponse(string response, Global.Type type)
    {
        // With Newtonsoft.Json
        var jorp = JObject.Parse(response);
        if (jorp.HasValues)
        {
            if (!jorp["result"].ToString().Equals("fail") && !jorp["result"].ToString().Equals("update"))
            {
                switch (type)
                {
                    case Type.Join:
                    case Type.Logout:
                    case Type.SendPoint:
                        return jorp["result"].ToString();
                    case Type.Login:
                        return jorp["session_key"].ToString();
                    case Type.GetPoint:
                        return jorp["point"].ToString();
                }
            }
        }
        return "ERROR";
    }

    private const string guid = "DDE75E9B-F758-4F2B-9801-04250959504A";
}