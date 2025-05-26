using System;
using System.Collections.Generic;
using System.Configuration;

class Program
{
    private static DatabaseService _dbService = new DatabaseService();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
       
        try
        {
            var testConnection = _dbService.GetConnection();
            testConnection.Open();
            testConnection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
            Console.WriteLine("Проверьте строку подключения в файле конфигурации.");
            return;
        }

        ShowMainMenu();
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ОНЛАЙН-МАГАЗИН ОДЕЖДЫ ===");
            Console.WriteLine("1. Управление товарами");
            Console.WriteLine("2. Управление заказами");
            Console.WriteLine("3. Управление пользователями");
            Console.WriteLine("4. Управление категориями");
            Console.WriteLine("5. Управление брендами");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        ShowProductsMenu();
                        break;
                    case 2:
                        ShowOrdersMenu();
                        break;
                    case 3:
                        ShowUsersMenu();
                        break;
                    case 4:
                        ShowCategoriesMenu();
                        break;
                    case 5:
                        ShowBrandsMenu();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод. Введите число.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void ShowProductsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== УПРАВЛЕНИЕ ТОВАРАМИ ===");
            Console.WriteLine("1. Просмотреть все товары");
            Console.WriteLine("2. Добавить новый товар");
            Console.WriteLine("3. Редактировать товар");
            Console.WriteLine("4. Удалить товар");
            Console.WriteLine("5. Поиск товара по названию");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите пункт меню: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        DisplayAllProducts();
                        break;
                    case 2:
                        AddNewProduct();
                        break;
                    case 3:
                        EditProduct();
                        break;
                    case 4:
                        DeleteProduct();
                        break;
                    case 5:
                        SearchProducts();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод. Введите число.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void DisplayAllProducts()
    {
        Console.WriteLine("\n=== СПИСОК ТОВАРОВ ===");
        var products = _dbService.GetAllProducts();
        
        if (products.Count == 0)
        {
            Console.WriteLine("Товары не найдены.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.ProductId}");
            Console.WriteLine($"Название: {product.ProductName}");
            Console.WriteLine($"Бренд: {product.Brand?.BrandName}");
            Console.WriteLine($"Цена: {product.Price:C}");
            Console.WriteLine($"Количество на складе: {product.StockQuantity}");
            Console.WriteLine($"Описание: {product.Description}");
            Console.WriteLine(new string('-', 40));
        }
    }

    static void AddNewProduct()
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО ТОВАРА ===");
        
        try
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            
            Console.Write("Введите описание товара: ");
            string description = Console.ReadLine();
            
            Console.Write("Введите цену товара: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Неверный формат цены.");
                return;
            }
            
            Console.Write("Введите количество на складе: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Неверный формат количества.");
                return;
            }
            
            Console.Write("Введите ID бренда: ");
            if (!int.TryParse(Console.ReadLine(), out int brandId))
            {
                Console.WriteLine("Неверный формат ID бренда.");
                return;
            }
            
            // Здесь должен быть код добавления товара в БД
            Console.WriteLine("Товар успешно добавлен!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
        }
    }

    

static void ShowOrdersMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== УПРАВЛЕНИЕ ЗАКАЗАМИ ===");
        Console.WriteLine("1. Просмотреть все заказы");
        Console.WriteLine("2. Просмотреть заказы пользователя");
        Console.WriteLine("3. Создать новый заказ");
        Console.WriteLine("4. Изменить статус заказа");
        Console.WriteLine("5. Просмотреть детали заказа");
        Console.WriteLine("6. Добавить товар в заказ");
        Console.WriteLine("7. Удалить товар из заказа");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите пункт меню: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    DisplayAllOrders();
                    break;
                case 2:
                    DisplayUserOrders();
                    break;
                case 3:
                    CreateNewOrder();
                    break;
                case 4:
                    UpdateOrderStatus();
                    break;
                case 5:
                    ShowOrderDetails();
                    break;
                case 6:
                    AddProductToOrder();
                    break;
                case 7:
                    RemoveProductFromOrder();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод. Введите число.");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}

static void ShowUsersMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== УПРАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯМИ ===");
        Console.WriteLine("1. Просмотреть всех пользователей");
        Console.WriteLine("2. Просмотреть профиль пользователя");
        Console.WriteLine("3. Добавить нового пользователя");
        Console.WriteLine("4. Редактировать пользователя");
        Console.WriteLine("5. Блокировать/разблокировать пользователя");
        Console.WriteLine("6. Поиск пользователя по email");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите пункт меню: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    DisplayAllUsers();
                    break;
                case 2:
                    DisplayUserProfile();
                    break;
                case 3:
                    AddNewUser();
                    break;
                case 4:
                    EditUser();
                    break;
                case 5:
                    ToggleUserStatus();
                    break;
                case 6:
                    SearchUserByEmail();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод. Введите число.");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}

static void ShowCategoriesMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== УПРАВЛЕНИЕ КАТЕГОРИЯМИ ===");
        Console.WriteLine("1. Просмотреть все категории");
        Console.WriteLine("2. Просмотреть иерархию категорий");
        Console.WriteLine("3. Добавить новую категорию");
        Console.WriteLine("4. Редактировать категорию");
        Console.WriteLine("5. Удалить категорию");
        Console.WriteLine("6. Назначить товару категорию");
        Console.WriteLine("7. Удалить категорию у товара");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите пункт меню: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    DisplayAllCategories();
                    break;
                case 2:
                    DisplayCategoryHierarchy();
                    break;
                case 3:
                    AddNewCategory();
                    break;
                case 4:
                    EditCategory();
                    break;
                case 5:
                    DeleteCategory();
                    break;
                case 6:
                    AssignCategoryToProduct();
                    break;
                case 7:
                    RemoveCategoryFromProduct();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод. Введите число.");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}

static void ShowBrandsMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== УПРАВЛЕНИЕ БРЕНДАМИ ===");
        Console.WriteLine("1. Просмотреть все бренды");
        Console.WriteLine("2. Добавить новый бренд");
        Console.WriteLine("3. Редактировать бренд");
        Console.WriteLine("4. Удалить бренд");
        Console.WriteLine("5. Поиск бренда по названию");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите пункт меню: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    DisplayAllBrands();
                    break;
                case 2:
                    AddNewBrand();
                    break;
                case 3:
                    EditBrand();
                    break;
                case 4:
                    DeleteBrand();
                    break;
                case 5:
                    SearchBrandByName();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод. Введите число.");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}


static void DisplayAllOrders()
{
    Console.WriteLine("\n=== СПИСОК ВСЕХ ЗАКАЗОВ ===");
    try
    {
        using (var connection = _dbService.GetConnection())
        {
            var command = new SqlCommand(
                @"SELECT o.OrderId, o.OrderDate, o.Status, o.TotalAmount, 
                         u.UserId, u.Username, u.Email
                  FROM Orders o
                  JOIN Users u ON o.UserId = u.UserId
                  ORDER BY o.OrderDate DESC", 
                connection);
            
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("Заказы не найдены.");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine($"ID заказа: {reader["OrderId"]}");
                    Console.WriteLine($"Дата: {reader["OrderDate"]}");
                    Console.WriteLine($"Статус: {reader["Status"]}");
                    Console.WriteLine($"Сумма: {reader["TotalAmount"]} руб.");
                    Console.WriteLine($"Пользователь: {reader["Username"]} ({reader["Email"]})");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при получении списка заказов: {ex.Message}");
    }
}

static void CreateNewOrder()
{
    Console.WriteLine("\n=== СОЗДАНИЕ НОВОГО ЗАКАЗА ===");
    try
    {
        Console.Write("Введите ID пользователя: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Неверный формат ID пользователя.");
            return;
        }

        Console.Write("Введите адрес доставки: ");
        string address = Console.ReadLine();

      
        using (var connection = _dbService.GetConnection())
        {
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                var orderCommand = new SqlCommand(
                    @"INSERT INTO Orders (UserId, OrderDate, Status, TotalAmount, ShippingAddress)
                      VALUES (@UserId, GETDATE(), 'New', 0, @Address);
                      SELECT SCOPE_IDENTITY();",
                    connection, transaction);
                
                orderCommand.Parameters.AddWithValue("@UserId", userId);
                orderCommand.Parameters.AddWithValue("@Address", address);
                
                int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());
                
                // Добавляем товары в заказ
                bool addMoreProducts = true;
                decimal totalAmount = 0;
                
                while (addMoreProducts)
                {
                    Console.Write("Введите ID товара (0 для завершения): ");
                    if (!int.TryParse(Console.ReadLine(), out int productId) || productId == 0)
                    {
                        addMoreProducts = false;
                        continue;
                    }

                    Console.Write("Введите количество: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Неверное количество. Попробуйте снова.");
                        continue;
                    }

                    var productCommand = new SqlCommand(
                        "SELECT Price, StockQuantity FROM Products WHERE ProductId = @ProductId",
                        connection, transaction);
                    productCommand.Parameters.AddWithValue("@ProductId", productId);
                    
                    using (var reader = productCommand.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Console.WriteLine("Товар не найден.");
                            reader.Close();
                            continue;
                        }

                        decimal price = reader.GetDecimal(0);
                        int stockQuantity = reader.GetInt32(1);
                        reader.Close();

                        if (stockQuantity < quantity)
                        {
                            Console.WriteLine($"Недостаточно товара на складе. Доступно: {stockQuantity}");
                            continue;
                        }

                        // Добавляем товар в заказ
                        var itemCommand = new SqlCommand(
                            @"INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
                              VALUES (@OrderId, @ProductId, @Quantity, @Price)",
                            connection, transaction);
                        
                        itemCommand.Parameters.AddWithValue("@OrderId", orderId);
                        itemCommand.Parameters.AddWithValue("@ProductId", productId);
                        itemCommand.Parameters.AddWithValue("@Quantity", quantity);
                        itemCommand.Parameters.AddWithValue("@Price", price);
                        
                        itemCommand.ExecuteNonQuery();
                        
                        var updateStockCommand = new SqlCommand(
                            "UPDATE Products SET StockQuantity = StockQuantity - @Quantity WHERE ProductId = @ProductId",
                            connection, transaction);
                        
                        updateStockCommand.Parameters.AddWithValue("@Quantity", quantity);
                        updateStockCommand.Parameters.AddWithValue("@ProductId", productId);
                        updateStockCommand.ExecuteNonQuery();
                        
                        totalAmount += price * quantity;
                        Console.WriteLine("Товар успешно добавлен в заказ.");
                    }
                }

                // Обновляем итоговую сумму заказа
                var updateOrderCommand = new SqlCommand(
                    "UPDATE Orders SET TotalAmount = @TotalAmount WHERE OrderId = @OrderId",
                    connection, transaction);
                
                updateOrderCommand.Parameters.AddWithValue("@TotalAmount", totalAmount);
                updateOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
                updateOrderCommand.ExecuteNonQuery();
                
                transaction.Commit();
                Console.WriteLine($"Заказ #{orderId} успешно создан. Итоговая сумма: {totalAmount} руб.");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при создании заказа: {ex.Message}");
    }
}

static void DisplayAllUsers()
{
    Console.WriteLine("\n=== СПИСОК ПОЛЬЗОВАТЕЛЕЙ ===");
    try
    {
        using (var connection = _dbService.GetConnection())
        {
            var command = new SqlCommand(
                "SELECT UserId, Username, Email, CreatedAt, IsActive FROM Users ORDER BY Username",
                connection);
            
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("Пользователи не найдены.");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["UserId"]}");
                    Console.WriteLine($"Логин: {reader["Username"]}");
                    Console.WriteLine($"Email: {reader["Email"]}");
                    Console.WriteLine($"Дата регистрации: {reader["CreatedAt"]}");
                    Console.WriteLine($"Статус: {(reader.GetBoolean(4) ? "Активен" : "Заблокирован"}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при получении списка пользователей: {ex.Message}");
    }
}

static void AddNewUser()
{
    Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО ПОЛЬЗОВАТЕЛЯ ===");
    try
    {
        Console.Write("Введите логин: ");
        string username = Console.ReadLine();
        
        Console.Write("Введите email: ");
        string email = Console.ReadLine();
        
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();
        
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new SqlCommand(
                @"INSERT INTO Users (Username, Email, PasswordHash, CreatedAt, IsActive)
                  VALUES (@Username, @Email, @Password, GETDATE(), 1);
                  SELECT SCOPE_IDENTITY();",
                connection);
            
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password); 
            
            connection.Open();
            int userId = Convert.ToInt32(command.ExecuteScalar());
            
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ПРОФИЛЯ ===");
            Console.Write("Имя: ");
            string firstName = Console.ReadLine();
            
            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine();
            
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();
            
            Console.Write("Адрес: ");
            string address = Console.ReadLine();
            
            var profileCommand = new SqlCommand(
                @"INSERT INTO UserProfiles (UserId, FirstName, LastName, Phone, Address)
                  VALUES (@UserId, @FirstName, @LastName, @Phone, @Address)",
                connection);
            
            profileCommand.Parameters.AddWithValue("@UserId", userId);
            profileCommand.Parameters.AddWithValue("@FirstName", firstName);
            profileCommand.Parameters.AddWithValue("@LastName", lastName);
            profileCommand.Parameters.AddWithValue("@Phone", phone);
            profileCommand.Parameters.AddWithValue("@Address", address);
            
            profileCommand.ExecuteNonQuery();
            
            Console.WriteLine($"Пользователь #{userId} успешно добавлен.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при добавлении пользователя: {ex.Message}");
    }
}

static void DisplayAllCategories()
{
    Console.WriteLine("\n=== СПИСОК КАТЕГОРИЙ ===");
    try
    {
        using (var connection = _dbService.GetConnection())
        {
            var command = new SqlCommand(
                "SELECT CategoryId, CategoryName, Description FROM Categories ORDER BY CategoryName",
                connection);
            
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("Категории не найдены.");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["CategoryId"]}");
                    Console.WriteLine($"Название: {reader["CategoryName"]}");
                    Console.WriteLine($"Описание: {reader["Description"]}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при получении списка категорий: {ex.Message}");
    }
}

static void AddNewCategory()
{
    Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОЙ КАТЕГОРИИ ===");
    try
    {
        Console.Write("Введите название категории: ");
        string name = Console.ReadLine();
        
        Console.Write("Введите описание: ");
        string description = Console.ReadLine();
        
        Console.Write("Введите ID родительской категории (0 если нет): ");
        if (!int.TryParse(Console.ReadLine(), out int parentId))
        {
            Console.WriteLine("Неверный формат ID.");
            return;
        }

        using (var connection = _dbService.GetConnection())
        {
            var command = new SqlCommand(
                @"INSERT INTO Categories (CategoryName, ParentCategoryId, Description)
                  VALUES (@Name, @ParentId, @Description);
                  SELECT SCOPE_IDENTITY();",
                connection);
            
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@ParentId", parentId == 0 ? DBNull.Value : (object)parentId);
            
            connection.Open();
            int categoryId = Convert.ToInt32(command.ExecuteScalar());
            
            Console.WriteLine($"Категория #{categoryId} успешно добавлена.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при добавлении категории: {ex.Message}");
    }
}

static void DisplayAllBrands()
{
    Console.WriteLine("\n=== СПИСОК БРЕНДОВ ===");
    try
    {
        using (var connection = _dbService.GetConnection())
        {
            var command = new SqlCommand(
                "SELECT BrandId, BrandName, Description, FoundedYear FROM Brands ORDER BY BrandName",
                connection);
            
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("Бренды не найдены.");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["BrandId"]}");
                    Console.WriteLine($"Название: {reader["BrandName"]}");
                    Console.WriteLine($"Описание: {reader["Description"]}");
                    Console.WriteLine($"Год основания: {reader["FoundedYear"] ?? "не указан"}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при получении списка брендов: {ex.Message}");
    }
}

static void AddNewBrand()
{
    Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО БРЕНДА ===");
    try
    {
        Console.Write("Введите название бренда: ");
        string name = Console.ReadLine();
        
        Console.Write("Введите описание: ");
        string description = Console.ReadLine();
        
        Console.Write("Введите год основания (если известен): ");
        string yearInput = Console.ReadLine();
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new SqlCommand(
                @"INSERT INTO Brands (BrandName, Description, FoundedYear)
                  VALUES (@Name, @Description, @Year);
                  SELECT SCOPE_IDENTITY();",
                connection);
            
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@Year", 
                int.TryParse(yearInput, out int year) ? (object)year : DBNull.Value);
            
            connection.Open();
            int brandId = Convert.ToInt32(command.ExecuteScalar());
            
            Console.WriteLine($"Бренд #{brandId} успешно добавлен.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при добавлении бренда: {ex.Message}");
    }
}

