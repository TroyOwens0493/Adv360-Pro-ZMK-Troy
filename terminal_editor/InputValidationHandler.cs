class InputValidationHandler
{
    public int GetIntFromUser(int min, int max)
    {
        bool running = true;
        string? userSelection;
        int selectedInt = 0;

        do
        {
            userSelection = Console.ReadLine();

            if (string.IsNullOrEmpty(userSelection))
            {
                Console.WriteLine("Please enter something");
                continue;
            }
            try
            {
                selectedInt = int.Parse(userSelection);
                if ((selectedInt > max) || (selectedInt < min))
                {
                    Console.WriteLine("Please enter a valid number");
                    continue;
                }
            }
            catch
            {
                Console.WriteLine("Please enter a valid number");
                continue;
            }

            running = false;
        } while (running);

        return selectedInt;
    }

    public string GetStringFromUser()
    {
        string? res = "";
        do
        {
            res = Console.ReadLine();
        } while (res != "");

        return res;
    }
}
