# Question

yes, I'd like to show different main menus for users based on their roles (passenger, driver, manager), but not essentially all. And I'd like to include a "menu stack" to allow going back.

---

# Answer

Great! You can extend your **Template Method Pattern** with a **State Pattern** and a **Menu Stack** to:

✔ Dynamically show different menus based on user roles.

✔ Allow users to **navigate back** to previous menus.

---

# **🚀 Updated Approach**

1. **Menu Base Class (Template Method Pattern)**
   * Ensures a consistent menu structure.
2. **Menu Manager (State Pattern + Stack)**
   * Determines which menu to load based on user role.
   * Maintains a **menu stack** for navigation.

---

## **1️⃣ Define `MenuBase` (Template Method)**

This class stays mostly the same but  **adds a way to exit the menu** , so it can return to the previous menu.

```csharp
public abstract class MenuBase
{
    public void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            DisplayTitle();
            DisplayOptions();
            HandleUserInput();
        }
    }

    protected abstract void DisplayTitle();
    protected abstract void DisplayOptions();
    protected abstract void HandleUserInput();
}
```

---

## **2️⃣ Create Different Menus for Roles**

Each role-specific menu  **inherits from `MenuBase`** .

#### **Passenger Menu**

```csharp
public class PassengerMenu : MenuBase
{
    protected override void DisplayTitle()
    {
        Console.WriteLine("🚖 Passenger Menu");
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("1. Book a Ride");
        Console.WriteLine("2. View Active Rides");
        Console.WriteLine("3. Go Back");
    }

    protected override void HandleUserInput()
    {
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Booking a new ride...");
                Console.ReadLine();
                break;
            case "2":
                Console.WriteLine("Showing active rides...");
                Console.ReadLine();
                break;
            case "3":
                MenuManager.GoBack();
                break;
            default:
                Console.WriteLine("Invalid option. Press Enter to try again.");
                Console.ReadLine();
                break;
        }
    }
}
```

---

#### **Driver Menu**

```csharp
public class DriverMenu : MenuBase
{
    protected override void DisplayTitle()
    {
        Console.WriteLine("🚗 Driver Menu");
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("1. View Available Rides");
        Console.WriteLine("2. Manage Current Rides");
        Console.WriteLine("3. Go Back");
    }

    protected override void HandleUserInput()
    {
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Showing available rides...");
                Console.ReadLine();
                break;
            case "2":
                Console.WriteLine("Managing current rides...");
                Console.ReadLine();
                break;
            case "3":
                MenuManager.GoBack();
                break;
            default:
                Console.WriteLine("Invalid option. Press Enter to try again.");
                Console.ReadLine();
                break;
        }
    }
}
```

---

#### **Manager Menu**

```csharp
public class ManagerMenu : MenuBase
{
    protected override void DisplayTitle()
    {
        Console.WriteLine("📊 Manager Menu");
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("1. View Reports");
        Console.WriteLine("2. Manage Drivers & Passengers");
        Console.WriteLine("3. Go Back");
    }

    protected override void HandleUserInput()
    {
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Showing reports...");
                Console.ReadLine();
                break;
            case "2":
                Console.WriteLine("Managing users...");
                Console.ReadLine();
                break;
            case "3":
                MenuManager.GoBack();
                break;
            default:
                Console.WriteLine("Invalid option. Press Enter to try again.");
                Console.ReadLine();
                break;
        }
    }
}
```

---

## **3️⃣ Menu Manager (State Pattern + Stack)**

A **central class to manage navigation** between menus dynamically.

```csharp
public static class MenuManager
{
    private static Stack<MenuBase> menuStack = new Stack<MenuBase>();

    public static void Start(string userRole)
    {
        MenuBase startMenu = userRole switch
        {
            "Passenger" => new PassengerMenu(),
            "Driver" => new DriverMenu(),
            "Manager" => new ManagerMenu(),
            _ => throw new ArgumentException("Invalid role")
        };

        menuStack.Push(startMenu);
        startMenu.ShowMenu();
    }

    public static void NavigateTo(MenuBase menu)
    {
        menuStack.Push(menu);
        menu.ShowMenu();
    }

    public static void GoBack()
    {
        if (menuStack.Count > 1)
        {
            menuStack.Pop();
            menuStack.Peek().ShowMenu();
        }
        else
        {
            Console.WriteLine("Exiting application...");
            Environment.Exit(0);
        }
    }
}
```

---

## **4️⃣ Main Program (Entry Point)**

Now, when the app starts, it asks for the **user role** and loads the correct menu.

```csharp
class Program
{
    static void Main()
    {
        Console.Write("Enter role (Passenger / Driver / Manager): ");
        string role = Console.ReadLine();

        try
        {
            MenuManager.Start(role);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

---

## **5️⃣ How This Works**

| Feature                           | Implementation                                                  |
| --------------------------------- | --------------------------------------------------------------- |
| **Template Method Pattern** | `MenuBase`defines the menu structure.                         |
| **State Pattern**           | `MenuManager`dynamically selects the menu based on user role. |
| **Navigation Stack**        | `MenuManager`maintains history for going back.                |

---

## **🚀 Key Benefits**

✅ **Dynamically loads the correct menu** → No need for hardcoded role checks in every menu.

✅ **Maintains a menu history stack** → Users can **go back** instead of getting lost.

✅ **Encapsulates navigation logic in `MenuManager`** → Cleaner and  **scalable** .

---

## **🔥 Next Steps**

Would you like to extend this by:

1. **Storing user roles in a database** instead of asking every time?
2. **Adding role-based permissions** (e.g., only Managers can see reports)?
3. **Persisting menu state** so users can resume where they left off?

Let me know how you'd like to improve this further! 🚀
