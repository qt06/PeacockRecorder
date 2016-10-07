using System;
using System.Text;
using System.Runtime.InteropServices;


namespace ZDCloudAPISharp
{
    class ZDCloudAPI
    {
        static ZDCloudAPI()
        {
            string commonbin = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "zdsr\\common\\bin\\");
            string pathlist = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Process).Trim(';');
            Environment.SetEnvironmentVariable("Path", commonbin + ";" + pathlist, EnvironmentVariableTarget.Process);
        }

        /// <summary>
        /// 回调方法用于之多云主动通知应用
        /// </summary>
        /// <returns>void</returns>
        /// <param name="type">[out] int type: 0:登录 1:退出 2:注销</param>
        public delegate void ZDCloudAPICallBack(int type);

        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <returns>int类型，0：成功初始化；1：appkey错误；2：seckey错误；其他数字，其他错误</returns>
        /// <param name="appkey">应用的appkey</param>
        /// <param name="seckey">应用的seckey</param>
        /// <param name="zdccb">回调方法，当之多云有相应消息的时候主动通知应用，null则不接受回调</param>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern int Initial(string appkey, string seckey, ZDCloudAPICallBack zdccb);

        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <returns>int类型，0：成功初始化；1：appkey错误；2：seckey错误；其他数字，其他错误</returns>
        /// <param name="appkey">应用的appkey</param>
        /// <param name="seckey">应用的seckey</param>
        public static int Initial(string appkey, string seckey)
        {
            return Initial(appkey, seckey, null);
        }

        /// <summary>
        ///释放接口， 在应用退出前调用
        /// </summary>
        /// <returns>void</returns>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern void UnInitial();

        /// <summary>
        /// 获取用户登陆信息
        /// </summary>
        /// <returns>bool,true:成功，uid、nick、zdck会复制到相应变量中；false:失败，用户未登录</returns>
        /// <param name="uid">out 用户id 缓冲区32</param>
        /// <param name="nick">out 用户昵称 缓冲区128</param>
        /// <param name="zdck">out 用户授权key 缓冲区256</param>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetLoginInfo(StringBuilder uid, StringBuilder nick, StringBuilder zdck);

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <returns>void,之多云会尝试自动登陆</returns>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern void Login();

        /// <summary>
        /// 朗读文本
        /// </summary>
        /// <returns>void</returns>
        /// <param name="Text">需要朗读的文本字符串</param>
        /// <param name="bspec">bool类型，是否异步朗读</param>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern void Speak(string Text, [MarshalAs(UnmanagedType.Bool)] bool bspec);

        /// <summary>
        /// 朗读文本
        /// </summary>
        /// <returns>void</returns>
        /// <param name="Text">需要朗读的文本字符串</param>
        public static void Speak(string Text)
        {
            Speak(Text, true);
        }

        /// <summary>
        /// 停止当前朗读
        /// </summary>
        /// <returns>void</returns>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern void StopSpeak();


        /// <summary>
        /// 根据url抓取远程数据
        /// </summary>
        /// <returns>返回远程数据</returns>
        /// <param name="url">目标地址最大2048</param>
        /// <param name="data">post方法要传递的信息 空字符串则使用GET方法</param>
        /// <param name="charset">指定编码,空字符串则按当前默认代码页转换</param>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetUrlContent(string url, string data, string charset);

        /// <summary>
        /// 根据url抓取远程数据
        /// </summary>
        /// <returns>返回远程数据</returns>
        /// <param name="url">目标地址</param>
        /// <param name="data">post方法要传递的信息</param>
        /// <param name="charset">指定编码</param>
        public static string GetUrlContentEx(string url, string data, string charset)
        {
            IntPtr ct = GetUrlContent(url, data, charset);
            string str = Marshal.PtrToStringAnsi(ct);
            return str;
        }

        /// <summary>
        /// 根据url抓取远程数据，使用POST方法，自动侦测编码
        /// </summary>
        /// <returns>返回远程数据</returns>
        /// <param name="url">目标地址</param>
        /// <param name="data">post方法要传递的信息</param>
        public static string GetUrlContentEx(string url, string data)
        {
            IntPtr ct = GetUrlContent(url, data, "");
            string str = Marshal.PtrToStringAnsi(ct);
            return str;
        }


        /// <summary>
        /// 根据url抓取远程数据，使用GET方法，自动侦测编码
        /// </summary>
        /// <returns>返回远程数据</returns>
        /// <param name="url">目标地址</param>
        public static string GetUrlContentEx(string url)
        {
            IntPtr ct = GetUrlContent(url, "", "");
            string str = Marshal.PtrToStringAnsi(ct);
            return str;
        }

        /// <summary>
        /// 新建下载任务
        /// </summary>
        /// <returns>void</returns>
        /// <param name="url">下载的url</param>
        /// <param name="FileName">默认保存的文件名</param>
        /// <param name="Path">默认保存路径，若留空则使用之多云默认保存路径</param>
        /// <param name="bShowWindow">bool,是否显示新建下载对话框，true：显示新建对话框；false： 不显示新建对话框</param>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern void NewDownload(string url, string FileName, string Path, [MarshalAs(UnmanagedType.Bool)] bool bShowWindow);

        /// <summary>
        /// 新建下载任务
        /// </summary>
        /// <returns>void</returns>
        /// <param name="url">下载的url</param>
        /// <param name="FileName">默认保存的文件名</param>
        /// <param name="Path">默认保存路径，若留空则使用之多云默认保存路径</param>
        public static void NewDownload(string url, string FileName, string Path)
        {
            NewDownload(url, FileName, Path, false);
        }

        /// <summary>
        /// 新建下载任务
        /// </summary>
        /// <returns>void</returns>
        /// <param name="url">下载的url</param>
        /// <param name="FileName">默认保存的文件名</param>
        public static void NewDownload(string url, string FileName)
        {
            NewDownload(url, FileName, "", false);
        }

        /// <summary>
        /// 新建下载任务
        /// </summary>
        /// <returns>void</returns>
        /// <param name="url">下载的url</param>
        public static void NewDownload(string url)
        {
            NewDownload(url, "", "", false);
        }

        /// <summary>
        /// 显示之多云下载管理器
        /// </summary>
        /// <returns>void</returns>
        [DllImport("ZDCloudAPI.dll", CharSet = CharSet.Ansi)]
        public static extern void ShowDownloadMgr();
    }
}
