using GeneralArchitectureTests;

namespace ArchitectureTests;

public class GeneralTest
{
	[Fact]
	public void General()
	{
		var architectureTester = new ArchitectureTester(
			new ArchitectureTestConfig(),
			new ArchitectureTestAssemblies(
				domainAssembly: Domain.AssemblyReference.Assembly,
				applicationAssembly: Application.AssemblyReference.Assembly,
				presentationAssembly: Presentation.AssemblyReference.Assembly,
				infrastructureAssembly: Infrastructure.AssemblyReference.Assembly,
				AvtMedia.CleanArchitecture.DomainLayer.AssemblyReference.Assembly,
				AvtMedia.CleanArchitecture.ApplicationLayer.AssemblyReference.Assembly,
				AvtMedia.CleanArchitecture.InfrastructureLayer.AssemblyReference.Assembly,
				AvtMedia.CleanArchitecture.PresentationLayer.AssemblyReference.Assembly,
				AvtMedia.GeneralLibrary.AssemblyReference.Assembly
			)
		);

		architectureTester.RunTests();
	}
}
