using System;

public abstract class FillingCardCreator<M> where M : Model
{
    protected readonly ModelFactory<M> ModelFactory;

    public FillingCardCreator(ModelFactory<M> modelFactory)
    {
        ModelFactory = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));
    }

    public FillingCard Create(FieldSize fieldSize)
    {
        FillingCard fillingCard = new FillingCard(fieldSize.AmountLayers,
                                                  fieldSize.AmountColumns,
                                                  fieldSize.AmountRows);

        FillFillingCard(fillingCard);

        return fillingCard;
    }

    protected abstract void FillFillingCard(FillingCard fillingCard);
}