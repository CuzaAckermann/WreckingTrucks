public static class DI
{
    public static Production Production { get; private set; }

    public static void Init(Production production)
    {
        Validator.ValidateNotNull(production);

        Production = production;
    }
}