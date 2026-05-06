//Реалізуйте клієнт для отримання списку користувачів із тестового сервера https://jsonplaceholder.typicode.com/users. Програма повинна виконати GET-запит, отримати список користувачів у форматі JSON,
//десеріалізувати його у список об’єктів та вивести дані у вигляді таблиці. У таблиці повинні відображатися такі поля: ID користувача, ім’я, електронна пошта та місто проживання.
//Після цього програма має запропонувати користувачеві ввести ID користувача, і на основі введеного значення знайти відповідний запис у списку та вивести детальну інформацію про цього користувача.

using System.Net.Http.Json;

using HttpClient client = new();
client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

try
{
    Console.WriteLine("Завантаження списку користувачів...");
    var users = await client.GetFromJsonAsync<List<User>>("users");

    if (users == null) return;

    Console.WriteLine("\n" + new string('=', 75));
    Console.WriteLine($"{"ID",-4} | {"Ім'я",-20} | {"Email",-25} | {"Місто",-15}");
    Console.WriteLine(new string('-', 75));

    foreach (var u in users)
    {
        Console.WriteLine($"{u.Id,-4} | {u.Name,-20} | {u.Email,-25} | {u.Address.City,-15}");
    }
    Console.WriteLine(new string('=', 75));

    Console.Write("\nВведіть ID користувача для перегляду деталей: ");
    if (int.TryParse(Console.ReadLine(), out int searchId))
    {
        var user = users.FirstOrDefault(u => u.Id == searchId);
        if (user != null)
        {
            Console.WriteLine("\n>>> ДЕТАЛЬНА ІНФОРМАЦІЯ <<<");
            Console.WriteLine($"Ім'я:     {user.Name}");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Email:    {user.Email}");
            Console.WriteLine($"Телефон:  {user.Phone}");
            Console.WriteLine($"Сайт:     {user.Website}");
            Console.WriteLine($"Адреса:   {user.Address.Street}, {user.Address.Suite}, {user.Address.City}");
        }
        else
        {
            Console.WriteLine("Користувача з таким ID не знайдено.");
        }
    }
}
catch (Exception ex) { Console.WriteLine($"Помилка: {ex.Message}"); }

public record Address(string Street, string Suite, string City);
public record User(int Id, string Name, string Username, string Email, Address Address, string Phone, string Website);

//Створіть утиліту для роботи з повідомленнями з тестового сервера https://jsonplaceholder.typicode.com/posts. Програма повинна отримати список постів через GET-запит, після чого запросити у користувача значення UserId.
//Далі необхідно відфільтрувати всі отримані записи, залишивши лише ті, які належать введеному користувачу. У результаті потрібно вивести список знайдених постів із зазначенням ID поста та його заголовка (Title).
//Наприкінці необхідно додатково вивести загальну кількість знайдених постів для цього користувача.

using System.Net.Http.Json;

using HttpClient client = new();

try
{
    Console.WriteLine("Отримання постів із сервера...");
    var posts = await client.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");

    if (posts == null) return;

    Console.Write("Введіть UserId для пошуку постів: ");
    if (int.TryParse(Console.ReadLine(), out int targetUserId))
    {
        var filteredPosts = posts.Where(p => p.UserId == targetUserId).ToList();

        if (filteredPosts.Any())
        {
            Console.WriteLine($"\nЗнайдено пости користувача #{targetUserId}:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"ID",-5} | {"Заголовок (Title)"}");
            Console.WriteLine(new string('-', 60));

            foreach (var post in filteredPosts)
            {
                Console.WriteLine($"{post.Id,-5} | {post.Title}");
            }

            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Усього постів для користувача {targetUserId}: {filteredPosts.Count}");
        }
        else
        {
            Console.WriteLine($"Постів для користувача з ID {targetUserId} не знайдено.");
        }
    }
    else
    {
        Console.WriteLine("Помилка: введено некоректний ID.");
    }
}
catch (Exception ex) { Console.WriteLine($"Виникла помилка: {ex.Message}"); }

public record Post(int Id, int UserId, string Title, string Body);