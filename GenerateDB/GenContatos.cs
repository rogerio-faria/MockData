using MockData;
using static MockData.Field;
using static MockData.Table;

namespace GenerateDB;

internal class GenContatos
{
	public void Generate()
	{
		var tabContatos = new Table("Contatos");
		Field[] fieldsConfig = new Field[]
		{
			new("id", FieldType.SEQUENTIAL),
			new("nometemp", FieldType.ONE_OF, values: GetNomes(), isTemporary: true),
			new("nome", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetNome(generatedValues["nometemp"]?.ToString())),
			new("email", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetEmailFromName(generatedValues["nome"]?.ToString())),
			new("status", FieldType.ONE_OF, values: new object[] { "Ativo", "Inativo" }, probability: new double[] {0.8, 0.2 }),
			new("tipo_contato", FieldType.ONE_OF, values: new object[] { "Cliente", "Fornecedor" }, probability: new double[] {0.7, 0.3}),
			new("faixa_renda", FieldType.NUMBER_BETWEEN, values: new object[] { 1, 6 }),

			new("CPF_CNPJ", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetCPF(generatedValues["nometemp"]?.ToString())),
			new("sexo", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetSexo(generatedValues["nometemp"]?.ToString())),
			new("data_nasc", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetNasc(generatedValues["nometemp"]?.ToString())),
			new("segmento", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetSegmento(generatedValues["nometemp"]?.ToString())),

			new("fone", FieldType.RANDOM_STRING, values: new object[] { 11, "0123654789" }),

			new("ceptemp", FieldType.ONE_OF, values: GetCEPs(), isTemporary: true),
			new("cep", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetCEP(generatedValues["ceptemp"]?.ToString())),
			new("cidade", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetCidade(generatedValues["ceptemp"]?.ToString())),
			new("UF", FieldType.DELEGATE, delegateFunction: (generatedValues) => GetUF(generatedValues["ceptemp"]?.ToString())),


		};

		foreach (var field in fieldsConfig)
		{
			tabContatos.AddColumn(field);
		}

		//tabContatos.CreateFile(new CreateFileConfig() { Path = @"c:\temp\contatos.csv", Delimiter = ";", RowCount = 10000 });
		tabContatos.CreateFile(new CreateFileConfig() { Path = @"c:\temp\contatos.csv", Delimiter = ";", RowsBasedOnFieldValues = true, RowsBasedOnField = "nometemp" });

	}

	private object GetUF(string? ceptemp)
	{
		return ceptemp.Split('|')[2];
	}

	private object GetCidade(string? ceptemp)
	{
		return ceptemp.Split('|')[1];
	}

	private object GetCEP(string? ceptemp)
	{
		return ceptemp.Split('|')[0];
	}

	private object[] GetCEPs()
	{
		return File.ReadLines("dbCEPS.txt").ToArray();
	}

	private static object GetSexo(string? nometemp)
	{
		string attribs = nometemp.Split('-')[1];
		return attribs.Split('|')[1];
	}

	private static object GetSegmento(string? nometemp)
	{
		string attribs = nometemp.Split('-')[1];
		return attribs.Split('|')[3];
	}

	private static object GetNasc(string? nometemp)
	{
		string attribs = nometemp.Split('-')[1];
		return attribs.Split('|')[2];
	}

	private static object GetCPF(string? nometemp)
	{
		string tipo = nometemp.Split('-')[1];
		if (tipo.StartsWith("CPF"))
			return Utils.RandomString(11, "0123456789");
		else
			return Utils.RandomString(8, "0123456789") + "0001" + Utils.RandomString(2, "0123456789");
	}

	public static string[] GetNomes()
	{
		return File.ReadLines("dbContatos.txt").ToArray();
	}

	public static string GetNome(string nometemp)
	{
		// Retorna o nome extraído antes do "-CPF"
		return nometemp?.Split('-')[0];
	}

	public static string GetEmailFromName(string name)
	{
		var dominios = new[] { "company.com", "gmail.com", "outlook.com", "hotmail.com", "msn.com", "mail.ru", "icloud.com", "facebook.com", "yahoo.com", "proton.com", "zoho.com", "yandex.com" };
		var prefix = $"{name.Split(' ').First().ToLower()}.{name.Split(' ').Last().ToLower()}@";

		int i = new Random().Next(0, dominios.Length);

		return Utils.HigienizarString(prefix + dominios[i]);
	}
}
