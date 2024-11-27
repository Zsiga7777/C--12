public class CodeGenerator : ICodeWriter
{
	private readonly Guid Guid = Guid.NewGuid();

	public string GetCode()
	{
		return $"code: {this.Guid}";
	}
}