# Question

For the console app, can and should I apply the Template design pattern (or a more suitable one) to create menus with navigation between them. How will it be applied?

---

# Answer

Yes! You can **and should** apply the **Template Method pattern** to structure your console app's menu navigation. It helps by providing a **common flow for menus** while allowing specific menu screens to define their unique content.

---

## **1. Why Use the Template Method Pattern for Console Menus?**

### ✅ **Benefits:**

✔ **Encapsulates menu logic** → Avoids duplicate code across multiple menus.

✔ **Improves maintainability** → New menus can be added easily by extending a base class.

✔ **Ensures consistent menu flow** → Common structure for user interactions.

---

## **2. How to Apply the Template Method Pattern**

### **Step 1: Define an Abstract Base Class for Menus**

This class contains **the template method** defining the general flow of any menu.

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

    protected abstract void DisplayTitle();   // Unique to each menu
    protected abstract void DisplayOptions(); // Unique to each menu
    protected abstract void HandleUserInput(); // Unique input handling per menu
}
```

---

### **Step 2: Implement Specific Menus by Extending `MenuBase`**

Each menu **inherits from `MenuBase`** and defines its unique behavior.

#### **Main Menu**

```csharp
public class MainMenu : MenuBase
{
    protected override void DisplayTitle()
    {
        Console.WriteLine("==== Main Menu ====");
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("1. Ride Management");
        Console.WriteLine("2. User Profile");
        Console.WriteLine("3. Exit");
    }

    protected override void HandleUserInput()
    {
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                new RideMenu().ShowMenu(); // Navigate to Ride Menu
                break;
            case "2":
                new UserProfileMenu().ShowMenu(); // Navigate to User Profile Menu
                break;
            case "3":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option. Press Enter to try again.");
                Console.ReadLine();
                break;
        }
    }
}
```

#### **Ride Management Menu**

```csharp
public class RideMenu : MenuBase
{
    protected override void DisplayTitle()
    {
        Console.WriteLine("==== Ride Management ====");
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("1. View Active Rides");
        Console.WriteLine("2. Book a New Ride");
        Console.WriteLine("3. Return to Main Menu");
    }

    protected override void HandleUserInput()
    {
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Displaying active rides...");
                Console.ReadLine();
                break;
            case "2":
                Console.WriteLine("Booking a new ride...");
                Console.ReadLine();
                break;
            case "3":
                return; // Return to Main Menu
            default:
                Console.WriteLine("Invalid option. Press Enter to try again.");
                Console.ReadLine();
                break;
        }
    }
}
```

#### **User Profile Menu**

```csharp
public class UserProfileMenu : MenuBase
{
    protected override void DisplayTitle()
    {
        Console.WriteLine("==== User Profile ====");
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("1. View Profile");
        Console.WriteLine("2. Edit Profile");
        Console.WriteLine("3. Return to Main Menu");
    }

    protected override void HandleUserInput()
    {
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Displaying user profile...");
                Console.ReadLine();
                break;
            case "2":
                Console.WriteLine("Editing user profile...");
                Console.ReadLine();
                break;
            case "3":
                return; // Return to Main Menu
            default:
                Console.WriteLine("Invalid option. Press Enter to try again.");
                Console.ReadLine();
                break;
        }
    }
}
```

---

### **Step 3: Start the Console App**

Your `Main` method initializes the `MainMenu`:

```csharp
class Program
{
    static void Main()
    {
        new MainMenu().ShowMenu();
    }
}
```

---

## **3. Where is the Template Method Pattern Applied?**

* The **`MenuBase` class** is the  **template method** , defining the general structure of menus.
* **Subclasses override only necessary parts** , like `DisplayTitle`, `DisplayOptions`, and `HandleUserInput`.
* This ensures  **code reuse and consistent menu structure** .

---

## **4. Alternative Approach: State Pattern for More Complex Navigation**

If your console app needs  **dynamic state changes (e.g., based on user roles or actions)** , you can combine the **State Pattern** with the Template Method.

* The **State Pattern** would allow a **MenuManager** to manage the current menu dynamically.
* The **Template Method** still ensures each menu follows a structured format.

---

## **5. Conclusion: Should You Use This?**

✅ **YES, because:**

✔ Avoids redundant menu logic.

✔ Makes adding new menus  **easy** .

✔ Follows **best design principles** for maintainability.

🚀 **Would you like to extend this with dynamic navigation or external APIs?**
