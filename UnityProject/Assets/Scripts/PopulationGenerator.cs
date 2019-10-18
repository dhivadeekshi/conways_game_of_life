using System;
using System.Collections.Generic;

public class PopulationGenerator {

    public static List<TileLocation> GetPopulationLoaf()
    {
        // Sample initial population for 6x6 board - loaf
        List<TileLocation> population = new List<TileLocation>();
        population.Add(new TileLocation(4, 2));
        population.Add(new TileLocation(3, 3));
        population.Add(new TileLocation(5, 3));
        population.Add(new TileLocation(2, 4));
        population.Add(new TileLocation(5, 4));
        population.Add(new TileLocation(3, 5));
        population.Add(new TileLocation(4, 5));
        return population;
    }

    public static List<TileLocation> GetPopulationToad()
    {
        // Sample initial population for 6x6 board - toad
        List<TileLocation> population = new List<TileLocation>();
        population.Add(new TileLocation(4, 2));
        population.Add(new TileLocation(5, 2));
        population.Add(new TileLocation(4, 3));
        population.Add(new TileLocation(5, 3));
        population.Add(new TileLocation(2, 4));
        population.Add(new TileLocation(3, 4));
        population.Add(new TileLocation(2, 5));
        population.Add(new TileLocation(3, 5));
        return population;
    }

    public static HashSet<int> GetRandomPopulation(int rows, int cols, int maxElements = 0)
    {
        HashSet<int> population = new HashSet<int>();

        var random = new Random();
        if (maxElements == 0)
            maxElements = random.Next(1, rows * cols);

        while(population.Count < maxElements)
            population.Add(random.Next(rows * cols));

        return population;
    }
}
