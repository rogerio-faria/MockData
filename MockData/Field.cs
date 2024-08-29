namespace MockData;

public class Field
{
	public enum FieldType
	{
		SEQUENTIAL,
		ONE_OF,
		DELEGATE,
		NUMBER_BETWEEN,
		RANDOM_STRING
	}

	public string Name { get; }
	public FieldType Type { get; }
	public object[] Values { get; }
	public Func<Dictionary<string, object>, object> DelegateFunction { get; }
	public double[] Probabilities { get; }
	public bool IsTemporary { get; }

	public Field(
		string name,
		FieldType type,
		object[] values = null,
		Func<Dictionary<string, object>, object> delegateFunction = null,
		double[] probability = null,
		bool isTemporary = false)
	{
		Name = name;
		Type = type;
		Values = values;
		DelegateFunction = delegateFunction;
		Probabilities = probability;
		IsTemporary = isTemporary;
	}

	public object GenerateValue(int index, Dictionary<string, object> generatedValues)
	{
		switch (Type)
		{
			case FieldType.SEQUENTIAL:
				return index + 1;

			case FieldType.ONE_OF:
				return GenerateValueWithProbability(index);

			case FieldType.DELEGATE:
				return DelegateFunction?.Invoke(generatedValues);

			case FieldType.NUMBER_BETWEEN:
				return Utils.RandomBetween((int)Values[0], (int)Values[1]);

			case FieldType.RANDOM_STRING:
				int length = Convert.ToInt32(Values[0]);
				string chars = Convert.ToString(Values[1]);
				return Utils.RandomString(length, chars);

			default:
				return null;
		}
	}

	private object GenerateValueWithProbability(int index)
	{
		if (Probabilities == null || Probabilities.Length != Values.Length)
		{
			return Values[new Random().Next(Values.Length)];
		}

		double cumulativeProbability = 0.0;
		double randomValue = new Random().NextDouble();

		for (int i = 0; i < Values.Length; i++)
		{
			cumulativeProbability += Probabilities[i];
			if (randomValue <= cumulativeProbability)
			{
				return Values[i];
			}
		}

		return Values.Last();
	}
}
