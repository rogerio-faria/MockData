using MockData;
using static MockData.Field;
using static MockData.Table;

namespace GenerateDB;

internal class GenPedidos
{
	public void Generate()
	{
		var tabPedidos = new Table("Pedidos");
		Field[] fieldsConfig = new Field[]
		{
			new("IdPedido", FieldType.SEQUENTIAL),
			new("IdCliente", FieldType.ONE_OF, values: Enumerable.Range(1, 260).Cast<object>().ToArray()),
			new("DataPedido", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetRandomDate(new DateTime(2020, 1, 1), new DateTime(2024, 7, 31))),
			new("FormaPagamento", FieldType.ONE_OF, values: new object[] { "Boleto", "Cartão", "PIX", "Dinheiro" }, probability: new double[] { 0.10, 0.56, 0.30, 0.04 }),
			new("IdVendedor", FieldType.ONE_OF, values: Enumerable.Range(261, 40).Cast<object>().ToArray()), // Assuming IdVendedor is 261-300 in the Contatos table
            new("IdFilial", FieldType.ONE_OF, values: new object[] { 1, 2, 3, 4 })
		};

		foreach (var field in fieldsConfig)
		{
			tabPedidos.AddColumn(field);
		}

		tabPedidos.CreateFile(new CreateFileConfig() { Path = @"c:\temp\pedidos.csv", Delimiter = ";", RowCount = 100000 });
	}

	private static string GetRandomDate(DateTime start, DateTime end)
	{
		Random rnd = new Random();
		int range = (end - start).Days;
		return start.AddDays(rnd.Next(range)).ToString("yyyy-MM-dd");
	}
}
