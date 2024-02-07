using System;
using SignalReciver.Brokerage.IBClient;
using Telegram.Bot;


namespace SignalReciver
{
    internal sealed class TelegramCommonClient : IUserInteraction
    {
        internal static long ChatId;
        internal static string BotId;
        internal static string SignalBotId;
        internal static long SignalChatId;
        private static TelegramBotClient BotClient;
        private static readonly RateLimiter _apiRateLimiter = new(25, new TimeSpan(0, 0, 1));
        private static SemaphoreSlim _semaphore = new(1);
        private static readonly HttpClient client = new();
        private ICommandHandler _commandHandler;
        
        internal TelegramCommonClient()
        {
            BotClient = new TelegramBotClient(SignalBotId);
            var a = new Thread(new ThreadStart(GetUpdates));
           a.Start();
        }

        public void SetCommandHandler(ICommandHandler handler)
        {
            _commandHandler = handler;
        }

        internal static void SendMessage(string message, bool isSignal = false)
        {
            _apiRateLimiter.Enter();
            _semaphore.Wait();
            try
            {
                var sendMessageReq = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", isSignal? SignalBotId : BotId, isSignal ? SignalChatId : ChatId, message);
                var stringTask = client.GetStringAsync(sendMessageReq);
                stringTask.Wait(1000);
            }
            catch (Exception) { }
            finally
            {
                _semaphore.Release();
            }
        }

        public void GetUpdates()
        {
            Console.WriteLine("Listening");
            try
            {
                var offset = 0;

                while (true)
                {
                    var updates = BotClient.GetUpdatesAsync(offset, 100, 360000).Result;
                    if (updates.Length == 0)
                        Console.WriteLine("coming up empty ");
                    foreach (var update in updates)
                    {
                        offset = update.Id + 1;

                        if (update.Message == null || update.Message.Text == null)
                            continue;

                        switch (update.Message.Text)
                        {
                            case string a when a.Contains("stats"):
                                a = a.Trim();
                                var stats = a.Split('-');
                                _commandHandler?.PostCommand("stats", stats[stats.Length - 1]);
                                break;
                            case "/refresh":
                                _commandHandler?.PostCommand("refresh");
                                break;
                            case "/oor":
                                 _commandHandler?.PostCommand("oor");
                                break;
                            case "/account":
                                 _commandHandler?.PostCommand("account");
                                break;
                            case "/open":
                                _commandHandler?.PostCommand("open");
                                break;
                            case string b when b.Contains('{'):
                                SendMessage("I got it", true);
                                break;
                            default:
                                SendMessage("I dont understand", true);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex.ToString());
                var a = new Thread(new ThreadStart(GetUpdates));
                a.Start();
            }
        }
    }
}