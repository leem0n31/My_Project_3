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
            await bot.SendTextMessageAsync(msg.Chat.Id, "Добро пожаловать в мир магических кредитов! Введите ваше имя:");
            return;
        }

        userNames[msg.Chat.Id] = msg.Text;
        await bot.SendTextMessageAsync(msg.Chat.Id, $"Спасибо, {msg.Text}! Для начала напишите 'Кредиты', чтобы узнать о доступных вариантах.");
        return;
    }

    var userName = userNames[msg.Chat.Id];

    if (msg.Text.ToLower() == "кредиты")
    {
        await bot.SendTextMessageAsync(msg.Chat.Id, "Выберите тип кредита:\n1. Кредит на метлу\n2. Кредит на обучение\n3. Кредит на зелья",
            replyMarkup: new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("1"),
            InlineKeyboardButton.WithCallbackData("2"),
            InlineKeyboardButton.WithCallbackData("3"),
            InlineKeyboardButton.WithCallbackData("Помощь"),
        }));
    }
    else if (msg.Text.ToLower() == "проверка")
    {
        await bot.SendTextMessageAsync(msg.Chat.Id, $"{userName}, проверка бота: работа корректна");
    }
}

async Task OnCallbackQuery(CallbackQuery query)
{
    await bot.AnswerCallbackQueryAsync(query.Id);

    if (query.Data == "1")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Кредит на метлу: 1000 сиклей на 12 месяцев. Процентная ставка: 5%.");
    }
    else if (query.Data == "2")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Кредит на обучение: 1500 сиклей на 24 месяца. Процентная ставка: 4%.");
    }
    else if (query.Data == "3")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Кредит на зелья: 500 сиклей на 6 месяцев. Процентная ставка: 6%.");
    }
    else if (query.Data == "Помощь")
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Для получения кредита вам нужно выбрать его тип. Напишите 'Кредиты', чтобы начать.");
    }
}

async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine(exception);
}
