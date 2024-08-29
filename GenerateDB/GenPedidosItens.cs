using MockData;
using static MockData.Field;
using static MockData.Table;

namespace GenerateDB;

internal class GenPedidosItens
{
	public void Generate()
	{
		var tabPedidosItens = new Table("PedidosItens");
		Field[] fieldsConfig = new Field[]
		{
			new("IdItemPedido", FieldType.SEQUENTIAL),
			new("IdPedido", FieldType.ONE_OF, values: Enumerable.Range(1, 100000).Cast<object>().ToArray()),
			new("IdProduto", FieldType.ONE_OF, values: Enumerable.Range(1, 500).Cast<object>().ToArray()),
			new("Quantidade", FieldType.ONE_OF, values: new object[] { 1, 2, 3, 4, 5, 6, 7, 8 }, probability: new double[] { 0.30, 0.40, 0.15, 0.05, 0.04, 0.03, 0.02, 0.01 }),
			new("PrecoUnitario", FieldType.NUMBER_BETWEEN, values: new object[] { 89, 599 }),
			new("ValorDesconto", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetValorDesconto(generatedValues["PrecoUnitario"]))
		};

		foreach (var field in fieldsConfig)
		{
			tabPedidosItens.AddColumn(field);
		}

		tabPedidosItens.CreateFile(new CreateFileConfig() { Path = @"c:\temp\pedidos_itens.csv", Delimiter = ";", RowCount = 400000 });
	}

	private static double GetValorDesconto(object precoUnitario)
	{
		Random rnd = new Random();
		double preco = Convert.ToDouble(precoUnitario);
		double descontoPercentual = rnd.Next(2, 16); // Percentual entre 2% e 15%
		return Math.Round(preco * (descontoPercentual / 100), 2); // Calcula o valor do desconto
	}
}
