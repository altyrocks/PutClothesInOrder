public class PutClothesInOrder
{
    public static void Main()
    {
        // initial input contains list of items and what they are dependent on
        var input = new string[,]
        {
            {"t-shirt","dress shirt"},
            {"dress shirt","pants"},
            {"dress shirt","suit jacket"},
            {"tie","suit jacket"},
            {"pants","suit jacket"},
            {"belt","suit jacket"},
            {"suit jacket","overcoat"},
            {"dress shirt","tie"},
            {"suit jacket","sun glasses"},
            {"sun glasses","overcoat"},
            {"left sock","pants"},
            {"pants","belt"},
            {"suit jacket","left shoe"},
            {"suit jacket","right shoe"},
            {"left shoe","overcoat"},
            {"right sock","pants"},
            {"right shoe","overcoat"},
            {"t-shirt","suit jacket"}
        };

        var clothesList = OrganizeClothing(input);

        // output results to console
        foreach (var item in clothesList)
        {
            Console.WriteLine(string.Join(", ", item));
        }
    }

    public static List<List<string>> OrganizeClothing(string[,] input)
    {
        // dictionary list of clothing items and how many dependencies they have
        var countOfDependencies = new Dictionary<string, List<string>>();
        // dictionary list of clothing items and if they have dependencies or not
        var hasDependencies = new Dictionary<string, int>();

        // loop through the number of rows
        for (int i = 0; i < input.GetLength(0); i++)
        {
            var dependency = input[i, 0];
            var item = input[i, 1];

            // create new string list if dependency doesn't exist
            if (!countOfDependencies.ContainsKey(dependency))
                countOfDependencies[dependency] = new List<string>();

            // add dependency to list
            countOfDependencies[dependency].Add(item);

            // zero out amount of dependency is not dependent on anything
            if (!hasDependencies.ContainsKey(dependency))
                hasDependencies[dependency] = 0;

            // zero out amount if item is not dependent on anything
            if (!hasDependencies.ContainsKey(item))
                hasDependencies[item] = 0;

            hasDependencies[item]++;
        }

        return SortClothes(countOfDependencies, hasDependencies);
    }

    private static List<List<string>> SortClothes(Dictionary<string, List<string>> countOfDependencies, Dictionary<string, int> hasDependencies)
    {
        var finalClothingList = new List<List<string>>();
        var queue = new Queue<string>();

        // add clothing items with no dependencies to the queue first
        foreach (var item in hasDependencies)
        {
            if (item.Value == 0)
                queue.Enqueue(item.Key);
        }

        // loop through until all clothing items are accounted for
        while (queue.Count > 0)
        {
            var line = new List<string>();
            int size = queue.Count;

            // loop through items with zero dependencies 
            for (int i = 0; i < size; i++)
            {
                // remove item so queue count is decreased
                var item = queue.Dequeue();

                line.Add(item);

                // is the current item in the list
                if (countOfDependencies.ContainsKey(item))
                {
                    var dependencies = countOfDependencies[item];

                    foreach (var dependency in dependencies)
                    {
                        // remove the dependency once we account for it
                        hasDependencies[dependency]--;

                        // if item has no dependencies left, add it to the queue
                        if (hasDependencies[dependency] == 0)
                            queue.Enqueue(dependency);
                    }
                }
            }

            // sort the line
            line.Sort();

            // add the line to the final list
            finalClothingList.Add(line);
        }

        return finalClothingList;
    }
}