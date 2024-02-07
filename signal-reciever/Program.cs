using System;


namespace SignalReciver
{
    class Program
    {

        static void Main(string[] args)
        {

            TelegramCommonClient.BotId = "6151012549:AAHD57wE4C-XmmLBSRzreaxCNYMyvmtNjgs";
            TelegramCommonClient.ChatId = -665949817;
            TelegramCommonClient.SignalBotId = "6985667882:AAG_RtbZhy6ybVnpqD29e9rlLk1El_oavq0";
            TelegramCommonClient.SignalChatId = -4153419136;
            var userInteraction = new TelegramCommonClient();


            //Thread.Sleep(100 * 60 * 60 * 1000);
        }

        
    }
}
