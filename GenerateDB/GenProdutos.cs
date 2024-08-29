using MockData;
using static MockData.Field;
using static MockData.Table;


namespace GenerateDB;

internal class GenProdutos
{
	public void Generate()
	{
		var tabProdutos = new Table("Produtos");
		Field[] fieldsConfig = new Field[]
		{
			new("IdProduto", FieldType.SEQUENTIAL),
			new("Cod1", FieldType.ONE_OF, values: Enumerable.Range(1, 8).Cast<object>().ToArray()), // Categoria
            new("Cod2", FieldType.ONE_OF, values: Enumerable.Range(1, 21).Cast<object>().ToArray()), // SubCategoria
            new("Cod3", FieldType.ONE_OF, values: new object[] { 1, 2, 3 }, probability: new double[] {0.3,0.6,0.1} ), // Masculino, Feminino, Unisex
            new("Cod4", FieldType.ONE_OF, values: Enumerable.Range(1, 8).Cast<object>().ToArray()), // Cor
            new("Cod5", FieldType.ONE_OF, values: Enumerable.Range(1, 8).Cast<object>().ToArray()), // Numeração
            new("Cod6", FieldType.ONE_OF, values: Enumerable.Range(1, 8).Cast<object>().ToArray()), // Estilo
        };

		foreach (var field in fieldsConfig)
		{
			tabProdutos.AddColumn(field);
		}

		tabProdutos.CreateFile(new CreateFileConfig() { Path = @"c:\temp\produtos.csv", Delimiter = ";", RowCount = 500 });
	}
}
