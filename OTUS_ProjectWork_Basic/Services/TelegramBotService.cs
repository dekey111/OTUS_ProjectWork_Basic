using OTUS_ProjectWork_Basic.DataBase;
using OTUS_ProjectWork_Basic.Interfaces;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

public class TelegramBotService
{
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ITelegramBotClient _botClient;
    private readonly Dictionary<long, UserState> _userStates;

    public TelegramBotService(
        IUserRepository userRepository,
        IBookRepository bookRepository,
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        string botToken)
    {
        _userRepository = userRepository;
        _bookRepository = bookRepository;
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _botClient = new TelegramBotClient(botToken);
        _userStates = new Dictionary<long, UserState>();
    }

    public async Task StartAsync()
    {
        var me = await _botClient.GetMe();
        Console.WriteLine($"Bot started: {me.Username}");

        _botClient.StartReceiving(HandleUpdateAsync,HandleErrorAsync);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is { } message)
        {
            await HandleMessageAsync(botClient, message, cancellationToken);
        }
        else if (update.CallbackQuery is { } callbackQuery)
        {
            await HandleCallbackQueryAsync(botClient, callbackQuery, cancellationToken);
        }
    }

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        var userId = (int)chatId;

        if (!_userStates.ContainsKey(chatId))
        {
            _userStates[chatId] = new UserState { CurrentMenu = MenuState.Main };
        }

        var userState = _userStates[chatId];

        switch (message.Text)
        {
            case "/start":
                var user =  _userRepository.GetOrCreateUserAsync(
                    telegramId: userId,
                    accountName: message.Chat.Username,
                    username: message.Chat.LastName + " " + message.Chat.FirstName,
                    createdate: DateTime.Now,
                    isActiv: true);

                await ShowMainMenu(chatId);
                break;
            case "🔍 Поиск книг":
                userState.CurrentMenu = MenuState.Search;
                await ShowSearchOptions(chatId);
                break;
            case "📚 По автору":
                userState.CurrentMenu = MenuState.SearchByAuthor;
                await ShowAuthors(chatId);
                break;
            case "📖 По жанру":
                userState.CurrentMenu = MenuState.SearchByCategory;
                await ShowCategories(chatId);
                break;
            case "🔎 По названию":
                userState.CurrentMenu = MenuState.SearchByTitle;
                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Введите название книги для поиска:",
                    cancellationToken: cancellationToken);
                break;
            case "🛒 Корзина":
                userState.CurrentMenu = MenuState.Cart;
                await ShowCart(chatId, userId);
                break;
            case "📦 Мои заказы":
                userState.CurrentMenu = MenuState.Orders;
                await ShowOrders(chatId, userId);
                break;
            case "⬅️ Назад":
                userState.CurrentMenu = MenuState.Main;
                await ShowMainMenu(chatId);
                break;
            default:
                await HandleOtherMessages(chatId, userId, message.Text, userState);
                break;
        }
    }

    private async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        try
        {
            var chatId = callbackQuery.Message.Chat.Id;
            var userId = (int)chatId;
            var callbackData = callbackQuery.Data;
            var userState = _userStates[chatId];

            // Обработка нажатия на автора
            if (userState.CurrentMenu == MenuState.SearchByAuthor && int.TryParse(callbackData, out var authorId))
            {
                var books = _bookRepository.GetByAuthor(authorId);
                await ShowBooks(chatId, books, "Книги этого автора:");
                userState.CurrentMenu = MenuState.Main;
            }
            // Обработка нажатия на категорию
            else if (userState.CurrentMenu == MenuState.SearchByCategory && int.TryParse(callbackData, out var categoryId))
            {
                var books = _bookRepository.GetByCategory(categoryId);
                await ShowBooks(chatId, books, "Книги в этой категории:");
                userState.CurrentMenu = MenuState.Main;
            }
            // Обработка нажатия на книгу
            else if (int.TryParse(callbackData, out var bookId))
            {
                await ShowBookDetails(chatId, bookId);
                userState.CurrentMenu = MenuState.BookDetails;
            }
            // Обработка кнопки "Назад"
            else if (callbackData == "back")
            {
                await ShowMainMenu(chatId);
                userState.CurrentMenu = MenuState.Main;
            }
            // Обработка добавления в корзину
            else if (callbackData.StartsWith("add_") && int.TryParse(callbackData.Substring(4), out var bookIdToAdd))
            {
                _cartRepository.AddOrUpdateItem(userId, bookIdToAdd);
                await botClient.AnswerCallbackQuery(
                    callbackQueryId: callbackQuery.Id,
                    text: "Книга добавлена в корзину");
            }
            // Обработка оформления заказа
            else if (callbackData == "checkout")
            {
                var cartItems = _cartRepository.GetUserCart(userId);
                if (cartItems.Any())
                {
                    _orderRepository.CreateOrder(userId, cartItems);
                    _cartRepository.ClearCart(userId);
                    await botClient.AnswerCallbackQuery(
                        callbackQueryId: callbackQuery.Id,
                        text: "Заказ оформлен!");
                    await ShowOrders(chatId, userId);
                }
                else
                {
                    await botClient.AnswerCallbackQuery(
                        callbackQueryId: callbackQuery.Id,
                        text: "Корзина пуста");
                }
            }
            // Обработка очистки корзины
            else if (callbackData == "clear_cart")
            {
                _cartRepository.ClearCart(userId);
                await botClient.AnswerCallbackQuery(
                    callbackQueryId: callbackQuery.Id,
                    text: "Корзина очищена");
                await ShowCart(chatId, userId);
            }

            // Убираем "часики" с кнопки
            await botClient.AnswerCallbackQuery(callbackQuery.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private async Task HandleOtherMessages(long chatId, int userId, string messageText, UserState userState)
    {
        switch (userState.CurrentMenu)
        {
            case MenuState.SearchByTitle:
                var books = _bookRepository.Search(messageText);
                await ShowBooks(chatId, books, "Результаты поиска:");
                userState.CurrentMenu = MenuState.Main;
                break;
            case MenuState.BookDetails when int.TryParse(messageText, out var bookId):
                await ShowBookDetails(chatId, bookId);
                break;
            case MenuState.Cart when int.TryParse(messageText, out var cartItemId):
                _cartRepository.RemoveItem(cartItemId);
                await ShowCart(chatId, userId);
                break;
            default:
                if (int.TryParse(messageText, out var id))
                {
                    switch (userState.CurrentMenu)
                    {
                        case MenuState.SearchByAuthor:
                            var booksByAuthor = _bookRepository.GetByAuthor(id);
                            await ShowBooks(chatId, booksByAuthor, "Книги этого автора:");
                            break;
                        case MenuState.SearchByCategory:
                            var booksByCategory = _bookRepository.GetByCategory(id);
                            await ShowBooks(chatId, booksByCategory, "Книги в этой категории:");
                            break;
                    }
                }
                break;
        }
    }

    private async Task ShowMainMenu(long chatId)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "🔍 Поиск книг" },
            new KeyboardButton[] { "🛒 Корзина", "📦 Мои заказы" }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Добро пожаловать в книжный магазин! Выберите действие:",
            replyMarkup: replyKeyboard);
    }

    private async Task ShowSearchOptions(long chatId)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "📚 По автору", "📖 По жанру" },
            new KeyboardButton[] { "🔎 По названию" },
            new KeyboardButton[] { "⬅️ Назад" }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Выберите способ поиска:",
            replyMarkup: replyKeyboard);
    }

    private async Task ShowAuthors(long chatId)
    {
        var authors = _bookRepository.GetAllActiveAuthors();
        var buttons = authors.Select(a => new[]
        {
            InlineKeyboardButton.WithCallbackData(a.Fullname, a.Id.ToString())
        }).ToList();

        buttons.Add(new[] { InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back") });

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Выберите автора:",
            replyMarkup: inlineKeyboard);
    }

    private async Task ShowCategories(long chatId)
    {
        var categories = _bookRepository.GetAllCategories();
        var buttons = categories.Select(c => new[]
        {
            InlineKeyboardButton.WithCallbackData(c.Name, c.Id.ToString())
        }).ToList();

        buttons.Add(new[] { InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back") });

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Выберите категорию:",
            replyMarkup: inlineKeyboard);
    }

    private async Task ShowBooks(long chatId, IEnumerable<Book> books, string message)
    {
        if (!books.Any())
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: "Книги не найдены.");
            return;
        }

        var buttons = books.Select(b => new[]
        {
            InlineKeyboardButton.WithCallbackData($"{b.Title} ({b.Price} руб.)", b.Id.ToString())
        }).ToList();

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        await _botClient.SendMessage(
            chatId: chatId,
            text: message,
            replyMarkup: inlineKeyboard);
    }

    private async Task ShowBookDetails(long chatId, int bookId)
    {
        var book = _bookRepository.GetById(bookId);
        if (book == null)
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: "Книга не найдена.");
            return;
        }

        var message = $"<b>{book.Title}</b>\n" +
                      $"Автор: {book.Author?.Fullname ?? "Неизвестен"}\n" +
                      $"Год: {book.Publicationyear}\n" +
                      $"Цена: {book.Price} руб.\n" +
                      $"\n{book.Description}\n" +
                      $"\nДобавить в корзину: /add_{book.Id}";

        var replyMarkup = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("🛒 Добавить в корзину", $"add_{book.Id}"),
            InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")
        });

        await _botClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.Html,
            replyMarkup: replyMarkup);
    }

    private async Task ShowCart(long chatId, int userId)
    {
        var cartItems = _cartRepository.GetUserCart(userId);
        if (!cartItems.Any())
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: "Ваша корзина пуста.");
            return;
        }

        var total = _cartRepository.CalculateTotal(userId);
        var message = "🛒 <b>Ваша корзина</b>\n\n";

        foreach (var item in cartItems)
        {
            message += $"{item.Book.Title} - {item.Quantity} x {item.Book.Price} руб. (ID: {item.Id})\n";
        }

        message += $"\n<b>Итого: {total} руб.</b>\n\n" +
                   "Введите ID товара, чтобы удалить его из корзины, или нажмите кнопку ниже для оформления заказа.";

        var replyMarkup = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("✅ Оформить заказ", "checkout"),
            InlineKeyboardButton.WithCallbackData("🗑 Очистить корзину", "clear_cart"),
            InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")
        });

        await _botClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.Html,
            replyMarkup: replyMarkup);
    }

    private async Task ShowOrders(long chatId, int userId)
    {
        var orders = _orderRepository.GetUserOrders(userId);
        if (!orders.Any())
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: "У вас пока нет заказов.");
            return;
        }

        var message = "📦 <b>Ваши заказы</b>\n\n";
        foreach (var order in orders)
        {
            message += $"{order.Book.Title} - {order.Quantity} x {order.Price / order.Quantity} руб. (Итого: {order.Price} руб.)\n" +
                      $"Дата: {order.Purchasedate:dd.MM.yyyy}\n\n";
        }

        await _botClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.Html);
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}

// UserState.cs
public class UserState
{
    public MenuState CurrentMenu { get; set; }
    public int? SelectedBookId { get; set; }
}

public enum MenuState
{
    Main,
    Search,
    SearchByAuthor,
    SearchByCategory,
    SearchByTitle,
    BookDetails,
    Cart,
    Orders
}