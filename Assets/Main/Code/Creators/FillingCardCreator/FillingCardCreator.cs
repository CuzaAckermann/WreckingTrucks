using System;

public abstract class FillingCardCreator<M, TC, FS> where M : Model
                                                    where TC : ITypeConverter
                                                    where FS : FieldSettings
{
    protected readonly ModelProduction<M> ModelProduction;
    protected readonly TC TypeConverter;

    public FillingCardCreator(ModelProduction<M> modelProduction,
                              TC typeConverter)
    {
        ModelProduction = modelProduction ?? throw new ArgumentNullException(nameof(modelProduction));
        TypeConverter = typeConverter ?? throw new ArgumentNullException(nameof(BlockTypeConverter));
    }

    public FillingCard Create(FS fieldSettings)
    {
        FillingCard fillingCard = new FillingCard(fieldSettings.FieldSize.AmountLayers,
                                                  fieldSettings.FieldSize.AmountColumns,
                                                  fieldSettings.FieldSize.AmountRows);

        FillFillingCard(fillingCard, fieldSettings);

        return fillingCard;
    }

    protected abstract void FillFillingCard(FillingCard fillingCard, FS fieldSettings);
}