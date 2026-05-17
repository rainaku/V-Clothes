using System.Text.Json.Serialization;

namespace VClothes.Data;

// DTOs that map directly to Supabase PostgREST JSON (snake_case)
// These are used for serialization/deserialization with the REST API.

public class RoleDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string? Description { get; set; }
}

public class UserDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
    [JsonPropertyName("password_hash")] public string PasswordHash { get; set; } = string.Empty;
    [JsonPropertyName("display_name")] public string? DisplayName { get; set; }
    [JsonPropertyName("is_active")] public bool IsActive { get; set; }
    [JsonPropertyName("role_id")] public int RoleId { get; set; }
    [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("last_login")] public DateTime? LastLogin { get; set; }
}

public class CategoryDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("is_active")] public bool IsActive { get; set; } = true;
    [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; }
}

public class SupplierDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("address")] public string? Address { get; set; }
    [JsonPropertyName("phone")] public string? Phone { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("note")] public string? Note { get; set; }
    [JsonPropertyName("is_active")] public bool IsActive { get; set; } = true;
    [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; }
}

public class EmployeeDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("employee_code")] public string EmployeeCode { get; set; } = string.Empty;
    [JsonPropertyName("full_name")] public string FullName { get; set; } = string.Empty;
    [JsonPropertyName("gender")] public string? Gender { get; set; }
    [JsonPropertyName("date_of_birth")] public DateTime? DateOfBirth { get; set; }
    [JsonPropertyName("phone")] public string? Phone { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("address")] public string? Address { get; set; }
    [JsonPropertyName("position")] public string? Position { get; set; }
    [JsonPropertyName("hire_date")] public DateTime? HireDate { get; set; }
    [JsonPropertyName("is_active")] public bool IsActive { get; set; } = true;
    [JsonPropertyName("user_id")] public int? UserId { get; set; }
}

public class CustomerDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("full_name")] public string FullName { get; set; } = string.Empty;
    [JsonPropertyName("phone")] public string? Phone { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("address")] public string? Address { get; set; }
    [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; }
}

public class ProductDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("product_code")] public string ProductCode { get; set; } = string.Empty;
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("cost_price")] public decimal CostPrice { get; set; }
    [JsonPropertyName("stock_quantity")] public int StockQuantity { get; set; }
    [JsonPropertyName("size")] public string? Size { get; set; }
    [JsonPropertyName("color")] public string? Color { get; set; }
    [JsonPropertyName("material")] public string? Material { get; set; }
    [JsonPropertyName("image_path")] public string? ImagePath { get; set; }
    [JsonPropertyName("category_id")] public int CategoryId { get; set; }
    [JsonPropertyName("supplier_id")] public int SupplierId { get; set; }
    [JsonPropertyName("is_active")] public bool IsActive { get; set; } = true;
    [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; }

    // Joined fields (from select with foreign key)
    [JsonPropertyName("categories")] public CategoryDto? Category { get; set; }
    [JsonPropertyName("suppliers")] public SupplierDto? Supplier { get; set; }
}

public class PurchaseInvoiceDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("invoice_code")] public string InvoiceCode { get; set; } = string.Empty;
    [JsonPropertyName("invoice_date")] public DateTime InvoiceDate { get; set; }
    [JsonPropertyName("supplier_id")] public int SupplierId { get; set; }
    [JsonPropertyName("employee_id")] public int EmployeeId { get; set; }
    [JsonPropertyName("total_amount")] public decimal TotalAmount { get; set; }
    [JsonPropertyName("note")] public string? Note { get; set; }

    [JsonPropertyName("suppliers")] public SupplierDto? Supplier { get; set; }
    [JsonPropertyName("employees")] public EmployeeDto? Employee { get; set; }
}

public class PurchaseInvoiceDetailDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("purchase_invoice_id")] public int PurchaseInvoiceId { get; set; }
    [JsonPropertyName("product_id")] public int ProductId { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("unit_price")] public decimal UnitPrice { get; set; }
    [JsonPropertyName("sub_total")] public decimal SubTotal { get; set; }

    [JsonPropertyName("products")] public ProductDto? Product { get; set; }
}

public class SalesInvoiceDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("invoice_code")] public string InvoiceCode { get; set; } = string.Empty;
    [JsonPropertyName("invoice_date")] public DateTime InvoiceDate { get; set; }
    [JsonPropertyName("customer_id")] public int? CustomerId { get; set; }
    [JsonPropertyName("employee_id")] public int EmployeeId { get; set; }
    [JsonPropertyName("total_amount")] public decimal TotalAmount { get; set; }
    [JsonPropertyName("discount")] public decimal Discount { get; set; }
    [JsonPropertyName("final_amount")] public decimal FinalAmount { get; set; }
    [JsonPropertyName("note")] public string? Note { get; set; }

    [JsonPropertyName("customers")] public CustomerDto? Customer { get; set; }
    [JsonPropertyName("employees")] public EmployeeDto? Employee { get; set; }
}

public class SalesInvoiceDetailDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("sales_invoice_id")] public int SalesInvoiceId { get; set; }
    [JsonPropertyName("product_id")] public int ProductId { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("unit_price")] public decimal UnitPrice { get; set; }
    [JsonPropertyName("discount")] public decimal Discount { get; set; }
    [JsonPropertyName("sub_total")] public decimal SubTotal { get; set; }

    [JsonPropertyName("products")] public ProductDto? Product { get; set; }
}
