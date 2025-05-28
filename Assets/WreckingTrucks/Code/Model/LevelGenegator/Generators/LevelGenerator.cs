using System.Collections.Generic;

public class LevelGenerator
{
    private BlocksGenerator _blocksGenerator;
    private TruckGenerator _trucksGenerator;

    public LevelGenerator(int amountColumnsForBlocks, int amountColumnsForTrucks)
    {
        _blocksGenerator = new BlocksGenerator(amountColumnsForBlocks);
        _trucksGenerator = new TruckGenerator(amountColumnsForTrucks);
    }

    public void AddTypeBlock<T>() where T : Block
    {
        _blocksGenerator.AddTypeBlock<T>();
    }

    public void AddTypeTruck<T>() where T : Truck
    {
        _trucksGenerator.AddTypeModel<T>();
    }

    public List<Row> GetRowsBlocks(int amountRows)
    {
        return _blocksGenerator.GetRows(amountRows);
    }

    public List<Row> GetRowsTrucks(int amountRows)
    {
        return _trucksGenerator.GetRows(amountRows);
    }
}