# MyExpenses API Endpoints

## 🔐 Authentication
- `POST /api/Auth/register` - Create a new user account.
- `POST /api/Auth/login` - Authenticate and receive a JWT token.
- `GET /api/Auth/authTest` - [Authorize] Test endpoint to verify token validity.

## 💰 Expenses
- `GET /api/Expenses` - [Authorize] Retrieve all expenses.
- `GET /api/Expenses/{id}` - [Authorize] Get a specific expense by ID.
- `POST /api/Expenses` - [Authorize] Create a new expense.
- `PUT /api/Expenses/{id}` - [Authorize] Update an existing expense.
- `DELETE /api/Expenses/{id}` - [Authorize] Remove an expense.

## 🏷 Categories
- `GET /api/Categories` - [Authorize] Retrieve all categories.
- `GET /api/Categories/{id}` - [Authorize] Get a specific category by ID.
- `POST /api/Categories` - [Authorize] Create a new category.
- `PUT /api/Categories/{id}` - [Authorize] Update an existing category.
- `DELETE /api/Categories/{id}` - [Authorize] Remove a category.