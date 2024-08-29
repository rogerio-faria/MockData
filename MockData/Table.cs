namespace MockData;

public class Table
{
	public class CreateFileConfig
	{
		public string Path { get; set; }
		public string Delimiter { get; set; } = ",";
		public int RowCount { get; set; } = 1000;
		public bool RowsBasedOnFieldValues { get; set; } = false;
		public string RowsBasedOnField { get; set; }
	}

	public string TableName { get; }
	public List<Field> Columns { get; set; } = new List<Field>();

	public Table(string tableName)
	{
		TableName = tableName;
	}

	public void AddColumn(Field column)
	{
		Columns.Add(column);
	}

	public void CreateFile(CreateFileConfig config)
	{
		if (!Directory.Exists(Path.GetDirectoryName(config.Path)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(config.Path));
		}

		var lines = new List<string>();
		var header = string.Join(config.Delimiter, Columns.Where(c => !c.IsTemporary).Select(c => c.Name));
		lines.Add(header);

		int rowCount = config.RowCount;
		List<object> fieldValues = null;

		if (config.RowsBasedOnFieldValues)
		{
			var field = Columns.FirstOrDefault(c => c.Name == config.RowsBasedOnField);
			if (field != null && field.Values != null)
			{
				fieldValues = field.Values.ToList();
				rowCount = fieldValues.Count;
			}
		}

		for (int i = 0; i < rowCount; i++)
		{
			var row = new List<string>();
			var generatedValues = new Dictionary<string, object>();

			foreach (var column in Columns)
			{
				object value = null;

				if (config.RowsBasedOnFieldValues && column.Name == config.RowsBasedOnField && fieldValues != null)
				{
					value = fieldValues[i];
				}
				else
				{
					value = column.GenerateValue(i, generatedValues);
				}

				generatedValues[column.Name] = value;

				if (!column.IsTemporary)
				{
					row.Add(value?.ToString());
				}
			}

			lines.Add(string.Join(config.Delimiter, row));
		}

		File.WriteAllLines(config.Path, lines);
	}
}
