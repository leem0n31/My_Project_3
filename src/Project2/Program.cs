﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7367399878:AAEr3bTJxrirWSq2i1rfTHPbP-PR04LrSuk", cancellationToken: cts.Token);
var me = await bot.GetMeAsync();
var userNames = new Dictionary<string, string>();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;
await bot.SendTextMessageAsync("1245700663", "Здравствуйте, для начала напишите /start ");


Console.WriteLine($"@{me.Username} запущен... Нажмите Enter чтобы выключить");
Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{

    if (msg.Text == "Проверка")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Проверка бота: работа корректна");
    }
    if (msg.Text == "Привет")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Здравствуйте");
    }
    if (msg.Text == "Видео")
    {
        await bot.SendVideoAsync("1245700663", "https://telegrambots.github.io/book/docs/video-countdown.mp4");
    }
    if (msg.Text == "/start")
    {
        await bot.SendTextMessageAsync("1245700663", "Выберите зелье",
           replyMarkup: new InlineKeyboardMarkup().AddButtons("Зелье неведимки", "Зелье щита", "Зелье урона", "help"));
    }
}


async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query })
    {
        await bot.AnswerCallbackQueryAsync(query.Id, $"You picked {query.Data}");


        if (query.Data == "Зелье неведимки")
        {
            await bot.SendTextMessageAsync("1245700663", "Вы активировали зелье невидимки. Теперь вы сможете быть невидимым на 60 секунд.");
        }
        if (query.Data == "Зелье щита")
        {
            await bot.SendTextMessageAsync("1245700663", "Вы активировали зелье щита. Теперь вы не можете получать урон на 60 секунд.");
        }
        if (query.Data == "Зелье урона")
        {
            await bot.SendTextMessageAsync("1245700663", "Вы активировали зелье урона. Теперь вы можете наносить в 2 раза больше урона на 60 секунд.");
        }
        if (query.Data == "help")
        {
            await bot.SendTextMessageAsync("1245700663", "Зелья нужны для битвы с боссом. Сделайте правильный выбор.");
        }
    }
}
