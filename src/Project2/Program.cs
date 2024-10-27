using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7367399878:AAEr3bTJxrirWSq2i1rfTHPbP-PR04LrSuk", cancellationToken: cts.Token);
var me = await bot.GetMeAsync();
var userNames = new Dictionary<long, string>();

Console.WriteLine($"@{me.Username} запущен... Нажмите Enter чтобы выключить");

bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync);

Console.ReadLine();
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message)
    {
        await OnMessage(update.Message);
    }
    else if (update.Type == UpdateType.CallbackQuery)
    {
        await OnCallbackQuery(update.CallbackQuery);
    }
}

async Task OnMessage(Message msg)
{
    if (msg.Text == null)
        return;

    if (!userNames.ContainsKey(msg.Chat.Id))
    {
        if (msg.Text == "/start")
        {
            await bot.SendTextMessageAsync(msg.Chat.Id, "Введите ваше имя:");
            return;
        }

        userNames[msg.Chat.Id] = msg.Text;
        await bot.SendTextMessageAsync(msg.Chat.Id, $"Спасибо, {msg.Text}! Для того, чтобы попасть в мир Хогвартса напишите Авадакедабра");
        return;
    }

    var userName = userNames[msg.Chat.Id];

    if (msg.Text == "Проверка")
    {
        await bot.SendTextMessageAsync(msg.Chat.Id, $"{userName}, проверка бота: работа корректна");
    }
    else if (msg.Text == "Привет")
    {
        await bot.SendTextMessageAsync(msg.Chat.Id, $"Здравствуйте, {userName}!");
    }
    else if (msg.Text == "Видео")
    {
        await bot.SendVideoAsync(msg.Chat.Id, "https://telegrambots.github.io/book/docs/video-countdown.mp4");
    }
    else if (msg.Text == "Стикер")
    {
        await bot.SendStickerAsync(msg.Chat.Id, "https://telegrambots.github.io/book/docs/sticker-fred.webp");
    }
    else if (msg.Text == "Авадакедабра")
    {
        await bot.SendTextMessageAsync(msg.Chat.Id, $"Здравствуйте, {userName}!",
             replyMarkup: new InlineKeyboardMarkup(new[]
             {
                 InlineKeyboardButton.WithCallbackData("Зелье неведимки"),
                 InlineKeyboardButton.WithCallbackData("Зелье щита"),
                 InlineKeyboardButton.WithCallbackData("Зелье урона"),
                 InlineKeyboardButton.WithCallbackData("help"),
             }));
    }
}

async Task OnCallbackQuery(CallbackQuery query)
{
    await bot.AnswerCallbackQueryAsync(query.Id, $"Вы выбрали {query.Data}");

    if (query.Data == "Зелье неведимки")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Вы активировали зелье невидимки. Теперь вы сможете быть невидимым на 60 секунд.");
    }
    else if (query.Data == "Зелье щита")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Вы активировали зелье щита. Теперь вы не можете получать урон на 60 секунд.");
    }
    else if (query.Data == "Зелье урона")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Вы активировали зелье урона. Теперь вы можете наносить в 2 раза больше урона на 60 секунд.");
    }
    else if (query.Data == "help")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Зелья нужны для битвы с боссом. Сделайте правильный выбор.");
    }
}

async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine(exception);
}
