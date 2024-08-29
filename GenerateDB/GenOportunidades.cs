using MockData;
using static MockData.Field;
using static MockData.Table;

namespace GenerateDB;

internal class GenOportunidades
{
	public void Generate()
	{
		var tabOportunidades = new Table("Oportunidades");
		Field[] fieldsConfig = new Field[]
		{
			new("IdOportunidade", FieldType.SEQUENTIAL),
			new("IdCliente", FieldType.NUMBER_BETWEEN, values: new object[] { 1, 300 }),
			new("NomeOportunidade", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetNomeOportunidade(generatedValues["IdOportunidade"].ToString())),
			new("ValorEstimado", FieldType.NUMBER_BETWEEN, values: new object[] { 200, 20000 }),
			new("DataCriacao", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetRandomDate(new DateTime(2020, 1, 1), new DateTime(2024, 7, 31))),
			new("DataFechamento", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetDataFechamento(generatedValues["DataCriacao"]?.ToString())),
			new("EtapaOportunidade", FieldType.ONE_OF, values: new object[] { "Leads", "Prospecção", "Qualificação", "Negociação", "Fechamento", "Perdidos" }, probability: new double[] { 0.38, 0.18, 0.14, 0.12, 0.10, 0.08 }),
			new("IdOperador", FieldType.ONE_OF, values: Enumerable.Range(1, 267).Cast<object>().ToArray()), // Supondo que IdOperador refere-se a contatos
            new("DataPerda", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetDataPerda(generatedValues["EtapaOportunidade"]?.ToString(), generatedValues["DataFechamento"]?.ToString())),
			new("IdMotivoPerda", FieldType.ONE_OF, values: Enumerable.Range(1, 12).Cast<object>().ToArray()),
			new("IdFilial", FieldType.ONE_OF, values: new object[] { 1, 2, 3, 4 }),
			new("Origem", FieldType.ONE_OF, values: new string[] {"Indicação", "Site", "Instagram", "Whatsapp", "Outros" }, probability: new double[] {0.07, 0.13, 0.27, 0.33, 0.2 } )
		};

		foreach (var field in fieldsConfig)
		{
			tabOportunidades.AddColumn(field);
		}

		tabOportunidades.CreateFile(new CreateFileConfig() { Path = @"c:\temp\oportunidades.csv", Delimiter = ";", RowCount = 100000 });
	}

	private static string GetNomeOportunidade(string idOportunidade)
	{
		// Gera um nome fictício para a oportunidade
		return $"Oportunidade-{idOportunidade}";
	}

	private static string GetDataFechamento(string dataCriacao)
	{
		Random rnd = new Random();
		if (rnd.NextDouble() <= 0.3)
		{
			return null; // 30% dos casos terão DataFechamento nula
		}
		else
		{
			DateTime creationDate = DateTime.Parse(dataCriacao);
			int daysToAdd = rnd.Next(10, 91); // Adiciona entre 10 e 90 dias
			return creationDate.AddDays(daysToAdd).ToString("yyyy-MM-dd");
		}
	}

	private static string GetRandomDate(DateTime start, DateTime end)
	{
		Random rnd = new Random();
		int range = (end - start).Days;
		return start.AddDays(rnd.Next(range)).ToString("yyyy-MM-dd");
	}

	private static string GetDataPerda(string status, string dataFechamento)
	{
		if (status == "Perdidos" && dataFechamento != null)
		{
			return dataFechamento;
		}
		return null;
	}
}
