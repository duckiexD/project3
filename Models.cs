// User.cs
public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public UserProfile Profile { get; set; }
}

// UserProfile.cs
public class UserProfile
{
    public int ProfileId { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}

// Product.cs
public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public List<Category> Categories { get; set; } = new List<Category>();
}

// Brand.cs
public class Brand
{
    public int BrandId { get; set; }
    public string BrandName { get; set; }
    public string Description { get; set; }
    public int? FoundedYear { get; set; }
}

// Category.cs
public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int? ParentCategoryId { get; set; }
    public string Description { get; set; }
}

// Order.cs
public class Order
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Payment Payment { get; set; }
}

// OrderItem.cs
public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Product Product { get; set; }
}

// Payment.cs
public class Payment
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; }
    public string TransactionId { get; set; }
}
