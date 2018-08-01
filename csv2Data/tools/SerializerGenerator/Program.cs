
using dbc;
using ProtoBuf.Meta;
namespace dbc
{
	public class Program
	{
		static void Main()
		{
		var model = TypeModel.Create();
		model.Compile("WdjDTOSerializer", "WdjDTOSerializer.dll");

        }
	}
}