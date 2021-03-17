using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class TwitchController : MonoBehaviour
{
    private static TwitchController _instance;

    public static TwitchController Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }

    private string server = "irc.chat.twitch.tv";
    private int port = 6667;
    public static string GAME_COMMAND = "!creature ";

    private string buffer = string.Empty;
    private bool stopThreads;
    private Queue<string> commandQueue = new Queue<string>();
    private List<string> recievedMsgs = new List<string>();
    private Thread inProc, outProc;

    private string username;
    private string channelName;
    private string password;

    public bool isConnected { get; private set; }

    public void Login(string user, string channel, string pass)
    {
        username = user;
        int startIndex = channel.IndexOf(".tv/") == -1 ? 0 : channel.IndexOf(".tv/") + 4;
        channel = channel.TrimEnd('/');
        string trimmedChannelName = channel.Substring(startIndex, channel.Length - startIndex);
        channelName = trimmedChannelName;
        password = pass;

#if UNITY_EDITOR
        if (String.IsNullOrEmpty(pass))
        {
            username = TwitchData.username;
            channelName = TwitchData.channelName;
            password = TwitchData.password;
        }
#endif
        StartIRC();
    }

    private void StartIRC()
    {
        System.Net.Sockets.TcpClient sock = new System.Net.Sockets.TcpClient();
        sock.Connect(server, port);
        if (!sock.Connected)
        {
            Debug.Log("Failed to connect!");
        }
        else
        {
            Debug.Log("Connected successfully");
        }

        var networkStream = sock.GetStream();
        var input = new System.IO.StreamReader(networkStream);
        var output = new System.IO.StreamWriter(networkStream);

        //Send PASS & NICK (TODO: use the ones in this class if empty)
        output.WriteLine("PASS " + password);
        output.WriteLine("NICK " + username.ToLower());
        output.Flush();

        //output proc
        outProc = new Thread(() => IRCOutputProcedure(output));
        outProc.Start();
        //input proc
        inProc = new Thread(() => IRCInputProcedure(input, networkStream));
        inProc.Start();
    }

    private void IRCInputProcedure(System.IO.TextReader input, System.Net.Sockets.NetworkStream networkStream)
    {
        while (!stopThreads)
        {
            if (!networkStream.DataAvailable)
            {
                Thread.Sleep(1);
                continue;
            }


            buffer = input.ReadLine();

            Debug.Log(buffer);

            //was message?
            if (buffer.Contains("PRIVMSG #"))
            {
                lock (recievedMsgs)
                {
                    recievedMsgs.Add(buffer);
                }
            }

            //Send pong reply to any ping messages
            if (buffer.StartsWith("PING "))
            {
                SendCommand(buffer.Replace("PING", "PONG"));
            }

            //After server sends 001 command, we can join a channel
            if (buffer.Split(' ')[1] == "001")
            {
                SendCommand("JOIN #" + channelName.ToLower());
                isConnected = true;
            }

            Thread.Sleep(1);
        }
    }

    private void IRCOutputProcedure(System.IO.TextWriter output)
    {
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        while (!stopThreads)
        {
            lock (commandQueue)
            {
                if (commandQueue.Count > 0) //do we have any commands to send?
                {
                    // https://github.com/justintv/Twitch-API/blob/master/IRC.md#command--message-limit 
                    //have enough time passed since we last sent a message/command?
                    if (stopWatch.ElapsedMilliseconds > 1750)
                    {
                        //send msg.
                        output.WriteLine(commandQueue.Peek());
                        output.Flush();
                        //remove msg from queue.
                        commandQueue.Dequeue();
                        //restart stopwatch.
                        stopWatch.Reset();
                        stopWatch.Start();
                    }
                }
            }

            Thread.Sleep(1);
        }
    }

    public void SendCommand(string cmd)
    {
        lock (commandQueue)
        {
            commandQueue.Enqueue(cmd);
        }
    }

    public void SendMsg(string msg)
    {
        lock (commandQueue)
        {
            commandQueue.Enqueue("PRIVMSG #" + channelName.ToLower() + " :" + msg + "\r\n");
        }
    }

    void OnEnable()
    {
        stopThreads = false;
    }

    void OnDisable()
    {
        stopThreads = true;
    }

    void OnDestroy()
    {
        stopThreads = true;
    }

    void Update()
    {
        lock (recievedMsgs)
        {
            if (recievedMsgs.Count > 0)
            {
                for (int i = 0; i < recievedMsgs.Count; i++)
                {
                    OnChatMsgReceived(recievedMsgs[i]);
                }

                recievedMsgs.Clear();
            }
        }
    }

    private void OnChatMsgReceived(string msg)
    {
        if (msg.Contains(GAME_COMMAND))
        {
            int msgIndex = msg.IndexOf("PRIVMSG #");
            string msgString = msg.Substring(msgIndex + channelName.Length + 11);

            if (msgString.StartsWith(GAME_COMMAND))
            {
                msgString = msgString.Substring(GAME_COMMAND.Length);
            }
            else
            {
                return;
            }

            string user = msg.Substring(1, msg.IndexOf('!') - 1);

            if (msgString.Length > 0)
            {
                msgString = msgString.Substring(0, Mathf.Min(msgString.Length, 25));
            }

            if (string.IsNullOrEmpty(msgString) || string.IsNullOrWhiteSpace(msgString)) return;

            string[] splittedString = msgString.Split(' ');

            if (Enum.TryParse(splittedString[0].ToUpper(), out CommandArg commandArg))
            {
                TwitchCommand twitchCommand = new TwitchCommand(user,
                    splittedString.Length > 1 ? splittedString[1] : String.Empty, commandArg);
                EventsManager.onTwitchCommandReceived(twitchCommand);
                Debug.Log(String.Format("Launched {0} from {1} with message {2}", twitchCommand.argument,
                    twitchCommand.username, twitchCommand.message));
            }

            //Debug.Log(String.Format("Received {0} from {1}", msgString, user));
        }
    }
}

public struct TwitchCommand
{
    public string username;
    public string message;
    public CommandArg argument;


    //Use this constructor only when you are sure that the argument is valid
    public TwitchCommand(string _username, string _message, CommandArg _arg)
    {
        username = _username;
        message = _message;
        argument = _arg;
    }
}

public enum CommandArg
{
    JOIN,
    LEAVE,
    BODY,
    TOP,
    FRONT,
    LEFT,
    RIGHT,
    BOTTOM,
    PARTCOLOR,
    BODYCOLOR,
    READY,
    RANDOMIZE,
    COMMIT
}